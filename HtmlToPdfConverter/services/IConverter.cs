using HtmlToPdfConverter.Entities;

namespace HtmlToPdfConverter.Services;

public interface IConverter
{
    public Task ConvertAsync(FileToConvertInfo fileInfo);
}
