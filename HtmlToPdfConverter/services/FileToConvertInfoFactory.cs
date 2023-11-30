using HtmlToPdfConverter.Configuration;
using HtmlToPdfConverter.Entities;
using Microsoft.Extensions.Options;

namespace HtmlToPdfConverter.Services
{
    public class FileToConvertInfoFactory
    {
        readonly IOptions<ConvertingOptions> _options;

        public FileToConvertInfoFactory(IOptions<ConvertingOptions> options)
        {
            _options = options;
        }

        public FileToConvertInfo Create(string fileName)
        {
            return new FileToConvertInfo(fileName, _options);
        }
    }
}
