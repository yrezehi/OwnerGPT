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
            Name = file.Name;
        }
    }
}
