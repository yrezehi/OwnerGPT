using OwnerGPT.DocumentEmbedding.Encoder.BERTTokenizers.Extensions;

namespace OwnerGPT.DocumentEmbedding.Encoder.BERTTokenizers.Base
{
    public abstract class UncasedTokenizer : TokenizerBase
    {
        protected UncasedTokenizer(Stream vocabularyFile) : base(vocabularyFile)
        {
        }

        protected override IEnumerable<string> TokenizeSentence(string text)
        {
            return text.Split(new string[] { " ", "   ", "\r\n" }, StringSplitOptions.None)
                .SelectMany(o => o.SplitAndKeep(".,;:\\/?!#$%()=+-*\"'–_`<>&^@{}[]|~'".ToArray()))
                .Select(o => o.ToLower());
        }
    }
}
