using OwnerGPT.DocumentEmbedding.Encoder.BERTTokenizers.Base;

namespace OwnerGPT.DocumentEmbedding.Encoder;
public class MiniLMTokenizer : UncasedTokenizer
{
    public MiniLMTokenizer() : base(ResourceLoader.OpenResource(typeof(MiniLMTokenizer).Assembly, "vocab.txt"))
    {
    }
}
