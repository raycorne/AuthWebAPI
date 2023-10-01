using Microsoft.AspNetCore.Mvc;

namespace MobileAppWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadFileController : ControllerBase
    {
        private IWebHostEnvironment _webHostEnvironment;

        public UploadFileController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("AddFile")]
        public async Task<IActionResult> AddFile()
        {
            try
            {
                var httpContent = HttpContext.Request;

                if (httpContent == null)
                {
                    return BadRequest();
                }
                if (httpContent.Form.Files.Count > 0)
                {
                    foreach (var file in httpContent.Form.Files)
                    {
                        var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "ImagesUploadFolder");

                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }

                        using (var memoryStream = new MemoryStream())
                        {
                            await file.CopyToAsync(memoryStream);
                            await System.IO.File.WriteAllBytesAsync(Path.Combine(filePath, file.FileName), memoryStream.ToArray());
                        }
                    }
                    return Ok(httpContent.Form.Files.Count.ToString() + " files uploaded");
                }
                return BadRequest("No file selected");
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}
