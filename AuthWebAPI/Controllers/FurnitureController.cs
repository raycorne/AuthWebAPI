using FurnitureRepo.Core.Data;
using FurnitureRepo.Core.Models;
using FurnitureRepo.Core.Responses;
using Microsoft.AspNetCore.Mvc;
using MobileAppWebAPI.Services.FurnitureImages;
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
        public async Task<IActionResult> AddFurniture([FromBody] AddFurnitureDTO addFurnitureDTO)
        {
            try
            {
                var uploadResponse = await UploadImagesToServer(addFurnitureDTO.Images!);
                //var uploadResponse = new UploadImagesResponse { IsSuccess = false, ErrorMessage = "Non active"};
                List<FurnitureImage> furnitureImages = new List<FurnitureImage>();
                if (uploadResponse.IsSuccess == true)
                {
                    foreach (var path in uploadResponse.ImagesPaths!)
                    {
                        furnitureImages.Add(new FurnitureImage
                        {
                            Id = Guid.NewGuid(),
                            Uri = path
                        });
                    }
                }

                var furnitureDTO = new FurnitureDTO
                {
                    Name = addFurnitureDTO.Name,
                    Description = addFurnitureDTO.Description,
                    CategoryId = addFurnitureDTO.CategoryId,
                    Price = addFurnitureDTO.Price,
                    IsActive = addFurnitureDTO.IsActive,
                    Images = furnitureImages
                };

                var response = await _furnitureRepository.AddFurniture(furnitureDTO);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<UploadImagesResponse> UploadImagesToServer(List<AddImageDTO> images)
        {
            var response = new UploadImagesResponse();
            try
            {
                List<string> imagesPaths = new();

                foreach (var image in images)
                {
                    if (!string.IsNullOrWhiteSpace(image.Bytes))
                    {
                        byte[] imgBytes = Convert.FromBase64String(image.Bytes);
                        string fileName = $"{image.FileName}";
                        if (!Directory.Exists("Images"))
                        {
                            Directory.CreateDirectory("Images");
                        }

                        string uploadsFolder = Path.Combine("Images", fileName);
                        Stream stream = new MemoryStream(imgBytes);
                        using (var ms = new FileStream(uploadsFolder, FileMode.Create))
                        {
                            await stream.CopyToAsync(ms);
                        }
                        imagesPaths.Add(uploadsFolder);
                    }
                }

                if (imagesPaths.Count > 0)
                {
                    response.IsSuccess = true;
                    response.ImagesPaths = imagesPaths;
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = "No file selected";
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
            }
            return response;
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

        [HttpGet("GetImagesByUrl")]
        public async Task<IActionResult> GetImagesByUrl(List<string> Uris)
        {
            try
            {
                //var returnResponse = await _furnitureImagesRepository.GetImagesByUrl(Uri);
                ImageGetResponse response = new();
                foreach (var uri in Uris)
                {
                    
                    if (System.IO.File.Exists(uri))
                    {
                        byte[] bytes = System.IO.File.ReadAllBytes(uri);   // You can use your own method over here.         
                        response.ImagesBytes!.Add(bytes);
                    }
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
