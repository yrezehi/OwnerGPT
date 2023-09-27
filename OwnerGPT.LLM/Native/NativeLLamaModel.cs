using System.Runtime.CompilerServices;
using System.Text;
using LlamaModel = System.IntPtr;
using NativeLLamaContext = System.IntPtr;
using LlamaToken = System.Int32;
using static OwnerGPT.LLM.Native.NativeLLamaInteroperability;
using Microsoft.Extensions.Options;
using OwnerGPT.LLM.Configuration;

namespace OwnerGPT.LLM.Native
{
    public class NativeLLamaModel : IDisposable
    {
        private LlamaModel _model;
        private NativeLLamaContext _context;
        public NativeLLamaContext Handle => _context;

        private NativeLLamaOptions _options = new();
        public NativeLLamaOptions Options { get => _options; }

        private byte[]? _initialState;
        internal byte[] GetInitialState() => _initialState ?? new byte[0];

        internal byte[] GetRawState() => NativeLLamaInteroperability.llama_copy_state_data(_context);
        internal void SetRawState(byte[] state) => NativeLLamaInteroperability.llama_set_state_data(_context, state);
        public NativeLLamaSession CreateSession() => new NativeLLamaSession(this);

        public string UntokenizeToText(IEnumerable<LlamaToken> tokenIds)
        {
            if (!tokenIds.Any())
                return String.Empty;

            var bytes = new List<byte[]>();
            foreach (var tokenId in tokenIds)
            {
                if (tokenId == NativeLLamaInteroperability.llama_token_bos(_context))
                    bytes.Add(Encoding.UTF8.GetBytes("<s>"));
                else if (tokenId == NativeLLamaInteroperability.llama_token_eos(_context))
                    bytes.Add(Encoding.UTF8.GetBytes("</s>"));
                else
                    bytes.Add(NativeLLamaInteroperability.llama_token_to_bytes(_context, tokenId));
            }

            return Encoding.UTF8.GetString(bytes.SelectMany(x => x).ToArray());
        }

        public void ResetState()
        {
            if (_initialState == null)
                return;

            SetRawState(_initialState);
        }

        public void Load(string modelPath, NativeLLamaOptions options, string? loraPath = null, string? loraBaseModelPath = null)
        {
            if (!File.Exists(modelPath))
                throw new FileNotFoundException($"Model file not found \"{modelPath}\".");

            if (_context != IntPtr.Zero)
                throw new InvalidOperationException($"Model already loaded.");

            var useLora = loraPath != null;

            if (useLora && !File.Exists(loraPath))
                throw new FileNotFoundException($"LoRA adapter file not found \"{loraPath}\".");

            var cparams = this.BuildDefaultParameters();

            _model = NativeLLamaInteroperability.llama_load_model_from_file(modelPath, cparams);
            _context = NativeLLamaInteroperability.llama_new_context_with_model(_model, cparams);
            _initialState = NativeLLamaInteroperability.llama_copy_state_data(_context);
            _options = options;

            if (useLora)
            {
                if (loraBaseModelPath == null)
                    loraBaseModelPath = modelPath;

                if (loraPath == null)
                    throw new ArgumentNullException(nameof(loraPath));

                if (loraBaseModelPath == null)
                    throw new ArgumentNullException(loraPath, nameof(loraBaseModelPath));

                var result = NativeLLamaInteroperability.llama_model_apply_lora_from_file(_model, loraPath, loraBaseModelPath, 4);
                if (result != 0)
                    throw new Exception($"Unable to load LoRA file (return code: {result}).");
            }
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

            while (NativeLLamaInteroperability.llama_get_kv_cache_token_count(_context) < NativeLLamaInteroperability.llama_n_ctx(_context) && !cancellationToken.IsCancellationRequested)
            {
                for (var offset = state.EvalOffset; offset < state.TokenIds.Count && !cancellationToken.IsCancellationRequested; offset += _options.BatchSize)
                {
                    var evalCount = state.TokenIds.Count - offset;
                    if (evalCount > _options.BatchSize)
                        evalCount = _options.BatchSize;

                    NativeLLamaInteroperability.llama_eval(
                        _context,
                        state.TokenIds.Skip(offset).ToArray(),
                        evalCount,
                        state.EvalOffset,
                        options.ThreadCount
                    );

                    state.EvalOffset += evalCount;
                }

                //var logits = LlamaCppInterop.llama_get_logits(_context);
                var n_vocab = NativeLLamaInteroperability.llama_n_vocab(_context);

                var candidates = new NativeLLamaInteroperability.llama_token_data[n_vocab];
                for (LlamaToken tokenId = 0; tokenId < n_vocab && !cancellationToken.IsCancellationRequested; tokenId++)
                    candidates[tokenId] = new NativeLLamaInteroperability.llama_token_data { id = tokenId, logit = NativeLLamaInteroperability.llama_get_logits(_context)[tokenId], p = 0.0f };

                if (cancellationToken.IsCancellationRequested)
                    break;

                var candidates_p = new NativeLLamaInteroperability.llama_token_data_array { data = candidates.ToArray(), size = (nuint)candidates.Length, sorted = false };

                // Apply penalties
                var newLineLogit = NativeLLamaInteroperability.llama_get_logits(_context)[NativeLLamaInteroperability.llama_token_nl(_context)];
                var lastRepeatCount = Math.Min(Math.Min(state.TokenIds.Count, options.LastTokenCountPenalty), NativeLLamaInteroperability.llama_n_ctx(_context));

                NativeLLamaInteroperability.llama_sample_repetition_penalty(
                    _context,
                    candidates_p,
                    state.TokenIds.Skip(state.TokenIds.Count - lastRepeatCount).Take(lastRepeatCount).ToArray(),
                    options.RepeatPenalty
                );

                NativeLLamaInteroperability.llama_sample_frequency_and_presence_penalties(
                    _context,
                    candidates_p,
                    state.TokenIds.Skip(state.TokenIds.Count - lastRepeatCount).Take(lastRepeatCount).ToArray(),
                    options.FrequencyPenalty,
                    options.PresencePenalty
                );

                if (!options.PenalizeNewLine)
                    NativeLLamaInteroperability.llama_get_logits(_context)[NativeLLamaInteroperability.llama_token_nl(_context)] = newLineLogit;

                var id = default(LlamaToken);

                // Sampling
                if (options.Temperature <= 0.0f)
                {
                    // Greedy
                    id = NativeLLamaInteroperability.llama_sample_token_greedy(_context, candidates_p);
                }
                else if (options.Mirostat == Mirostat.Mirostat)
                {
                    // Mirostat
                    var mirostat_m = 100;
                    NativeLLamaInteroperability.llama_sample_temperature(_context, candidates_p, options.Temperature);
                    id = NativeLLamaInteroperability.llama_sample_token_mirostat(_context, candidates_p, options.MirostatTAU, options.MirostatETA, mirostat_m, ref mirostatMU);
                }
                else if (options.Mirostat == Mirostat.Mirostat2)
                {
                    // Mirostat2
                    NativeLLamaInteroperability.llama_sample_temperature(_context, candidates_p, options.Temperature);
                    id = NativeLLamaInteroperability.llama_sample_token_mirostat_v2(_context, candidates_p, options.MirostatTAU, options.MirostatETA, ref mirostatMU);
                }
                else
                {
                    // Temperature
                    NativeLLamaInteroperability.llama_sample_top_k(_context, candidates_p, options.TopK, 1);
                    NativeLLamaInteroperability.llama_sample_tail_free(_context, candidates_p, options.TfsZ, 1);
                    NativeLLamaInteroperability.llama_sample_typical(_context, candidates_p, options.TypicalP, 1);
                    NativeLLamaInteroperability.llama_sample_top_p(_context, candidates_p, options.TopP, 1);
                    NativeLLamaInteroperability.llama_sample_temperature(_context, candidates_p, options.Temperature);
                    id = NativeLLamaInteroperability.llama_sample_token(_context, candidates_p);
                }

                state.TokenIds.Add(id);

                yield return NativeLLamaInteroperability.llama_token_to_bytes(_context, id);

                if (id == NativeLLamaInteroperability.llama_token_eos(_context))
                    break;
            }

            await Task.CompletedTask;
        }

        public List<LlamaToken> Tokenize(string text, bool addBos = false, bool addEos = false)
        {
            NativeLLamaInteroperability.llama_tokenize(_context, text, out var _tokens, addBos);
            var tokens = new List<LlamaToken>();
            for (var i = 0; i < _tokens.Length; i++) tokens.Add(_tokens[i]);
            if (addEos) tokens.Add(NativeLLamaInteroperability.llama_token_eos(_context));
            return tokens;
        }

        public void Dispose()
        {
            if (_context != IntPtr.Zero)
            {
                NativeLLamaInteroperability.llama_free(_context);
                _context = IntPtr.Zero;
            }

            if (_model != IntPtr.Zero)
            {
                NativeLLamaInteroperability.llama_free_model(_model);
                _model = IntPtr.Zero;
            }

            NativeLLamaInteroperability.llama_backend_free();
        }
    }
}
