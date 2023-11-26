using FurnitureRepo.Core.Data;
using FurnitureRepo.Core.Models;
using FurnitureRepo.Core.Models.FurnitureModels;

namespace MobileAppWebAPI
{
    public static class FurnitureMapper
    {
        public static Furniture ToFurniture(this FurnitureDTO dto) => new Furniture
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            CategoryId = dto.CategoryId,
            Price = dto.Price,
            IsActive = dto.IsActive,
            Url = dto.Url,
            Images = ToListFurnitureImage(dto.Images!)
        };

        public static List<FurnitureImage> ToListFurnitureImage(this List<FurnitureImageDTO> imagesDTO)
        {
            var images = new List<FurnitureImage>();
            if(imagesDTO != null && imagesDTO.Count > 0)
            {
                foreach (var image in imagesDTO)
                    images.Add(ToFurnitureImage(image));
            }
            
            return images;
        }

        public static FurnitureImage ToFurnitureImage(this FurnitureImageDTO imageDTO) => new FurnitureImage
        {
            Id = imageDTO.Id,
            Uri = imageDTO.Uri,
            IsMainImage = imageDTO.IsMainImage,
            FurnitureId = imageDTO.FurnitureId,
        };

        public static FurnitureDTO ToFurnitureDTO(this Furniture furniture) => new FurnitureDTO
        {
            Id = furniture.Id,
            Name = furniture.Name,
            Description = furniture.Description,
            CategoryId = furniture.CategoryId,
            Price = furniture.Price,
            IsActive = furniture.IsActive,
            Url = furniture.Url,
            Images = ToListFurnitureImageDTO(furniture.Images!)
        };

        public static List<FurnitureImageDTO> ToListFurnitureImageDTO(this List<FurnitureImage> images)
        {
            var imagesDTO = new List<FurnitureImageDTO>();
            if (images != null && images.Count > 0)
            {
                foreach (var image in images)
                    imagesDTO.Add(ToFurnitureImageDTO(image));
            }

            return imagesDTO;
        }

        public static FurnitureImageDTO ToFurnitureImageDTO(this FurnitureImage imageDTO) => new FurnitureImageDTO
        {
            Id = imageDTO.Id,
            Uri = imageDTO.Uri,
            IsMainImage = imageDTO.IsMainImage,
            FurnitureId = imageDTO.FurnitureId,
        };
    }
}
