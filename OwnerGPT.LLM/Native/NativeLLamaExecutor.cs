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

        public async IAsyncEnumerable<string> ExecuteAsync(NativeLLamaOptions options, LlamaToken[] tokenizedContext, [EnumeratorCancellation] CancellationToken cancellationToken = default)
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

        internal async IAsyncEnumerable<byte[]> InferAsync(NativeLLamaOptions options, LlamaToken[] tokenizedContext, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var mirostatMU = 2.0f * 5.0f;

            while (NativeLLamaInteroperability.llama_get_kv_cache_token_count(Context) < NativeLLamaInteroperability.llama_n_ctx(Context) && !cancellationToken.IsCancellationRequested)
            {
                int evaulateOffset = 0;
                int batchSize = 512;

                for (var offset = evaulateOffset; offset < tokenizedContext.Count() && !cancellationToken.IsCancellationRequested; offset += 512)
                {
                    var evalCount = tokenizedContext.Count() - offset;

                    if (evalCount > batchSize)
                        evalCount = batchSize;

                    NativeLLamaInteroperability.llama_eval(
                        Context,
                        tokenizedContext.Skip(offset).ToArray(),
                        evalCount,
                        evaulateOffset,
                        12
                    );

                    evaulateOffset += evalCount;
                }

                var n_vocab = NativeLLamaInteroperability.llama_n_vocab(Context);

                var candidates = new NativeLLamaInteroperability.llama_token_data[n_vocab];
                for (LlamaToken tokenId = 0; tokenId < n_vocab && !cancellationToken.IsCancellationRequested; tokenId++)
                    candidates[tokenId] = new NativeLLamaInteroperability.llama_token_data { id = tokenId, logit = NativeLLamaInteroperability.llama_get_logits(Context)[tokenId], p = 0.0f };

                if (cancellationToken.IsCancellationRequested)
                    break;

                var candidates_p = new NativeLLamaInteroperability.llama_token_data_array { data = candidates.ToArray(), size = (nuint)candidates.Length, sorted = false };

                // Apply penalties
                var newLineLogit = NativeLLamaInteroperability.llama_get_logits(Context)[NativeLLamaInteroperability.llama_token_nl(Context)];
                var lastRepeatCount = Math.Min(Math.Min(tokenizedContext.Count(), 64), NativeLLamaInteroperability.llama_n_ctx(Context));

                NativeLLamaInteroperability.llama_sample_repetition_penalty(
                    Context,
                    candidates_p,
                    tokenizedContext.Skip(tokenizedContext.Count() - lastRepeatCount).Take(lastRepeatCount).ToArray(),
                    1.1f
                );

                NativeLLamaInteroperability.llama_sample_frequency_and_presence_penalties(
                    Context,
                    candidates_p,
                    tokenizedContext.Skip(tokenizedContext.Count() - lastRepeatCount).Take(lastRepeatCount).ToArray(),
                    0.0f,
                    0.0f
                );

                if (!false)
                    NativeLLamaInteroperability.llama_get_logits(Context)[NativeLLamaInteroperability.llama_token_nl(Context)] = newLineLogit;

                var id = default(LlamaToken);

                if (true)
                {
                    // Mirostat
                    var mirostat_m = 100;
                    NativeLLamaInteroperability.llama_sample_temperature(Context, candidates_p, 0.5f);
                    id = NativeLLamaInteroperability.llama_sample_token_mirostat(Context, candidates_p, 5.0f, 0.1f, mirostat_m, ref mirostatMU);
                }

                tokenizedContext.Append(id);

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

            LlamaToken tokenized = NativeLLamaInteroperability.llama_tokenize_with_model(Context, context, rentedArray, contextCount, bos);

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
