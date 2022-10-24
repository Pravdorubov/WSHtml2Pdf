namespace WSHtml2Pdf.Middlewares
{
    public class ItemsSetter
    {
        private RequestDelegate next;
        private IConfiguration configuration;

        public ItemsSetter(RequestDelegate next, IConfiguration configuration)
        {
            this.next = next;
            this.configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path == "/upload" || context.Request.Path == "/scanner" || context.Request.Path == "/progress")
            {
                var cookies = context.Request.Cookies;
                string cookiename = configuration["cookiename"];
                string checkname = configuration["checkitemname"];
                string htmlfilename = configuration["htmlitemname"];
                string pdffilename = configuration["pdfitemname"];


                if (cookies.ContainsKey(cookiename))
                {
                    cookies.TryGetValue(cookiename, out var guid);

                    context.Items.Add(checkname, true);
                    context.Items.Add(htmlfilename, $"{guid}.html");
                    context.Items.Add(pdffilename, $"{guid}.pdf");
                }
                else
                {
                    context.Items.Add(checkname, false);
                }
            }
            await next.Invoke(context);
        }
    }
}
