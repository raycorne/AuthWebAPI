using FurnitureRepo.Core.Data;
using FurnitureRepo.Core.Models.FurnitureCategoryModels;
using FurnitureRepo.Core.Models.FurnitureModels;
using FurnitureRepo.Core.Responses;
using Microsoft.EntityFrameworkCore;
using MobileAppWebAPI.Context;

namespace MobileAppWebAPI.Services.Furnitures
{
    public class FurnitureRepository : IFurnitureRepository
	{
		private readonly MobileAppDBContext _context;

		public FurnitureRepository(MobileAppDBContext context)
		{
			_context = context;
		}

		public async Task<RepositoryMainResponse> AddFurniture(FurnitureDTO furnitureDTO)
		{
			var response = new RepositoryMainResponse();
			try
			{
				await _context.Furnitures.AddAsync(new Furniture
				{
					Name = furnitureDTO.Name,
					Description = furnitureDTO.Description,
					CategoryId = furnitureDTO.CategoryId,
					Price = furnitureDTO.Price,
					IsActive = furnitureDTO.IsActive,
					Url = furnitureDTO.Url,
					Images = furnitureDTO.Images
				});

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
				var exictingFurniture = _context.Furnitures.Where(x => x.Id == furniture.Id).FirstOrDefault();
				if (exictingFurniture != null)
				{
					exictingFurniture.Name = furniture.Name;
					exictingFurniture.Description = furniture.Description;
					exictingFurniture.CategoryId = furniture.CategoryId;
					exictingFurniture.Price = furniture.Price;
					exictingFurniture.IsActive = furniture.IsActive;
					exictingFurniture.Url = furniture.Url;
					exictingFurniture.Images = furniture.Images;

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

		public async Task<RepositoryGetAllFurnitureResponse> GetAllFurnitures()
		{
			var response = new RepositoryGetAllFurnitureResponse();
			List<FurnitureDTO> furnituresDTO = new();
			try
			{
				var furnitures = await _context.Furnitures.
					Include(furniture => furniture.Images).
					ToListAsync();

				foreach(var furniture in furnitures)
				{
					furnituresDTO.Add(new FurnitureDTO
					{
						Id = furniture.Id,
						Name = furniture.Name,
						Description = furniture.Description,
						CategoryId = furniture.CategoryId,
						Price = furniture.Price,
						IsActive = furniture.IsActive,
						Url = furniture.Url,
						Images = furniture.Images,
					});
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

		public async Task<RepositoryGetSingleFurnitureResponse> GetFurnitureById(Guid id)
		{
			var response = new RepositoryGetSingleFurnitureResponse();
			try
			{
				var furniture = await _context.Furnitures.
					Where(f => f.Id == id).
                    Include(furniture => furniture.Images).
                    FirstOrDefaultAsync();

                if (furniture != null)
				{
					var furnitureDTO = new FurnitureDTO
					{
						Id = furniture.Id,
						Name = furniture.Name,
						Description = furniture.Description,
						CategoryId = furniture.CategoryId,
						Price = furniture.Price,
						IsActive = furniture.IsActive,
						Url = furniture.Url,
						Images = furniture.Images
					};
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

        public async Task<RepositoryGetSingleFurnitureResponse> GetFurnitureByUrl(string url)
        {
            var response = new RepositoryGetSingleFurnitureResponse();
            try
            {
				var furniture = await _context.Furnitures.
					Where(f => f.Url == url).
					Include(furniture => furniture.Images).
					FirstOrDefaultAsync();

                if (furniture != null)
                {
                    var furnitureDTO = new FurnitureDTO
                    {
                        Id = furniture.Id,
                        Name = furniture.Name,
                        Description = furniture.Description,
                        CategoryId = furniture.CategoryId,
                        Price = furniture.Price,
                        IsActive = furniture.IsActive,
                        Url = furniture.Url,
                        Images = furniture.Images
                    };
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

		

        //public async Task<List<Furniture>> GetAll() =>
        //	await _db.Furnitures.ToListAsync();

        //public async Task<Furniture> GetByID(Guid Id) =>
        //	await _db.Furnitures.Where(f => f.Id == Id).FirstOrDefaultAsync();
    }
}
