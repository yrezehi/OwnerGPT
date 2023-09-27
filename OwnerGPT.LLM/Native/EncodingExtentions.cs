using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace OwnerGPT.LLM.Native
{
    public static class EncodingExtentions
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
