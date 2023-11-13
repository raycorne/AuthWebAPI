using FurnitureRepo.Core.Models.FurnitureModels;
using FurnitureRepo.Core.Responses;

namespace MobileAppWebAPI.Services.Furnitures
{
    public interface IFurnitureRepository
    {
        public Task<RepositoryMainResponse> AddFurniture(FurnitureDTO furnitureDTO);
        public Task<RepositoryMainResponse> UpdateFurniture(FurnitureDTO furnitureDTO);
        public Task<RepositoryMainResponse> DeleteFurniture(DeleteFurnitureDTO furniture);
        public Task<RepositoryGetAllFurnitureResponse> GetAllFurnitures();
        public Task<RepositoryGetSingleFurnitureResponse> GetFurnitureById(Guid id);
        public Task<RepositoryGetSingleFurnitureResponse> GetFurnitureByUrl(string url);

    }
}
