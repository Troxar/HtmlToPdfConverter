using HtmlToPdfConverter.Configuration;
using HtmlToPdfConverter.Entities;
using HtmlToPdfConverter.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using static System.Net.Mime.MediaTypeNames;

namespace HtmlToPdfConverter.Pages
{
    [IgnoreAntiforgeryToken]
    public class ConvertModel : PageModel
    {
        readonly IOptions<ConvertingOptions> _options;
        readonly ILogger<ConvertModel> _logger;

        [BindProperty]
        public IFormFile? InputFile { get; set; }

        public ConvertModel(IOptions<ConvertingOptions> options, ILogger<ConvertModel> logger)
        {
            _options = options;
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync([FromServices] IConverter converter)
        {
            if (InputFile is null)
            {
                return BadRequest("File not received");
            }

            if (InputFile.Length == 0)
            {
                return BadRequest("File is empty");
            }

            var fileInfo = new FileToConvertInfo(InputFile.FileName, _options);

            _logger.LogInformation(string.Format("File uploading: {0}", fileInfo.OriginalFileName));
            using (var fileStream = new FileStream(fileInfo.UploadPath, FileMode.Create))
            {
                await InputFile.CopyToAsync(fileStream);
            }

            _logger.LogInformation(string.Format("File converting: {0}", fileInfo.UploadPath));
            await converter.ConvertAsync(fileInfo);

            return PhysicalFile(fileInfo.DownloadPath, Application.Pdf, fileInfo.ConvertedFileName);
        }
    }
}
