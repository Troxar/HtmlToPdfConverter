using HtmlToPdfConverter.entities;
using HtmlToPdfConverter.services;

namespace HtmlToPdfConverter;

public class Startup
{
    private readonly IConfiguration _config;

    public Startup(IConfiguration config)
    {
        _config = config;
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IConverter, PuppeteerSharpConverter>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        
        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseEndpoints(e => e.MapPost("/api/upload", UploadHandler));
    }

    async Task<IResult> UploadHandler(HttpRequest request, IConverter converter, ILogger<Startup> logger, CancellationToken token)
    {
        var form = await request.ReadFormAsync(token);
        if (form.Files.Count == 0)
        {
            return await Task.FromResult(Results.BadRequest("File not received"));
        }
        
        var file = form.Files[0];
        if (file.Length == 0)
        {
            return await Task.FromResult(Results.BadRequest("File is empty"));
        }

        var innerFileName = Path.GetRandomFileName();
        var fileInfo = new FileToConvertInfo(
            file.FileName,
            Path.ChangeExtension(file.FileName, "pdf"),
            Path.Combine(Directory.GetCurrentDirectory(), "upload", Path.ChangeExtension(innerFileName, ".html")),
            Path.Combine(Directory.GetCurrentDirectory(), "download", Path.ChangeExtension(innerFileName, ".pdf"))
        );
        
        logger.LogInformation($"File uploading: {fileInfo.OriginalFileName}");
        await using (var stream = File.Create(fileInfo.UploadPath))
        {
            await file.CopyToAsync(stream, token);
        }

        logger.LogInformation($"File convertation: {fileInfo.UploadPath}");
        await converter.ConvertAsync(fileInfo);
        
        logger.LogInformation($"File downloading: {fileInfo.DownloadPath}");
        return await Task.FromResult(Results.File(fileInfo.DownloadPath, null, fileInfo.ConvertedFileName));
    }
}