﻿namespace OwnerGPT.LLM.Configuration
{
    public static class ModelConfiguration
    {
        /* Paths of the actual models binary file */
        public static string LLAMA_MODEL_PATH = "C:\\llm_models\\llama-2-7b-guanaco-qlora.Q5_K_S.gguf";

        /* Model tunning parameters */
        public static uint CONTEXT_SIZE = 1024;

        public static uint SEED_COUNT = 1337;

        public static int GPU_LAYER_COUNT = 12;
        /* Inference parameters */

        // Should range from 0.0 to 1.0, the higher the value more the model gets creative on ambiguous context
        public static float INFERENCE_TEMPERATURE = 0.6f;

        // Idk what would happen when exceeding the limit? 
        public static float CONTEXT_LIMIT = 4096;

        public static float TEMPERATURE = 0.75f;
        public static int MAX_TOKENS = 512;
    }
}
