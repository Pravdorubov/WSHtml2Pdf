using System;

namespace WSHtml2Pdf.Middlewares
{
    public class Cleaner
    {
        private RequestDelegate next;
        private IConfiguration configuration;
        private IWebHostEnvironment environment;

        public Cleaner(RequestDelegate next, IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.next = next;
            this.configuration = configuration;
            this.environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await next.Invoke(context);

            string itemNeedClean = configuration["needcleanitemname"];

            if (context.Items.ContainsKey(itemNeedClean) && (bool) context.Items[itemNeedClean])
            {
                string htmlfilename = configuration["htmlitemname"];
                string pdffilename = configuration["pdfitemname"];
                var items = context.Items;

                DeleteFile(items[htmlfilename].ToString(), configuration["htmlfilesdir"]);
                DeleteFile(items[pdffilename].ToString(), configuration["pdffilesdir"]);
            }

            
        }

        public void DeleteFile(string filename, string path)
        {
            var filePath = Path.Combine(environment.WebRootPath, path, filename);
            FileInfo fileInfo = new(filePath);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
        }

    }
}
