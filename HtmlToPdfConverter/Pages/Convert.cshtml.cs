using HtmlToPdfConverter.entities;
using HtmlToPdfConverter.services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.Net.Mime.MediaTypeNames;

namespace HtmlToPdfConverter.Pages
{
    [IgnoreAntiforgeryToken]
    public class ConvertModel : PageModel
    {
        readonly IConfiguration _config;
        readonly ILogger<ConvertModel> _logger;

        [BindProperty]
        public IFormFile? InputFile { get; set; }

        public ConvertModel(IConfiguration config, ILogger<ConvertModel> logger)
        {
            _config = config;
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

            var innerFileName = Path.GetRandomFileName();
            var uploadPath = Path.Combine(_config["Converting:UploadFolder"],
                Path.ChangeExtension(innerFileName, ".html"));
            var downloadPath = Path.Combine(_config["Converting:DownloadFolder"],
                Path.ChangeExtension(innerFileName, ".pdf"));

            var fileInfo = new FileToConvertInfo
            {
                OriginalFileName = InputFile.FileName,
                ConvertedFileName = Path.ChangeExtension(InputFile.FileName, "pdf"),
                UploadPath = Path.GetFullPath(uploadPath),
                DownloadPath = Path.GetFullPath(downloadPath)
            };

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
