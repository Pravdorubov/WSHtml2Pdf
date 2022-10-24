namespace WSHtml2Pdf.Services.Interfaces
{
    public interface IFileConverter
    {
        Task Start(HttpContext context);
    }
}
