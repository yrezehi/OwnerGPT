﻿using System.Collections.Generic;
using System.IO;

namespace OwnerGPT.DocumentEncoder.Encoder.BERTTokenizers.Helpers
{
    public class VocabularyReader
    {
        public static List<string> ReadFile(Stream vocabularyFile)
        {
            var result = new List<string>();

            using (var reader = new StreamReader(vocabularyFile))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        result.Add(line);
                    }
                }
            }

            return result;
        }
    }
}