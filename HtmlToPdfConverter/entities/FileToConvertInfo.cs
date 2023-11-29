namespace HtmlToPdfConverter.entities;

public class FileToConvertInfo
{
    public string OriginalFileName { get; set; } = string.Empty;
    public string ConvertedFileName { get; set; } = string.Empty;
    public string UploadPath { get; set; } = string.Empty;
    public string DownloadPath { get; set; } = string.Empty;
}
