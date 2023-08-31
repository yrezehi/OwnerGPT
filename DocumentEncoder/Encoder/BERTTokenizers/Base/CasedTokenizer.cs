using DocumentEncoder.Encoder.BERTTokenizers.Extensions;

namespace DocumentEncoder.Encoder.BERTTokenizers.Base
{
    public abstract class CasedTokenizer : TokenizerBase
    {
        protected CasedTokenizer(Stream vocabularyFile) : base(vocabularyFile) { }

        protected override IEnumerable<string> TokenizeSentence(string text)
        {
            return text.Split(new string[] { " ", "   ", "\r\n" }, StringSplitOptions.None)
                .SelectMany(o => o.SplitAndKeep(".,;:\\/?!#$%()=+-*\"'–_`<>&^@{}[]|~'".ToArray()));
        }
    }
}
