using WSHtml2Pdf.Services.Interfaces;

namespace WSHtml2Pdf.Services.Implementations
{
    public class FileCleaner : IFileCleaner
    {
        private IWebHostEnvironment environment;
        private IConfiguration configuration;

        public FileCleaner(IWebHostEnvironment environment, IConfiguration configuration)
        {
            this.environment = environment;
            this.configuration = configuration;
        }

        public void Clean(HttpContext context)
        {
            string pdfFilenameItem = configuration["pdfitemname"];
            string htmFfilenameItem = configuration["htmlitemname"];

            string htmlFilename = context.Items[htmFfilenameItem].ToString();
            string pdfFilename = context.Items[pdfFilenameItem].ToString();

            DeleteFile(htmlFilename, configuration["htmlfilesdir"]);
            DeleteFile(pdfFilename, configuration["pdffilesdir"]);

        }

        private void DeleteFile(string filename, string path)
        {
            var filePath = Path.Combine(environment.WebRootPath, path, filename);
            FileInfo fileInfo = new(filePath);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
        }
    }
}
