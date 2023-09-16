using FurnitureRepo.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace MobileAppWebAPI.Services.Furnitures
{
	public interface IFurnitureRepository
	{
		public Task<MainResponse> AddFurniture(FurnitureDTO furnitureDTO);
		public Task<MainResponse> UpdateFurniture([FromBody] FurnitureDTO furnitureDTO);
		public Task<MainResponse> DeleteFurniture(DeleteFurnitureDTO forniture);
		public Task<MainResponse> GetAllFurnitures();
		public Task<MainResponse> GetFurnitureById(Guid id);
	}
}
