namespace WSHtml2Pdf.Services.Interfaces
{
    public interface IFileResolver
    {
        Task Upload(HttpContext context);
        Task Convert(HttpContext context);
        Task Send(HttpContext context);

    }
}
