using AuthWebAPI.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

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
        public async Task<IActionResult> AddFile(IFormFile[] formFiles)
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

        [HttpPost("AddFileByPath")]
        public async Task<IActionResult> AddFileByPath([FromBody] string? path)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    return BadRequest();
                }

                if (!string.IsNullOrWhiteSpace(path))
                {
                    byte[] imgBytes = Convert.FromBase64String(path);
                    string fileName = $"TestFile.jpeg";
                    string image = await UploadFile(imgBytes, fileName);
                    return Ok("Image was added");
                    //userToBeCreated.UserAvatar = avatar;
                }
                else
                {
                    return BadRequest("No image selected");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<string> UploadFile(byte[] bytes, string fileName)
        {
            string uploadsFolder = Path.Combine("Images", fileName);
            Stream stream = new MemoryStream(bytes);
            using (var ms = new FileStream(uploadsFolder, FileMode.Create))
            {
                await stream.CopyToAsync(ms);
            }
            return uploadsFolder;

        }
    }
}
