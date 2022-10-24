using WSHtml2Pdf.Services.Interfaces;

namespace WSHtml2Pdf.Services.Implementations
{
    public class FileUploader : IFileUploader
    {
        private IConfiguration configuration;
        private IWebHostEnvironment environment;
        private IStreamFileUploadService streamFileUploadService;

        public HashSet<string> Upoading = new HashSet<string>();

        public FileUploader(IConfiguration configuration, IWebHostEnvironment environment, IStreamFileUploadService streamFileUploadService)
        {
            this.configuration = configuration;
            this.environment = environment;
            this.streamFileUploadService = streamFileUploadService;
        }

        public async Task Start(HttpContext context)
        {
            
            var items = context.Items;
            if (items.ContainsKey("htmlfilename"))
            {
                
                var filename = items["htmlfilename"].ToString();
                Upoading.Add(filename);
                var filePath = Path.Combine(environment.WebRootPath, configuration["htmlfilesdir"], filename);
                await streamFileUploadService.SaveFileToPhysicalFolder(context, filePath);
                Upoading.Remove(filename);

                //await using var writeStream = File.Create(filePath);
                //await request.BodyReader.CopyToAsync(writeStream);
            }
        }

        public bool IsUploading(string guid)
        {
            return Upoading.Contains(guid);
        }
    }
}
