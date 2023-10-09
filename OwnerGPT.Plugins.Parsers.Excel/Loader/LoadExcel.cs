using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnerGPT.Plugins.Parsers.Excel.Loader
{
    public static class LoadExcel
    {
        public static List<dynamic?> Load(Stream stream) =>
            MiniExcel.Query(stream).ToList();
    }
}
