using WSHtml2Pdf.Services.Interfaces;

namespace WSHtml2Pdf.Services.Implementations
{
    public class FileSender : IFileSender
    {
        private IWebHostEnvironment environment;
        private IConfiguration configuration;
        private IState state;

        public FileSender(IWebHostEnvironment environment, IConfiguration configuration, IState state)
        {
            this.environment = environment;
            this.configuration = configuration;
            this.state = state;
        }

        public async Task<bool> Start(HttpContext context)
        {

            var items = context.Items;
            string checkname = configuration["checkitemname"];
            string pdffilename = configuration["pdfitemname"];
            string htmlfilename = configuration["htmlitemname"];
            if (items.ContainsKey(checkname) && (bool)items[checkname])
            {
                state.StartSending(pdffilename);
                var filename = items[pdffilename].ToString();
                var path = Path.Combine(environment.WebRootPath, configuration["pdffilesdir"], filename);
                context.Response.Cookies.Delete(configuration["cookiename"]);

                context.Response.Headers.ContentDisposition = "attachment";
                context.Response.Headers.ContentType = "application/pdf";
                state.StopSending(pdffilename);
                state.SetReadyToSend(filename, false);

                await context.Response.SendFileAsync(path);

                return true;
            }

            return false;
        }

        

    }
}
