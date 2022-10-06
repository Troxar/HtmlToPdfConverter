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

    async Task<IResult> UploadHandler(HttpRequest request)
    {
        var form = await request.ReadFormAsync();
        if (form.Files.Count == 0)
        {
            return await Task.FromResult(Results.BadRequest("File not received"));
        }
        
        var file = form.Files[0];
        if (file.Length == 0)
        {
            return await Task.FromResult(Results.BadRequest("File is empty"));
        }

        var originalFileName = Path.GetFileName(file.FileName);
        var savingFileName = Path.GetRandomFileName();
        var savingFilePath = Path.Combine(Directory.GetCurrentDirectory(), "upload", savingFileName);

        await using (var stream = File.Create(savingFilePath))
        {
            await file.CopyToAsync(stream);
        }
        
        return await Task.FromResult(Results.Ok($"{originalFileName} saved as {savingFileName}"));
    }
}