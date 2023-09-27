using System.Runtime.CompilerServices;
using System.Text;
using LlamaModel = System.IntPtr;
using NativeLLamaContext = System.IntPtr;
using LlamaToken = System.Int32;
using static OwnerGPT.LLM.Native.NativeLLamaInteroperability;
using Microsoft.Extensions.Options;
using OwnerGPT.LLM.Configuration;
using System.Reflection;
using LLama.Exceptions;
using LLama.Common;
using static System.Net.Mime.MediaTypeNames;
using LLama.Abstractions;
using LLama.Native;
using System.Runtime.InteropServices;
using System;
using LLama;

namespace OwnerGPT.LLM.Native
{
    public class NativeLLamaModel : IDisposable
    {
        private LlamaModel Model;
        private NativeLLamaContext Context;
        public NativeLLamaContext Handle => Context;

        private NativeLLamaOptions _options = new();
        public NativeLLamaOptions Options { get => _options; }

        private byte[]? InitialState;
        internal byte[] GetInitialState() => InitialState ?? new byte[0];

        internal byte[] GetRawState() => NativeLLamaInteroperability.llama_copy_state_data(Context);
        internal void SetRawState(byte[] state) => NativeLLamaInteroperability.llama_set_state_data(Context, state);
        public NativeLLamaSession CreateSession() => new NativeLLamaSession(this);

        private int StatelessEvaulateOffset = 0;

        public string UntokenizeToText(IEnumerable<LlamaToken> tokenIds)
        {
            if (!tokenIds.Any())
                return String.Empty;

            var bytes = new List<byte[]>();
            foreach (var tokenId in tokenIds)
            {
                if (tokenId == NativeLLamaInteroperability.llama_token_bos(Context))
                    bytes.Add(Encoding.UTF8.GetBytes("<s>"));
                else if (tokenId == NativeLLamaInteroperability.llama_token_eos(Context))
                    bytes.Add(Encoding.UTF8.GetBytes("</s>"));
                else
                    bytes.Add(NativeLLamaInteroperability.llama_token_to_bytes(Context, tokenId));
            }

            return Encoding.UTF8.GetString(bytes.SelectMany(x => x).ToArray());
        }

        public void ResetState()
        {
            if (InitialState == null)
                return;

            SetRawState(InitialState);
        }

        public void Load(NativeLLamaOptions options)
        {
            if (Context != IntPtr.Zero)
                throw new InvalidOperationException($"Model already loaded.");

            var parameters = this.BuildDefaultParameters();

            Model = NativeLLamaInteroperability.llama_load_model_from_file(ModelConfiguration.LLAMA_MODEL_PATH, parameters);

            if (Model == IntPtr.Zero)
                throw new Exception("Failed to load model!");

            Context = NativeLLamaInteroperability.llama_new_context_with_model(Model, parameters);

            if (Context == IntPtr.Zero)
                throw new RuntimeError("Failed to create context from model");

            InitialState = NativeLLamaInteroperability.llama_copy_state_data(Context);
             
            _options = options;
        }

        private llama_context_params BuildDefaultParameters()
        {
            llama_context_params parameters = NativeLLamaInteroperability.llama_context_default_params();
            parameters.seed = ModelConfiguration.UNCHECKED_SEED_COUNT;
            parameters.n_ctx = ModelConfiguration.CONTEXT_SIZE;
            parameters.n_gpu_layers = ModelConfiguration.GPU_LAYER_COUNT;
            parameters.rope_freq_base = 10000.0f;
            parameters.rope_freq_scale = 1f;
            parameters.low_vram = false;
            parameters.use_mmap = true;
            parameters.use_mlock = false;
            return parameters;
        }


        internal async IAsyncEnumerable<string> GenerateStatlessTokenStringAsync(NativeLlamaGenerateOptions options, List<LlamaToken> tokens, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var bytesBuffer = new List<byte>();

            await foreach (var tokenBytes in AdvancedStatelessGenerateTokenBytesAsync(options, tokens, cancellationToken))
            {
                    yield return tokenBytes;
                    bytesBuffer.Clear();
            }

            if (bytesBuffer.Any())
            {
                yield return Encoding.UTF8.GetString(bytesBuffer.ToArray());
            }
        }

        internal async IAsyncEnumerable<string> AdvancedStatelessGenerateTokenBytesAsync(NativeLlamaGenerateOptions options, List<LlamaToken> tokens, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            int n_past = 1;
            List<LlamaToken> lastTokens = new(0);
            for (int i = 0; i < lastTokens.Count; i++)
            {
                lastTokens[i] = 0;
            }
            int n_prompt_tokens = tokens.Count;

            NativeLLamaInteroperability.llama_eval(
                Context,
                tokens.ToArray(),
                n_prompt_tokens,
                n_past,
                options.ThreadCount
            );

            lastTokens.AddRange(tokens);
            n_past += n_prompt_tokens;
            var mu = (float?)null;
            int max_tokens = int.MaxValue;

            for (int i = 0; i < max_tokens; i++)
            {
                var repeat_last_n = ModelConfiguration.CONTEXT_SIZE;

                var tokenDataArray = this.ApplyPenalty(lastTokens, null, repeat_last_n);

                var id = Sample(tokenDataArray, ref mu);

                lastTokens.Add(id);

                string response = TokenToString(id, Encoding.UTF8);
                yield return response;

                tokens.Clear();
                tokens.Add(id);

                string last_output = "";
                foreach (var token in lastTokens)
                {
                    last_output += TokenToString(token, Encoding.UTF8);
                }

                bool should_break = false;
                foreach (var antiprompt in "Answer:")
                {
                    if (last_output.EndsWith(antiprompt))
                    {
                        should_break = true;
                        break;
                    }
                }
                if (should_break)
                {
                    break;
                }

                if (n_past + tokens.Count > ModelConfiguration.CONTEXT_SIZE)
                {
                    int n_left = n_past - 0;

                    n_past = Math.Max(1, 0);

                    // insert n_left/2 tokens at the start of embed from last_n_tokens
                    tokens.InsertRange(0, lastTokens.Take(lastTokens.Count - tokens.Count).Skip(ModelConfiguration.CONTEXT_SIZE - n_left / 2 - tokens.Count));
                }

                n_past = Eval(tokens.ToArray(), n_past);
            }
        }

        public LlamaToken Eval(LlamaToken[] tokens, LlamaToken pastTokensCount)
        {
            int total = tokens.Length;
            for (int i = 0; i < total; i += 128)
            {
                int n_eval = total - i;
                if (n_eval > 128)
                {
                    n_eval = 128;
                }

                if (!EEval(tokens, pastTokensCount, 12))
                {
                    throw new RuntimeError("Failed to eval.");
                }

                pastTokensCount += n_eval;
            }
            return pastTokensCount;
        }

        public bool EEval(LlamaToken[] tokens, int n_past, int n_threads)
        {
            unsafe
            {
                return NativeLLamaInteroperability.llama_eval(Context, tokens, tokens.Length, n_past, n_threads) == 0;
            }
        }

        public string TokenToString(int llama_token, Encoding encoding)
        {
            var span = TokenToSpan(llama_token);

            if (span.Length == 0)
                return "";

            unsafe
            {
                fixed (byte* ptr = &span[0])
                {
                    return encoding.GetString(ptr, span.Length);
                }
            }
        }

        public ReadOnlySpan<byte> TokenToSpan(LlamaToken llama_token)
        {
            unsafe
            {
                var logits = MemoryMarshal.GetReference<byte>(NativeLLamaInteroperability.llama_token_to_bytes(Context, llama_token));
                var bytes = new Span<byte>(Unsafe.AsPointer<byte>(ref logits), int.MaxValue);
                var terminator = bytes.IndexOf((byte)0);
                return bytes.Slice(0, terminator);
            }
        }

        public LlamaToken Sample(llama_token_data_array candidates, ref float? mirostat_mu, float temperature = 0.8f, MirostatType mirostat = MirostatType.Disable,
                                  float mirostatTau = 5.0f, float mirostatEta = 0.1f, int topK = 40, float topP = 0.95f, float tfsZ = 1.0f, float typicalP = 1.0f)
        {
            LlamaToken id = 0;
            if (temperature <= 0)
            {
                NativeLLamaInteroperability.llama_sample_token_greedy(Context, candidates);
            }
            else
            {
                var mu = mirostat_mu ?? (2 * mirostatTau);
                {
                    if (mirostat == MirostatType.Mirostat)
                    {
                        const int mirostat_m = 100;
                        NativeLLamaInteroperability.llama_sample_temperature(Context, candidates, temperature);

                        id = NativeLLamaInteroperability.llama_sample_token_mirostat(Context, candidates, mirostatTau, mirostatEta, mirostat_m, ref mu);
                    }
                    else if (mirostat == MirostatType.Mirostat2)
                    {
                        NativeLLamaInteroperability.llama_sample_temperature(Context, candidates, temperature);
                        id = NativeLLamaInteroperability.llama_sample_token_mirostat_v2(Context, candidates, mirostatTau, mirostatEta, ref mu);
                    }
                    else
                    {
                        // Temperature sampling
                        NativeLLamaInteroperability.llama_sample_top_k(Context, candidates, topK, 1);
                        NativeLLamaInteroperability.llama_sample_tail_free(Context, candidates, tfsZ, 1);
                        NativeLLamaInteroperability.llama_sample_typical(Context, candidates, typicalP, 1);
                        NativeLLamaInteroperability.llama_sample_top_p(Context, candidates, topP, 1);
                        NativeLLamaInteroperability.llama_sample_temperature(Context, candidates, temperature);
                        id = NativeLLamaInteroperability.llama_sample_token(Context, candidates);
                    }
                }
                mirostat_mu = mu;
            }
            return id;
        }

        public llama_token_data_array ApplyPenalty(IEnumerable<LlamaToken> lastTokens, Dictionary<LlamaToken, float>? logitBias = null,
            int repeatLastTokensCount = 64, float repeatPenalty = 1.1f, float alphaFrequency = .0f, float alphaPresence = .0f, bool penalizeNL = true)
        {
            var n_vocab = NativeLLamaInteroperability.llama_n_vocab(Context);
            var logits = GetLogits(n_vocab);

            // Apply params.logit_bias map
            if (logitBias is not null)
            {
                foreach (var (key, value) in logitBias)
                {
                    logits[key] += value;
                }
            }

            var candidates = new llama_token_data[n_vocab];
            var candidatesData = new LLamaTokenData[n_vocab];

            for (LlamaToken token_id = 0; token_id < n_vocab; token_id++)
            {
                var tokenData = new llama_token_data();

                tokenData.id = token_id;
                tokenData.logit = logits[token_id];
                tokenData.p = 0.0f;

                candidates[token_id] = tokenData;
                candidatesData[token_id] = new LLamaTokenData(token_id, logits[token_id], 0.0f);
            }

            // Apply penalties
            float nl_logit = logits[NativeLLamaInteroperability.llama_token_nl(Context)];
            int lastTokensCount = lastTokens.Count();
            var last_n_repeat = Math.Min(Math.Min(lastTokensCount, repeatLastTokensCount), ModelConfiguration.CONTEXT_SIZE);

            var arrayCandidates = new NativeLLamaInteroperability.llama_token_data_array { data = candidates, size = (nuint)candidates.Length, sorted = false };

            llama_sample_repetition_penalty(Context, arrayCandidates, lastTokens.Skip(lastTokensCount - last_n_repeat).ToArray(),repeatPenalty);

            NativeLLamaInteroperability.llama_sample_repetition_penalty(Context, arrayCandidates, lastTokens.Skip(lastTokensCount - last_n_repeat).ToArray(), repeatPenalty);

            NativeLLamaInteroperability.llama_sample_frequency_and_presence_penalties(Context, arrayCandidates,
                lastTokens.Skip(lastTokensCount - last_n_repeat).ToArray(),
                 alphaFrequency, alphaPresence);

            if (!penalizeNL)
            {
                logits[NativeLLamaInteroperability.llama_token_nl(Context)] = nl_logit;
            }

            return arrayCandidates;
        }

        public Span<float> GetLogits(LlamaToken vocabCount)
        {
            unsafe
            {
                var logits = MemoryMarshal.GetReference(NativeLLamaInteroperability.llama_get_logits(Context));
                return new Span<float>(Unsafe.AsPointer<float>(ref logits), vocabCount);
            }
        }

        internal async IAsyncEnumerable<byte[]> StatelessGenerateTokenBytesAsync(NativeLlamaGenerateOptions options, List<LlamaToken> tokens, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var mirostatMU = 2.0f * options.MirostatTAU;
            StatelessEvaulateOffset = 0;
            while (NativeLLamaInteroperability.llama_get_kv_cache_token_count(Context) < NativeLLamaInteroperability.llama_n_ctx(Context) && !cancellationToken.IsCancellationRequested)
            {
                for (var offset = StatelessEvaulateOffset; offset < tokens.Count && !cancellationToken.IsCancellationRequested; offset += _options.BatchSize)
                {
                    var evalCount = tokens.Count - offset;
                    if (evalCount > _options.BatchSize)
                        evalCount = _options.BatchSize;

                    NativeLLamaInteroperability.llama_eval(
                        Context,
                        tokens.Skip(offset).ToArray(),
                        evalCount,
                        StatelessEvaulateOffset,
                        options.ThreadCount
                    );

                    StatelessEvaulateOffset += evalCount;
                }

                //var logits = NativeLLamaInteroperability.llama_get_logits(_context);
                var n_vocab = NativeLLamaInteroperability.llama_n_vocab(Context);

                var candidates = new NativeLLamaInteroperability.llama_token_data[n_vocab];
                for (LlamaToken tokenId = 0; tokenId < n_vocab && !cancellationToken.IsCancellationRequested; tokenId++)
                    candidates[tokenId] = new NativeLLamaInteroperability.llama_token_data { id = tokenId, logit = NativeLLamaInteroperability.llama_get_logits(Context)[tokenId], p = 0.0f };

                if (cancellationToken.IsCancellationRequested)
                    break;

                var candidates_p = new NativeLLamaInteroperability.llama_token_data_array { data = candidates.ToArray(), size = (nuint)candidates.Length, sorted = false };

                // Apply penalties
                var newLineLogit = NativeLLamaInteroperability.llama_get_logits(Context)[NativeLLamaInteroperability.llama_token_nl(Context)];
                var lastRepeatCount = Math.Min(Math.Min(tokens.Count, options.LastTokenCountPenalty), NativeLLamaInteroperability.llama_n_ctx(Context));

                NativeLLamaInteroperability.llama_sample_repetition_penalty(
                    Context,
                    candidates_p,
                    tokens.Skip(tokens.Count - lastRepeatCount).Take(lastRepeatCount).ToArray(),
                    options.RepeatPenalty
                );

                NativeLLamaInteroperability.llama_sample_frequency_and_presence_penalties(
                    Context,
                    candidates_p,
                    tokens.Skip(tokens.Count - lastRepeatCount).Take(lastRepeatCount).ToArray(),
                    options.FrequencyPenalty,
                    options.PresencePenalty
                );

                if (!options.PenalizeNewLine)
                    NativeLLamaInteroperability.llama_get_logits(Context)[NativeLLamaInteroperability.llama_token_nl(Context)] = newLineLogit;

                var id = default(LlamaToken);

                // Sampling
                if (options.Temperature <= 0.0f)
                {
                    // Greedy
                    id = NativeLLamaInteroperability.llama_sample_token_greedy(Context, candidates_p);
                }
                else if (options.Mirostat == Mirostat.Mirostat)
                {
                    // Mirostat
                    var mirostat_m = 100;
                    NativeLLamaInteroperability.llama_sample_temperature(Context, candidates_p, options.Temperature);
                    id = NativeLLamaInteroperability.llama_sample_token_mirostat(Context, candidates_p, options.MirostatTAU, options.MirostatETA, mirostat_m, ref mirostatMU);
                }
                else if (options.Mirostat == Mirostat.Mirostat2)
                {
                    // Mirostat2
                    NativeLLamaInteroperability.llama_sample_temperature(Context, candidates_p, options.Temperature);
                    id = NativeLLamaInteroperability.llama_sample_token_mirostat_v2(Context, candidates_p, options.MirostatTAU, options.MirostatETA, ref mirostatMU);
                }
                else
                {
                    // Temperature
                    NativeLLamaInteroperability.llama_sample_top_k(Context, candidates_p, options.TopK, 1);
                    NativeLLamaInteroperability.llama_sample_tail_free(Context, candidates_p, options.TfsZ, 1);
                    NativeLLamaInteroperability.llama_sample_typical(Context, candidates_p, options.TypicalP, 1);
                    NativeLLamaInteroperability.llama_sample_top_p(Context, candidates_p, options.TopP, 1);
                    NativeLLamaInteroperability.llama_sample_temperature(Context, candidates_p, options.Temperature);
                    id = NativeLLamaInteroperability.llama_sample_token(Context, candidates_p);
                }

                tokens.Add(id);

                yield return NativeLLamaInteroperability.llama_token_to_bytes(Context, id);

                if (id == NativeLLamaInteroperability.llama_token_eos(Context))
                    break;
            }

            await Task.CompletedTask;
        }

        internal async IAsyncEnumerable<string> GenerateTokenStringAsync(NativeLlamaGenerateOptions options, LlamaCppSessionState state, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var bytesBuffer = new List<byte>();
            await foreach (var tokenBytes in GenerateTokenBytesAsync(options, state, cancellationToken))
            {
                bytesBuffer.AddRange(tokenBytes);
                if (bytesBuffer.ToArray().TryGetUtf8String(out var tokenString) && tokenString != null)
                {
                    yield return tokenString;
                    bytesBuffer.Clear();
                }
            }

            if (bytesBuffer.Any())
            {
                yield return Encoding.UTF8.GetString(bytesBuffer.ToArray());
            }
        }

        internal async IAsyncEnumerable<byte[]> GenerateTokenBytesAsync(NativeLlamaGenerateOptions options, LlamaCppSessionState state, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var mirostatMU = 2.0f * options.MirostatTAU;

            while (NativeLLamaInteroperability.llama_get_kv_cache_token_count(Context) < NativeLLamaInteroperability.llama_n_ctx(Context) && !cancellationToken.IsCancellationRequested)
            {
                for (var offset = state.EvalOffset; offset < state.TokenIds.Count && !cancellationToken.IsCancellationRequested; offset += _options.BatchSize)
                {
                    var evalCount = state.TokenIds.Count - offset;
                    if (evalCount > _options.BatchSize)
                        evalCount = _options.BatchSize;

                    NativeLLamaInteroperability.llama_eval(
                        Context,
                        state.TokenIds.Skip(offset).ToArray(),
                        evalCount,
                        state.EvalOffset,
                        options.ThreadCount
                    );

                    state.EvalOffset += evalCount;
                }

                //var logits = NativeLLamaInteroperability.llama_get_logits(Context);
                var n_vocab = NativeLLamaInteroperability.llama_n_vocab(Context);

                var candidates = new NativeLLamaInteroperability.llama_token_data[n_vocab];
                for (LlamaToken tokenId = 0; tokenId < n_vocab && !cancellationToken.IsCancellationRequested; tokenId++)
                    candidates[tokenId] = new NativeLLamaInteroperability.llama_token_data { id = tokenId, logit = NativeLLamaInteroperability.llama_get_logits(Context)[tokenId], p = 0.0f };

                if (cancellationToken.IsCancellationRequested)
                    break;

                var candidates_p = new NativeLLamaInteroperability.llama_token_data_array { data = candidates.ToArray(), size = (nuint)candidates.Length, sorted = false };

                // Apply penalties
                var newLineLogit = NativeLLamaInteroperability.llama_get_logits(Context)[NativeLLamaInteroperability.llama_token_nl(Context)];
                var lastRepeatCount = Math.Min(Math.Min(state.TokenIds.Count, options.LastTokenCountPenalty), NativeLLamaInteroperability.llama_n_ctx(Context));

                NativeLLamaInteroperability.llama_sample_repetition_penalty(
                    Context,
                    candidates_p,
                    state.TokenIds.Skip(state.TokenIds.Count - lastRepeatCount).Take(lastRepeatCount).ToArray(),
                    options.RepeatPenalty
                );

                NativeLLamaInteroperability.llama_sample_frequency_and_presence_penalties(
                    Context,
                    candidates_p,
                    state.TokenIds.Skip(state.TokenIds.Count - lastRepeatCount).Take(lastRepeatCount).ToArray(),
                    options.FrequencyPenalty,
                    options.PresencePenalty
                );

                if (!options.PenalizeNewLine)
                    NativeLLamaInteroperability.llama_get_logits(Context)[NativeLLamaInteroperability.llama_token_nl(Context)] = newLineLogit;

                var id = default(LlamaToken);

                // Sampling
                if (options.Temperature <= 0.0f)
                {
                    // Greedy
                    id = NativeLLamaInteroperability.llama_sample_token_greedy(Context, candidates_p);
                }
                else if (options.Mirostat == Mirostat.Mirostat)
                {
                    // Mirostat
                    var mirostat_m = 100;
                    NativeLLamaInteroperability.llama_sample_temperature(Context, candidates_p, options.Temperature);
                    id = NativeLLamaInteroperability.llama_sample_token_mirostat(Context, candidates_p, options.MirostatTAU, options.MirostatETA, mirostat_m, ref mirostatMU);
                }
                else if (options.Mirostat == Mirostat.Mirostat2)
                {
                    // Mirostat2
                    NativeLLamaInteroperability.llama_sample_temperature(Context, candidates_p, options.Temperature);
                    id = NativeLLamaInteroperability.llama_sample_token_mirostat_v2(Context, candidates_p, options.MirostatTAU, options.MirostatETA, ref mirostatMU);
                }
                else
                {
                    // Temperature
                    NativeLLamaInteroperability.llama_sample_top_k(Context, candidates_p, options.TopK, 1);
                    NativeLLamaInteroperability.llama_sample_tail_free(Context, candidates_p, options.TfsZ, 1);
                    NativeLLamaInteroperability.llama_sample_typical(Context, candidates_p, options.TypicalP, 1);
                    NativeLLamaInteroperability.llama_sample_top_p(Context, candidates_p, options.TopP, 1);
                    NativeLLamaInteroperability.llama_sample_temperature(Context, candidates_p, options.Temperature);
                    id = NativeLLamaInteroperability.llama_sample_token(Context, candidates_p);
                }

                state.TokenIds.Add(id);

                yield return NativeLLamaInteroperability.llama_token_to_bytes(Context, id);

                if (id == NativeLLamaInteroperability.llama_token_eos(Context))
                    break;
            }

            await Task.CompletedTask;
    }

        public List<LlamaToken> Tokenize(string text, bool addBos = false, bool addEos = false)
        {
            NativeLLamaInteroperability.llama_tokenize(Context, text, out var _tokens, addBos);
            var tokens = new List<LlamaToken>();
            for (var i = 0; i < _tokens.Length; i++) tokens.Add(_tokens[i]);
            if (addEos) tokens.Add(NativeLLamaInteroperability.llama_token_eos(Context));
            return tokens;
        }

        public void Dispose()
        {
            if (Context != IntPtr.Zero)
            {
                NativeLLamaInteroperability.llama_free(Context);
                Context = IntPtr.Zero;
            }

            if (Model != IntPtr.Zero)
            {
                NativeLLamaInteroperability.llama_free_model(Model);
                Model = IntPtr.Zero;
            }

            NativeLLamaInteroperability.llama_backend_free();
        }
    }
}
