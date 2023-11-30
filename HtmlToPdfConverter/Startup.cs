using HtmlToPdfConverter.Configuration;
using HtmlToPdfConverter.Services;
using System.Net;

namespace HtmlToPdfConverter;

public class Startup
{
    readonly IConfiguration _config;

    public Startup(IConfiguration config)
    {
        _config = config;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRazorPages();
        services.AddTransient<IConverter, PuppeteerSharpConverter>();
        services.AddScoped<FileToConvertInfoFactory>();
        services.Configure<ConvertingOptions>(_config.GetSection("Converting"));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler(builder => builder.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync("Something went wrong");
            }));
        }

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
        });
    }
}
