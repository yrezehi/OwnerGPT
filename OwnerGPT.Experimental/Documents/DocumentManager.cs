using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnerGPT.Experimental.Documents
{
    public class DocumentManager
    {
        private static string TEMPORARY_DIRECTORY_PATH = "C:\\OwnerGPT.Temporary.Upload";

        public DocumentManager() {
            CreateTemporaryDirectoryIfNotExists();
        }

        private void CreateTemporaryDirectoryIfNotExists()
        {
            if(Directory.Exists(TEMPORARY_DIRECTORY_PATH))
                NukeTemporaryDirectory(TEMPORARY_DIRECTORY_PATH);

            if (!Directory.Exists(TEMPORARY_DIRECTORY_PATH))
                Directory.CreateDirectory(TEMPORARY_DIRECTORY_PATH);
        }

        private void NukeTemporaryDirectory(string directoryPath)
        {
            var directoryInformation =  new DirectoryInfo(directoryPath);

            foreach (var file in directoryInformation.GetFiles())
            {
                file.Delete();
            }
        }
    }
}
