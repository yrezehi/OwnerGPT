namespace OwnerGPT.LLM.Configuration
{
    public static class ModelConfiguration
    {
        /* Paths of the actual models binary file */
        public static string LLAMA_MODEL_PATH = "C:\\llm_models\\llama-2-7b-guanaco-qlora.Q5_K_S.gguf";

        /* Model tunning parameters */
        public static int CONTEXT_SIZE = 1024;
        public static int UNCHECKED_CONTEXT_SIZE = 16384;

        public static int SEED_COUNT = 1337;
        public static uint UNCHECKED_SEED_COUNT = 1337;

        public static int GPU_LAYER_COUNT = 5;
        public static int UNCHECKED_GPU_LAYER_COUNT = 42;
        /* Inference parameters */

        // Should range from 0.0 to 1.0, the higher the value more the model gets creative on ambiguous context
        public static float INFERENCE_TEMPERATURE = 0.6f;

        // Idk what would happen when exceeding the limit? 
        public static float CONTEXT_LIMIT = 4096;
    }
}
