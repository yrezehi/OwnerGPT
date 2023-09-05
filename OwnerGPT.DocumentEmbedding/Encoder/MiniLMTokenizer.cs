using OwnerGPT.DocumentEncoder.Encoder.BERTTokenizers.Base;

namespace OwnerGPT.DocumentEncoder.Encoder;
public class MiniLMTokenizer : UncasedTokenizer
{
    public MiniLMTokenizer() : base(ResourceLoader.OpenResource(typeof(MiniLMTokenizer).Assembly, "vocab.txt"))
    {
    }
}
