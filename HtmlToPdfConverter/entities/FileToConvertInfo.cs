namespace HtmlToPdfConverter.entities;

internal record FileToConvertInfo(string OriginalFileName,
    string ConvertedFileName,
    string UploadPath,
    string DownloadPath);