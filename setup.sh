#!/bin/bash

echo "                                                                     "
echo "                 ██████╗ ██╗    ██╗███╗   ██╗███████╗██████╗         "
echo "                ██╔═══██╗██║    ██║████╗  ██║██╔════╝██╔══██╗        "
echo "                ██║   ██║██║ █╗ ██║██╔██╗ ██║█████╗  ██████╔╝        "
echo "                ██║   ██║██║███╗██║██║╚██╗██║██╔══╝  ██╔══██╗        "
echo "                ╚██████╔╝╚███╔███╔╝██║ ╚████║███████╗██║  ██║        "
echo "                 ╚═════╝  ╚══╝╚══╝ ╚═╝  ╚═══╝╚══════╝╚═╝  ╚═╝        "
echo "                          (OwnerGPT Setup Script)                    "
echo "                                                                     "

if [[ ! "$OSTYPE" == "msys" ]]; then
	echo "script only works on windows at the moment!"
fi

LLAMA_MODEL_URL="https://huggingface.co/TheBloke/llama-2-7B-Guanaco-QLoRA-GGUF/resolve/main/llama-2-7b-guanaco-qlora.Q5_K_S.gguf"
LLAMA_MODEL_FILE_NAME="llama-7b.gguf"

echo -e "\n\nchecking model link...\n"

if curl --output /dev/null --silent --head --fail "$LLAMA_MODEL_URL"; then
  echo "URL of the model is working fine!"
else
  echo "URL does not exist or there something not right!"
fi

echo -e "\n\nattempting to download the model to ./$LLAMA_MODEL_FILE_NAME"

curl -L "$LLAMA_MODEL_URL" --progress-bar > $LLAMA_MODEL_FILE_NAME