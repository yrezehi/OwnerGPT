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