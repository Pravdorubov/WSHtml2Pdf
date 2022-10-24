using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using WSHtml2Pdf.Services.Interfaces;

namespace WSHtml2Pdf.Services.Implementations
{
    public class StreamFileUploadLocalService : IStreamFileUploadService
    {
        private async Task UploadFile(MultipartReader reader, MultipartSection? section, string filepath)
        {
            while (section != null)
            {
                var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(
                    section.ContentDisposition, out var contentDisposition
                );
                if (hasContentDispositionHeader)
                {
                    if (contentDisposition.DispositionType.Equals("form-data") &&
                    (!string.IsNullOrEmpty(contentDisposition.FileName.Value) ||
                    !string.IsNullOrEmpty(contentDisposition.FileNameStar.Value)))
                    {
                        string filePath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "UploadedFiles"));
                        byte[] fileArray;
                        using (var memoryStream = new MemoryStream())
                        {
                            await section.Body.CopyToAsync(memoryStream);
                            fileArray = memoryStream.ToArray();
                        }
                        using (var fileStream = File.Create(filepath))
                        {
                            await fileStream.WriteAsync(fileArray);
                        }
                    }
                }
                section = await reader.ReadNextSectionAsync();
            }
        }

        public async Task SaveFileToPhysicalFolder(HttpContext context, string filepath)
        {
            var request = context.Request;
            var boundary = HeaderUtilities.RemoveQuotes(MediaTypeHeaderValue.Parse(request.ContentType).Boundary).Value;
            var reader = new MultipartReader(boundary, request.Body);
            var section = await reader.ReadNextSectionAsync();
            await UploadFile(reader, section, filepath);
            
        }
    }
}
