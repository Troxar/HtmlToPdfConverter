using HtmlToPdfConverter.Entities;
using PuppeteerSharp;

namespace HtmlToPdfConverter.Services;

public class PuppeteerSharpConverter : IConverter
{
    public async Task ConvertAsync(FileToConvertInfo fileInfo)
    {
        var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync();
        await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true
        });
        await using var page = await browser.NewPageAsync();
        await page.GoToAsync(fileInfo.UploadPath);
        await page.PdfAsync(fileInfo.DownloadPath);
    }
}
