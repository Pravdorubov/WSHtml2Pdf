using WSHtml2Pdf.Middlewares;
using WSHtml2Pdf.Services.Implementations;
using WSHtml2Pdf.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IFileResolver, FileResolver>();
builder.Services.AddScoped<IFileUploader, FileUploader>();
builder.Services.AddScoped<IFileConverter, FileConverter>();
builder.Services.AddScoped<IFileSender, FileSender>();
builder.Services.AddScoped<IFileCleaner, FileCleaner>();
builder.Services.AddSingleton<IStreamFileUploadService, StreamFileUploadLocalService>();
builder.Services.AddSingleton<IState, State>();
//builder.Services.AddHostedService<WSBackgroundService>();

var app = builder.Build();

builder.Configuration.AddIniFile("config.ini");
app.UseFileServer();

app.UseMiddleware<Cleaner>();
app.UseMiddleware<ItemsSetter>();

app.MapPost("/upload", async (HttpContext context, IFileResolver fileResolver) =>
{
    await fileResolver.Upload(context);
});

app.MapGet("/setcookie", async (HttpContext context, IConfiguration configuration) => {
    var guid = Guid.NewGuid();
    context.Response.Cookies.Append(configuration["cookiename"], guid.ToString(), new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(1) });
    await context.Response.CompleteAsync(); 
 });

app.MapGet("/progress", async (HttpContext context, IState state, IConfiguration configuration) => 
{
    string checkname = configuration["checkitemname"];
    bool needProgress = (bool)context.Items[checkname];
    if (needProgress)
    {
        string itemHtmlFilename = configuration["htmlitemname"];
        string itemPdfFilename = configuration["pdfitemname"];
        string itemNeedClean = configuration["needcleanitemname"];

        string htmlFilename = (string)context.Items[itemHtmlFilename];
        string pdfFilename = (string)context.Items[itemPdfFilename];

        if (state.IsHtmlLoaded(htmlFilename))
        {
            if (state.IsReadyToSend(pdfFilename))
            {
                await context.Response.WriteAsync("Pdf is loading");
            }
            else
            {
                await context.Response.WriteAsync("Converting is going");
            }
        }
        else
        {
            await context.Response.WriteAsync("Html is loading");
        }
    }
    else
    {
        await context.Response.WriteAsync("");
    }
});

app.MapGet("/scanner", async (HttpContext context, IState state, IConfiguration configuration, IFileResolver fileResolver) =>
{
    string checkname = configuration["checkitemname"];
    bool needScan = (bool) context.Items[checkname];
    if (needScan)
    {
        string itemHtmlFilename = configuration["htmlitemname"];
        string itemPdfFilename = configuration["pdfitemname"];
        string itemNeedClean = configuration["needcleanitemname"];

        string htmlFilename = (string)context.Items[itemHtmlFilename];
        string pdfFilename = (string)context.Items[itemPdfFilename];

        if (state.IsHtmlLoaded(htmlFilename))
        {
            if (state.IsReadyToSend(pdfFilename))
            {
                if (!state.IsSending(pdfFilename))
                {
                   await fileResolver.Send(context);
                }
            }
            else
            {
                if (!state.IsConverting(htmlFilename))
                {
                    await fileResolver.Convert(context);
                }
            }
        }

    }
});

app.Run();
