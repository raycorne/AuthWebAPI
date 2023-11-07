using FurnitureRepo.Core.Models;
using FurnitureRepo.Core.Responses;

namespace MobileAppWebAPI.Services.Furnitures
{
    public interface IFurnitureRepository
    {
        public Task<RepositoryMainResponse> AddFurniture(FurnitureDTO furnitureDTO);
        public Task<RepositoryMainResponse> UpdateFurniture(FurnitureDTO furnitureDTO);
        public Task<RepositoryMainResponse> DeleteFurniture(DeleteFurnitureDTO forniture);
        public Task<RepositoryGetAllFurnitureResponse> GetAllFurnitures();
        public Task<RepositoryGetSingleFurnitureResponse> GetFurnitureById(Guid id);
        public Task<RepositoryGetSingleFurnitureResponse> GetFurnitureByUrl(string url);

    }
}
