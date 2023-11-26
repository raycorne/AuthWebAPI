using FurnitureRepo.Core.Responses;

namespace MobileAppWebAPI.Services.FurnitureImages
{
    public interface IFurnitureImageRepository
    {
        public Task<RepositoryMainResponse> DeleteImage(Guid id);
    }
}
