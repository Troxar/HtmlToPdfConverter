using HtmlToPdfConverter.entities;

namespace HtmlToPdfConverter.services;

internal interface IConverter
{
    public Task ConvertAsync(FileToConvertInfo fileInfo);
}