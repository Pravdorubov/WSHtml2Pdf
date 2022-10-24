using PuppeteerSharp;
using WSHtml2Pdf.Services.Interfaces;

namespace WSHtml2Pdf.Services.Implementations
{
    public class FileConverter : IFileConverter
    {
        private IConfiguration configuration;
        private IWebHostEnvironment environment;
        private IState state;

        public FileConverter(IConfiguration configuration, IWebHostEnvironment environment, IState state)
        {
            this.configuration = configuration;
            this.environment = environment;
            this.state = state;
        }

        public async Task Start(HttpContext context)
        {
            var items = context.Items;
            if (items.ContainsKey("htmlfilename") && items.ContainsKey("pdffilename"))
            {
                var filenameHtml = items["htmlfilename"].ToString();
                var filenamePdf = items["pdffilename"].ToString();
                string filenamePathHtml = $"{context.Request.Headers.Referer}{configuration["htmlfilesdir"]}/{filenameHtml}";
                string filenamePatPdf = Path.Combine(environment.WebRootPath, configuration["pdffilesdir"], filenamePdf);

                state.StartConverting(filenameHtml);

                var options = new LaunchOptions
                {
                    Headless = true
                };

                var browserFetcher = new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision).Result;

                using (var browser = await Puppeteer.LaunchAsync(options))
                using (var page = await browser.NewPageAsync())
                {
                    await page.GoToAsync(filenamePathHtml);

                    await page.PdfAsync(filenamePatPdf);

                }
                state.StopConverting(filenameHtml);
                state.SetReadyToSend(filenamePdf,true);
            }
        }

    }
}
