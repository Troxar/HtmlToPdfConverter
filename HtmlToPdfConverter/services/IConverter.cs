using HtmlToPdfConverter.entities;

namespace HtmlToPdfConverter.services;

public interface IConverter
{
    public Task ConvertAsync(FileToConvertInfo fileInfo);
}
