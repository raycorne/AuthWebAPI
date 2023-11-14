using FurnitureRepo.Core.Models.FurnitureModels;
using FurnitureRepo.Core.Responses;

namespace MobileAppWebAPI.Services.Furnitures
{
    public interface IFurnitureRepository
    {
        public Task<RepositoryMainResponse> AddFurniture(FurnitureDTO furnitureDTO);
        public Task<RepositoryMainResponse> UpdateFurniture(FurnitureDTO furnitureDTO);
        public Task<RepositoryMainResponse> DeleteFurniture(DeleteFurnitureDTO furniture);
        public Task<GetAllFurnitureResponse> GetAllFurnitures();
        public Task<GetSingleFurnitureResponse> GetFurnitureById(Guid id);
        public Task<GetSingleFurnitureResponse> GetFurnitureByUrl(int categoryId, string url);
        public Task<GetAllFurnitureResponse> GetFurnituresInCategory(int categoryId);

    }
}
