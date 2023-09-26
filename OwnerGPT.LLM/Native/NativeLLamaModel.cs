﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OwnerGPT.LLM.Native
{
    using LlamaModel = System.IntPtr;
    using LlamaContext = System.IntPtr;
    using LlamaToken = System.Int32;

    public class NativeLLamaModel : IDisposable
    {
        private LlamaModel _model;
        private LlamaContext _context;
        private NativeLLamaOptions _options = new();
        private byte[]? _initialState;

        public NativeLLamaModel()
        { }

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

        public LlamaContext Handle => _context;

        public NativeLLamaOptions Options { get => _options; }

        public List<LlamaToken> Tokenize(string text, bool addBos = false, bool addEos = false)
        {
            NativeLLamaInteroperability.llama_tokenize(_context, text, out var _tokens, addBos);
            var tokens = new List<LlamaToken>();
            for (var i = 0; i < _tokens.Length; i++) tokens.Add(_tokens[i]);
            if (addEos) tokens.Add(NativeLLamaInteroperability.llama_token_eos(_context));
            return tokens;
        }

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

        internal byte[] GetInitialState() => _initialState ?? new byte[0];
        internal byte[] GetRawState() => NativeLLamaInteroperability.llama_copy_state_data(_context);
        internal void SetRawState(byte[] state) => NativeLLamaInteroperability.llama_set_state_data(_context, state);

        public void ResetState()
        {
            if (_initialState == null)
                return;

            SetRawState(_initialState);
        }

        /// <summary>
        /// Load a model and optionally load a LoRA adapter
        /// </summary>
        /// <param name="modelPath">Specify the path to the LLaMA model file</param>
        /// <param name="options">Model options</param>
        /// <param name="loraPath">Apply a LoRA (Low-Rank Adaptation) adapter to the model (will override use_mmap=false). This allows you to adapt the pretrained model to specific tasks or domains.</param>
        /// <param name="loraBaseModelPath">Optional model to use as a base for the layers modified by the LoRA adapter. This flag is used in conjunction with the lora model path, and specifies the base model for the adaptation.</param>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public void Load(string modelPath, NativeLLamaOptions options, string? loraPath = null, string? loraBaseModelPath = null)
        {
            if (!File.Exists(modelPath))
                throw new FileNotFoundException($"Model file not found \"{modelPath}\".");

            if (_context != IntPtr.Zero)
                throw new InvalidOperationException($"Model already loaded.");

            var useLora = loraPath != null;

            if (useLora && !File.Exists(loraPath))
                throw new FileNotFoundException($"LoRA adapter file not found \"{loraPath}\".");

            var cparams = NativeLLamaInteroperability.llama_context_default_params();
            cparams.seed = options.Seed;
            cparams.n_ctx = options.ContextSize;
            cparams.n_gpu_layers = options.GpuLayers;
            cparams.rope_freq_base = options.RopeFrequencyBase;
            cparams.rope_freq_scale = options.RopeFrequencyScale;
            cparams.low_vram = options.LowVRAM;
            cparams.use_mmap = useLora ? false : options.UseMemoryMapping;
            cparams.use_mlock = options.UseMemoryLocking;

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

        public NativeLLamaSession CreateSession() => new NativeLLamaSession(this);

        internal async IAsyncEnumerable<string> GenerateTokenStringAsync(LlamaCppGenerateOptions options, LlamaCppSessionState state, [EnumeratorCancellation] CancellationToken cancellationToken = default)
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

        internal async IAsyncEnumerable<byte[]> GenerateTokenBytesAsync(LlamaCppGenerateOptions options, LlamaCppSessionState state, [EnumeratorCancellation] CancellationToken cancellationToken = default)
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
    }
    file static class Extensions
    {
        private static Encoding? _utf8;

        public static bool TryGetUtf8String(this byte[] bytes, out string? str)
        {
            if (_utf8 == null)
            {
                _utf8 = (Encoding)Encoding.UTF8.Clone();
                _utf8.DecoderFallback = new DecoderExceptionFallback();
            }

            try
            {
                _utf8.DecoderFallback = new DecoderExceptionFallback();
                str = _utf8.GetString(bytes);
                return true;
            }
            catch (DecoderFallbackException)
            {
                str = null;
                return false;
            }
        }
    }
}
