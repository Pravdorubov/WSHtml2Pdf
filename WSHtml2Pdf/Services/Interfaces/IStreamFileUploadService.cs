using Microsoft.AspNetCore.WebUtilities;

namespace WSHtml2Pdf.Services.Interfaces
{
    public interface IStreamFileUploadService
    {
        Task SaveFileToPhysicalFolder(HttpContext context,  string filepath);
    }
}
