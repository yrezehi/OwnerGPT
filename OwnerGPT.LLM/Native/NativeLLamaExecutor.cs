using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LlamaModel = System.IntPtr;
using NativeLLamaContext = System.IntPtr;
using LlamaToken = System.Int32;
using System.Buffers;
using LLama.Common;
using static System.Net.Mime.MediaTypeNames;
using System.Threading;
using LLama.Native;
using static LLama.LLamaContext;
using System.Runtime.CompilerServices;
using System.Text.Unicode;

namespace OwnerGPT.LLM.Native
{
    public class NativeLLamaExecutor
    {
        private NativeLLamaContext Context;
        private NativeLLamaModel Model;

        public NativeLLamaExecutor(NativeLLamaContext context, NativeLLamaModel model)
        {
            Context = context;
            Model = model;
        }

        public async IAsyncEnumerable<string> Execute(string prompt, NativeLLamaOptions options, CancellationToken cancellationToken = default)
        {
            ExecuteAsync(options, Tokenize(prompt), cancellationToken);
            yield break;
        }

        internal async IAsyncEnumerable<string> ExecuteAsync(NativeLLamaOptions options, int[] tokenizedContext, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var bytesBuffer = new List<byte>();
            await foreach (var tokenBytes in InferAsync(options, tokenizedContext, cancellationToken))
            {
                bytesBuffer.AddRange(tokenBytes);

                yield return Encoding.UTF8.GetString(bytesBuffer.ToArray());
                bytesBuffer.Clear();
            }

            if (bytesBuffer.Any())
            {
                yield return Encoding.UTF8.GetString(bytesBuffer.ToArray());
            }
        }

        internal async IAsyncEnumerable<byte[]> InferAsync(NativeLLamaOptions options, int[] tokenizedContext, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var mirostatMU = 2.0f * options.MirostatTAU;

            while (NativeLLamaInteroperability.llama_get_kv_cache_token_count(Context) < NativeLLamaInteroperability.llama_n_ctx(Context) && !cancellationToken.IsCancellationRequested)
            {
                for (var offset = state.EvalOffset; offset < state.TokenIds.Count && !cancellationToken.IsCancellationRequested; offset += options.BatchSize)
                {
                    var evalCount = state.TokenIds.Count - offset;
                    if (evalCount > options.BatchSize)
                        evalCount = options.BatchSize;

                    NativeLLamaInteroperability.llama_eval(
                        Context,
                        state.TokenIds.Skip(offset).ToArray(),
                        evalCount,
                        state.EvalOffset,
                        options.ThreadCount
                    );

                    state.EvalOffset += evalCount;
                }

                //var logits = LlamaCppInterop.llama_get_logits(Context);
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

        public LlamaToken[] Tokenize(string context, bool bos = false)
        {
            var contextCount = this.GetBytesCount(context) + (bos ? 1 : 0);
            var rentedArray = ArrayPool<int>.Shared.Rent(contextCount);

            LlamaToken tokenized = NativeLLamaInteroperability.llama_tokenize_with_model(Model, context, rentedArray, contextCount, bos);

            var result = new int[tokenized];

            Array.ConstrainedCopy(rentedArray, 0, result, 0, tokenized);

            ArrayPool<int>.Shared.Return(rentedArray);

            return result;
        }

        private LlamaToken GetBytesCount(string context)
        {
            return Encoding.UTF8.GetByteCount(context);
        }
    }
}
