﻿using FurnitureRepo.Core.Models;
using FurnitureRepo.Core.Models.FurnitureModels;
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
        private readonly IFurnitureImageRepository _furnitureImagesRepository;

        public FurnitureController(IFurnitureRepository furnitureRepository,
            IWebHostEnvironment webHostEnvironment,
            IFurnitureImageRepository furnitureImagesRepository)
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
                Guid furnitureId = Guid.NewGuid();

                var furnitureImages = GetUploadedImages(furnitureId, addFurnitureDTO.MainImagePath!);

                
                var furnitureDTO = new FurnitureDTO
                {
                    Id = furnitureId,
                    Name = addFurnitureDTO.Name,
                    Description = addFurnitureDTO.Description,
                    CategoryId = addFurnitureDTO.CategoryId,
                    Price = addFurnitureDTO.Price,
                    IsActive = addFurnitureDTO.IsActive,
                    Url = addFurnitureDTO.Url,
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

        private List<FurnitureImageDTO> GetUploadedImages(Guid furnitureId, string mainImagePath)
        {
            var images = new List<FurnitureImageDTO>();
            string mainImageName = "";

            if (string.IsNullOrEmpty(mainImagePath) == false)
                mainImageName = Path.GetFileName(mainImagePath);


            string tempPath = "Images/Unsorted";
            string newDirectoryPath = $"Images/{furnitureId}";
            if (!Directory.Exists(newDirectoryPath))
                Directory.CreateDirectory(newDirectoryPath);

            string[] files = Directory.GetFiles(tempPath);

            if (files.Length > 0)
            {
                foreach (var file in files)
                {
                    if (System.IO.File.Exists(file))
                    {
                        bool isMain = Path.GetFileName(file) == mainImageName;
                        string newFilePath = Path.Combine(newDirectoryPath, Path.GetFileName(file));
                        System.IO.File.Move(file, newFilePath);
                        images.Add(new FurnitureImageDTO
                        {
                            Id = Guid.NewGuid(),
                            Uri = newFilePath,
                            IsMainImage = isMain
                        });
                    }
                }
            }

            return images;
        }

        

        [HttpPut("UpdateFurniture")]
        public async Task<IActionResult> UpdateFurniture([FromBody] FurnitureDTO furnitureDTO)
        {
            try
            {
                var furnitureImages = GetUploadedImages(furnitureDTO.Id);
                if (furnitureImages.Count > 0)
                {
                    if (furnitureDTO.Images == null)
                        furnitureDTO.Images = new List<FurnitureImageDTO>();
                    furnitureDTO.Images.AddRange(furnitureImages);
                }

                var response = await _furnitureRepository.UpdateFurniture(furnitureDTO);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private List<FurnitureImageDTO> GetUploadedImages(Guid furnitureId)
        {
            var images = new List<FurnitureImageDTO>();


            string tempPath = "Images/Unsorted";
            string newDirectoryPath = $"Images/{furnitureId}";
            if (!Directory.Exists(newDirectoryPath))
                Directory.CreateDirectory(newDirectoryPath);

            string[] files = Directory.GetFiles(tempPath);
            if (files.Length > 0)
            {
                foreach (var file in files)
                {
                    if (System.IO.File.Exists(file))
                    {
                        string newFilePath = Path.Combine(newDirectoryPath, Path.GetFileName(file));
                        System.IO.File.Move(file, newFilePath);
                        images.Add(new FurnitureImageDTO
                        {
                            Id = Guid.NewGuid(),
                            Uri = newFilePath,
                        });
                    }
                }
            }

            return images;
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

        [HttpGet("GetFurnitureByUrl/{CategoryId}/{FurnitureUrl}")]
        public async Task<IActionResult> GetFurnitureByUrl(int CategoryId, string FurnitureUrl)
        {
            try
            {
                var response = await _furnitureRepository.GetFurnitureByUrl(CategoryId, FurnitureUrl);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetFurnituresInCategory/{CategoryId}")]
        public async Task<IActionResult> GetFurnituresInCategory(int CategoryId)
        {
            try
            {
                var response = await _furnitureRepository.GetFurnituresInCategory(CategoryId);
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
