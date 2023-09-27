using System.Runtime.CompilerServices;
using System.Text;
using LlamaModel = System.IntPtr;
using NativeLLamaContext = System.IntPtr;
using LlamaToken = System.Int32;
using static OwnerGPT.LLM.Native.NativeLLamaInteroperability;
using Microsoft.Extensions.Options;
using OwnerGPT.LLM.Configuration;
using System.Reflection;

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
            Context = NativeLLamaInteroperability.llama_new_context_with_model(Model, parameters);
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
            parameters.rope_freq_scale = 1.0f;
            parameters.low_vram = false;
            parameters.use_mmap = false;
            parameters.use_mlock = false;

            return parameters;
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

                //var logits = LlamaCppInterop.llama_get_logits(_context);
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
