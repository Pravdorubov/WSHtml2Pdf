using WSHtml2Pdf.Services.Interfaces;

namespace WSHtml2Pdf.Services.Implementations
{
    public class FileResolver : IFileResolver
    {
        private IFileUploader fileUploader;
        private IFileConverter fileConverter;
        private IFileSender fileSender;
        public IFileCleaner fileCleaner;

        public FileResolver(IFileUploader fileUploader, IFileConverter fileConverter, IFileSender fileSender, IFileCleaner fileCleaner)
        {
            this.fileUploader = fileUploader;
            this.fileConverter = fileConverter;
            this.fileSender = fileSender;
            this.fileCleaner = fileCleaner;
        }

        public async Task Convert(HttpContext context)
        {
            await fileConverter.Start(context);   
        }

        public async Task Send(HttpContext context)
        {
            bool isSend = await fileSender.Start(context);
            if (isSend)
            {
                fileCleaner.Clean(context);
            }
        }

        public async Task Upload(HttpContext context)
        {
            await fileUploader.Start(context);
        }
    }
}
