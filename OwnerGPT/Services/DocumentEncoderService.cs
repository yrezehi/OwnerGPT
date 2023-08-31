using DocumentEncoder.Encoder;

namespace OwnerGPT.Services
{
    public class DocumentEncoderService
    {
        private readonly SentenceEncoder SentenceEncoder;

        public DocumentEncoderService(SentenceEncoder sentenceEncoder)
        {
            SentenceEncoder = sentenceEncoder;
        }

        public float[][] Encode(params string[] documents)
        {
            return SentenceEncoder.Encode(documents);
        }

        public float[] Encode(string document)
        {
            var encodedDocuments = SentenceEncoder.Encode(new string[] { document });

            if(encodedDocuments.Length != 0)
                return encodedDocuments[0];


            throw new Exception();
        }
    }
}
