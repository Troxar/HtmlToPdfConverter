using HtmlToPdfConverter.Configuration;
using Microsoft.Extensions.Options;

namespace HtmlToPdfConverter.Entities;

public class FileToConvertInfo
{
    public string OriginalFileName { get; set; } = string.Empty;
    public string ConvertedFileName { get; set; } = string.Empty;
    public string UploadPath { get; set; } = string.Empty;
    public string DownloadPath { get; set; } = string.Empty;

    public FileToConvertInfo(string fileName, IOptions<ConvertingOptions> options)
    {
        var innerFileName = Path.GetRandomFileName();
        var pdfExtension = ".pdf";
        var htmlExtension = ".html";

        var uploadPath = Path.Combine(options.Value.UploadFolder, Path.ChangeExtension(innerFileName, htmlExtension));
        var downloadPath = Path.Combine(options.Value.DownloadFolder, Path.ChangeExtension(innerFileName, pdfExtension));

        OriginalFileName = fileName;
        ConvertedFileName = Path.ChangeExtension(fileName, pdfExtension);
        UploadPath = Path.GetFullPath(uploadPath);
        DownloadPath = Path.GetFullPath(downloadPath);
    }
}
