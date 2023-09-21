using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnerGPT.Plugins.Manager.Documents.Models
{
    public class PluginDocument
    {
        public string Name { get; set; }
        public byte[] Bytes { get; set; }
        public string Extension { get; set; }

        public PluginDocument(IFormFile file) { 

            if(file == null || file.Length == 0)
                throw new Exception("File is not valid!");

            Name = file.Name;
            Bytes = file.GetBytes();
        }
    }
}
