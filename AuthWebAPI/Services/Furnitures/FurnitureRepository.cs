using FurnitureRepo.Core.Data;
using FurnitureRepo.Core.Models;
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

		public async Task<MainResponse> AddFurniture(FurnitureDTO furnitureDTO)
		{
			var response = new MainResponse();
			try
			{
				await _context.AddAsync(new Furniture
				{
					Name = furnitureDTO.Name,
					Description = furnitureDTO.Description,
					Type = furnitureDTO.Type,
					Cost = furnitureDTO.Cost,
					IsActive = furnitureDTO.IsActive,
					ImgUrl = furnitureDTO.ImgUrl
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

		public async Task<MainResponse> UpdateFurniture(FurnitureDTO furniture)
		{
			var response = new MainResponse();
			try
			{
				var exictingFurniture = _context.Furnitures.Where(x => x.Id == furniture.Id).FirstOrDefault();
				if (exictingFurniture != null)
				{
					exictingFurniture.Name = furniture.Name;
					exictingFurniture.Description = furniture.Description;
					exictingFurniture.Type = furniture.Type;
					exictingFurniture.Cost = furniture.Cost;
					exictingFurniture.IsActive = furniture.IsActive;
					exictingFurniture.ImgUrl = furniture.ImgUrl;

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

		public async Task<MainResponse> DeleteFurniture(DeleteFurnitureDTO forniture)
		{
			var response = new MainResponse();
			try
			{
				var exictingFurniture = _context.Furnitures.Where(x => x.Id == forniture.Id).FirstOrDefault();
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

		public async Task<MainResponse> GetAllFurnitures()
		{
			var response = new MainResponse();
			try
			{
				response.Content = await _context.Furnitures.ToListAsync();
				response.IsSuccess = true;
			}
			catch (Exception ex)
			{
				response.ErrorMessage = ex.Message;
				response.IsSuccess = false;
			}
			return response;
		}

		public async Task<MainResponse> GetFurnitureById(Guid id)
		{
			var response = new MainResponse();
			try
			{
				response.Content = await _context.Furnitures.Where(f => f.Id == id).FirstOrDefaultAsync();
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
