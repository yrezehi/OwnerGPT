namespace OwnerGPT.Plugins.Manager.Documents
{
    // TODO: limit temporary file size via configuration
    public class DocumentManager
    {
        private static string TEMPORARY_DIRECTORY_PATH = "C:\\OwnerGPT.Temporary.Upload";

        public DocumentManager()
        {
            CreateTemporaryDirectoryIfNotExists();
        }

        public void UploadDocument() { }

        private void CreateTemporaryDirectoryIfNotExists()
        {
            if (Directory.Exists(TEMPORARY_DIRECTORY_PATH))
                NukeTemporaryDirectory(TEMPORARY_DIRECTORY_PATH);

            if (!Directory.Exists(TEMPORARY_DIRECTORY_PATH))
                Directory.CreateDirectory(TEMPORARY_DIRECTORY_PATH);
        }

        private void NukeTemporaryDirectory(string directoryPath)
        {
            var directoryInformation = new DirectoryInfo(directoryPath);

            foreach (var file in directoryInformation.GetFiles())
            {
                file.Delete();
            }
        }
    }
}
