using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LlamaModel = System.IntPtr;
using NativeLLamaContext = System.IntPtr;
using LlamaToken = System.Int32;
using System.Buffers;

namespace OwnerGPT.LLM.Native
{
    public class NativeLLamaExecutor
    {
        private NativeLLamaContext Context;
        private LlamaModel Model;

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
