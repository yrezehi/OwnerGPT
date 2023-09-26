using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LlamaModel = System.IntPtr;
using NativeLLamaContext = System.IntPtr;
using LlamaToken = System.Int32;

namespace OwnerGPT.LLM.Native
{
    public class NativeLLamaExecutor
    {
        private NativeLLamaContext Context;

        public List<LlamaToken> Tokenize(string context, bool addBos = false)
        {
            var contextCount = this.GetBytesCount(context);

            NativeLLamaInteroperability.llama_tokenize(Context, text, out var _tokens, addBos);
            var tokens = new List<LlamaToken>();
            for (var i = 0; i < _tokens.Length; i++) tokens.Add(_tokens[i]);
            return tokens;
        }

        private Int32 GetBytesCount(string context)
        {
            return Encoding.UTF8.GetByteCount(context);
        }
    }
}
