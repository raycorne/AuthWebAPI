﻿using FurnitureRepo.Core.Data;
using FurnitureRepo.Core.Models;
using FurnitureRepo.Core.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Imaging;

namespace MobileAppWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadImageController : ControllerBase
    {

        private string _unsortedDirectory = "Images/Unsorted";

        [HttpPost("UploadImagesToServer")]
        public async Task<IActionResult> UploadImagesToServer(List<AddImageDTO> images)
        {
            var response = new RepositoryMainResponse();
            try
            {
                foreach (var image in images)
                {
                    if (!string.IsNullOrWhiteSpace(image.Bytes))
                    {
                        byte[] imgBytes = Convert.FromBase64String(image.Bytes);
                        string fileName = $"{image.FileName}";
                        if (!Directory.Exists(_unsortedDirectory))
                        {
                            Directory.CreateDirectory(_unsortedDirectory);
                        }
                        string uploadsFolder = Path.Combine(_unsortedDirectory, fileName);

                        var compressedImage = CompressImage(imgBytes);

                        Stream stream = new MemoryStream(compressedImage);
                        using (var ms = new FileStream(uploadsFolder, FileMode.Create))
                        {
                            await stream.CopyToAsync(ms);
                        }

                        /*Stream stream = new MemoryStream(imgBytes);
                        using (var ms = new FileStream(uploadsFolder, FileMode.Create))
                        {
                            await stream.CopyToAsync(ms);
                        }*/


                    }
                }

                if (images.Count > 0)
                {
                    response.IsSuccess = true;
                    response.Content = "Files uploaded";
                    return Ok(response);
                }
                else
                {
                    return BadRequest("No file selected");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private byte[] CompressImage(byte[] originalImageBytes)
        {
            using (MemoryStream originalStream = new MemoryStream(originalImageBytes))
            using (Image image = Image.FromStream(originalStream))
            using (MemoryStream compressedStream = new MemoryStream())
            {
                // Определите параметры сжатия
                EncoderParameters encoderParameters = new EncoderParameters(1);
                long compressionLevel = 50; // Уровень сжатия (0 - без сжатия, 100 - максимальное сжатие)
                encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, compressionLevel);

                // Получите кодек для JPEG-изображений
                ImageCodecInfo jpegCodecInfo = GetEncoderInfo("image/jpeg");

                // Сохраните сжатое изображение в MemoryStream
                image.Save(compressedStream, jpegCodecInfo, encoderParameters);


                return compressedStream.ToArray();
            }
        }

        private ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.MimeType == mimeType)
                {
                    return codec;
                }
            }
            return null;
        }

        [HttpGet("GetUnlinkedImages")]
        public async Task<IActionResult> GetUnlinkedImages()
        {
            try
            {
                string[] files = await Task.Run(() => Directory.GetFiles(_unsortedDirectory));
                return Ok(files);

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
