namespace WSHtml2Pdf.Services.Interfaces
{
    public interface IFileUploader
    {
        bool IsUploading(string guid);
        Task Start(HttpContext context);
    }
}
