namespace OwnerGPT.LLM.Configuration
{
    public static class ModelConfiguration
    {
        /* Paths of the actual models binary file */
        public static string LLAMA_MODEL_PATH = "C:\\llm_models\\llama-2-7b-guanaco-qlora.Q2_K.gguf";

        /* Model tunning parameters */
        public static int MODEL_CONTEXT_SIZE = 1024;
        public static int MODEL_SEED_COUNT = 1337;
        public static int MODEL_GPU_LAYER_COUNT = 5;

        /* Inference parameters */

        // Should range from 0.0 to 1.0, the higher the value more the model gets creative on ambiguous context
        public static float INFERENCE_TEMPERATURE = 0.6f;

        // Idk what would happen when exceeding the limit? 
        public static float LLAMA_CONTEXT_LIMIT = 4096;
    }
}
