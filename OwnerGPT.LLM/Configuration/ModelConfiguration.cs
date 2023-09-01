using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnerGPT.LLM.Configuration
{
    public static class ModelConfiguration
    {
        /* Paths of the actual models binary file */
        public static string LLAMA_MODEL_PATH = "";

        /* Model tunning parameters */
        public static int MODEL_CONTEXT_SIZE = 1024;
        public static int MODEL_SEED_COUNT = 1337;
        public static int MODEL_GPU_LAYER_COUNT = 5;

        /* Inference parameters */

        // should range from 0.0 to 1.0, the higher the value more the model gets creative on ambiguous context
        public static float INFERENCE_TEMPERATURE = 0.6f;
    }
}
