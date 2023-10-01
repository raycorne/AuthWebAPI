using FurnitureRepo.Core.Data;
using FurnitureRepo.Core.Models;
using FurnitureRepo.Core.Requests;
using FurnitureRepo.Core.Responses;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MobileAppWebAPI.Services.Furnitures;

namespace MobileAppWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FurnitureController : ControllerBase
    {
        private readonly IFurnitureRepository _furnitureRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFurnitureImagesRepository _furnitureImagesRepository;

        public FurnitureController(IFurnitureRepository furnitureRepository,
            IWebHostEnvironment webHostEnvironment,
            IFurnitureImagesRepository furnitureImagesRepository)
        {
            _furnitureRepository = furnitureRepository;
            _webHostEnvironment = webHostEnvironment;
            _furnitureImagesRepository = furnitureImagesRepository;
        }

        [HttpPost("AddFurniture")]
        public async Task<IActionResult> AddFurniture([FromForm] FurnitureUploadRequest furnitureUploadRequest)
        {
            try
            {
                var uploadResponse = await UploadImagesToServer(furnitureUploadRequest.Files);
                //var uploadResponse = new UploadImagesResponse { IsSuccess = false, ErrorMessage = "Non active"};
                List<string> images = new List<string>();
                if(uploadResponse.IsSuccess ==  true)
                {
                    List<FurnitureImage> furnitureImages = new List<FurnitureImage>();
                    foreach (var path in uploadResponse.ImagesPaths!)
                    {
                        furnitureImages.Add(new FurnitureImage
                        {
                            Id = Guid.NewGuid(),
                            Uri = path
                        });
                    }
                    //var imageResponse = await _furnitureImagesRepository.UploadImages(furnitureImages);
                    furnitureUploadRequest.FurnitureDTO.Images = furnitureImages;
                }
                var response = await _furnitureRepository.AddFurniture(furnitureUploadRequest.FurnitureDTO);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<UploadImagesResponse> UploadImagesToServer(IFormFile[] files)
        {
            var response = new UploadImagesResponse();
            try
            {
                var httpContent = HttpContext.Request;

                if (httpContent == null)
                {
                    response.ErrorMessage = "Content = null";
                    response.IsSuccess = false;
                    return response;
                }
                if (httpContent.Form.Files.Count > 0)
                {
                    List<string> imagesPaths = new List<string>();
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
                        imagesPaths.Add(Path.Combine(filePath, file.FileName));
                    }
                    response.IsSuccess = true;
                    response.ImagesPaths = imagesPaths;
                    return response;
                }
                response.IsSuccess = false;
                response.ErrorMessage = "No file selected";
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        [HttpPut("UpdateFurniture")]
        public async Task<IActionResult> UpdateFurniture([FromBody] FurnitureDTO furnitureDTO)
        {
            try
            {
                var response = await _furnitureRepository.UpdateFurniture(furnitureDTO);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteFurniture")]
        public async Task<IActionResult> DeleteFurniture([FromBody] DeleteFurnitureDTO deleteFurnitureDTO)
        {
            try
            {
                var response = await _furnitureRepository.DeleteFurniture(deleteFurnitureDTO);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllFurnitures")]
        public async Task<IActionResult> GetAllFurnitures()
        {
            try
            {
                var response = await _furnitureRepository.GetAllFurnitures();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetFurnitureById/{FurnitureId}")]
        public async Task<IActionResult> GetFurnitureById(Guid FurnitureId)
        {
            try
            {
                var response = await _furnitureRepository.GetFurnitureById(FurnitureId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
