using FurnitureRepo.Core.Models.FurnitureModels;
using FurnitureRepo.Core.Responses;
using Microsoft.EntityFrameworkCore;
using MobileAppWebAPI.Context;

namespace MobileAppWebAPI.Services.Furnitures
{
    public class FurnitureRepository : IFurnitureRepository
    {
        private readonly MobileAppDBContext _context;

        public FurnitureRepository(
            MobileAppDBContext context)
        {
            _context = context;
        }

        public async Task<RepositoryMainResponse> AddFurniture(FurnitureDTO furnitureDTO)
        {
            var response = new RepositoryMainResponse();
            try
            {

                var furniture = FurnitureMapper.ToFurniture(furnitureDTO);

                await _context.Furnitures.AddAsync(furniture);

                await _context.SaveChangesAsync();

                response.IsSuccess = true;
                response.Content = "Furniture added";
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<RepositoryMainResponse> UpdateFurniture(FurnitureDTO furniture)
        {
            var response = new RepositoryMainResponse();
            try
            {
                var existingFurniture = _context.Furnitures.Where(x => x.Id == furniture.Id)
                    .Include(f => f.Images)
                    .FirstOrDefault();
                if (existingFurniture != null)
                {
                    existingFurniture.Name = furniture.Name;
                    existingFurniture.Description = furniture.Description;
                    existingFurniture.CategoryId = furniture.CategoryId;
                    existingFurniture.Price = furniture.Price;
                    existingFurniture.IsActive = furniture.IsActive;
                    existingFurniture.Url = furniture.Url;

                    var newFurnitureImages = FurnitureMapper.ToListFurnitureImage(furniture.Images!);


                    if(newFurnitureImages.Count > 0) 
                    {
                        foreach (var newImage in newFurnitureImages)
                        {
                            var existingImage = existingFurniture.Images.Where(i => i.Id == newImage.Id).FirstOrDefault();
                            if (existingImage == null)
                            {
                                newImage.FurnitureId = existingFurniture.Id;
                                await _context.FurnitureImages.AddAsync(newImage);
                            }
                        }
                        
                    }

                    _context.Furnitures.Update(existingFurniture);

                    await _context.SaveChangesAsync();

                    response.IsSuccess = true;
                    response.Content = "Record Updated";
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = "Furniture wasn't found";
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<RepositoryMainResponse> DeleteFurniture(DeleteFurnitureDTO furniture)
        {
            var response = new RepositoryMainResponse();
            try
            {
                var exictingFurniture = _context.Furnitures.Where(x => x.Id == furniture.Id).FirstOrDefault();
                if (exictingFurniture != null)
                {
                    if (Directory.Exists($"Images/{exictingFurniture.Id}"))
                        Directory.Delete($"Images/{exictingFurniture.Id}", true);

                    _context.Furnitures.Remove(exictingFurniture);
                    await _context.SaveChangesAsync();


                    response.IsSuccess = true;
                    response.Content = "Furniture was deleted";
                }
                else
                {
                    response.ErrorMessage = "This furniture ID wasn't founded";
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<GetAllFurnitureResponse> GetAllFurnitures()
        {
            var response = new GetAllFurnitureResponse();
            List<FurnitureDTO> furnituresDTO = new();
            try
            {
                var furnitures = await _context.Furnitures.
                    Include(furniture => furniture.Images).
                    ToListAsync();

                foreach (var furniture in furnitures)
                {
                    var furnitureDTO = FurnitureMapper.ToFurnitureDTO(furniture);
                    furnituresDTO.Add(furnitureDTO);
                    /*furnituresDTO.Add(new FurnitureDTO
                    {
                        Id = furniture.Id,
                        Name = furniture.Name,
                        Description = furniture.Description,
                        CategoryId = furniture.CategoryId,
                        Price = furniture.Price,
                        IsActive = furniture.IsActive,
                        Url = furniture.Url,
                        Images = furniture.Images,
                    });*/
                }

                response.Furnitures = furnituresDTO;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<GetSingleFurnitureResponse> GetFurnitureById(Guid id)
        {
            var response = new GetSingleFurnitureResponse();
            try
            {
                var furniture = await _context.Furnitures.
                    Where(f => f.Id == id).
                    Include(furniture => furniture.Images).
                    FirstOrDefaultAsync();

                if (furniture != null)
                {
                    /*var furnitureDTO = new FurnitureDTO
                    {
                        Id = furniture.Id,
                        Name = furniture.Name,
                        Description = furniture.Description,
                        CategoryId = furniture.CategoryId,
                        Price = furniture.Price,
                        IsActive = furniture.IsActive,
                        Url = furniture.Url,
                        Images = furniture.Images
                    };*/
                    var furnitureDTO = FurnitureMapper.ToFurnitureDTO(furniture);
                    response.Furniture = furnitureDTO;
                    response.IsSuccess = true;
                }
                else
                {
                    response.ErrorMessage = "Мебель не найдена";
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<GetSingleFurnitureResponse> GetFurnitureByUrl(int categoryId, string furnitureUrl)
        {
            var response = new GetSingleFurnitureResponse();
            try
            {
                var furniture = await _context.Furnitures.
                    Where(f => f.Url == furnitureUrl && f.CategoryId == categoryId).
                    Include(furniture => furniture.Images).
                    FirstOrDefaultAsync();

                if (furniture != null)
                {
                    /*var furnitureDTO = new FurnitureDTO
                    {
                        Id = furniture.Id,
                        Name = furniture.Name,
                        Description = furniture.Description,
                        CategoryId = furniture.CategoryId,
                        Price = furniture.Price,
                        IsActive = furniture.IsActive,
                        Url = furniture.Url,
                        Images = furniture.Images
                    };*/
                    var furnitureDTO = FurnitureMapper.ToFurnitureDTO(furniture);
                    response.Furniture = furnitureDTO;
                    response.IsSuccess = true;
                }
                else
                {
                    response.ErrorMessage = "Мебель не найдена";
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<GetAllFurnitureResponse> GetFurnituresInCategory(int categoryId)
        {
            var response = new GetAllFurnitureResponse();
            List<FurnitureDTO> furnituresDTO = new();
            try
            {
                var furnitures = await _context.Furnitures
                    .Where(f => f.CategoryId == categoryId)
                    .Include(furniture => furniture.Images)
                    .ToListAsync();

                foreach (var furniture in furnitures)
                {
                    /*furnituresDTO.Add(new FurnitureDTO
                    {
                        Id = furniture.Id,
                        Name = furniture.Name,
                        Description = furniture.Description,
                        CategoryId = furniture.CategoryId,
                        Price = furniture.Price,
                        IsActive = furniture.IsActive,
                        Url = furniture.Url,
                        Images = furniture.Images,
                    });*/
                    var furnitureDTO = FurnitureMapper.ToFurnitureDTO(furniture);
                    furnituresDTO.Add(furnitureDTO);
                }

                response.Furnitures = furnituresDTO;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        //public async Task<List<Furniture>> GetAll() =>
        //	await _db.Furnitures.ToListAsync();

        //public async Task<Furniture> GetByID(Guid Id) =>
        //	await _db.Furnitures.Where(f => f.Id == Id).FirstOrDefaultAsync();
    }
}
