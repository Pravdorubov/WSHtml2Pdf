namespace WSHtml2Pdf.Services.Interfaces
{
    public interface IFileSender
    {
        Task<bool> Start(HttpContext context);
    }
}
