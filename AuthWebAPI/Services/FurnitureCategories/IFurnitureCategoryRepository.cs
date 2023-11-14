using FurnitureRepo.Core.Models.FurnitureCategoryModels;
using FurnitureRepo.Core.Responses;

namespace MobileAppWebAPI.Services.FurnitureCategories
{
    public interface IFurnitureCategoryRepository
    {
        public Task<RepositoryMainResponse> AddCategory(FurnitureCategoryDTO categoryDTO);
        public Task<RepositoryMainResponse> DeleteCategory(DeleteFurnitureCategoryDTO deleteCategoryDTO);
        public Task<RepositoryMainResponse> UpdateCategry(FurnitureCategoryDTO categoryDTO);
        public Task<GetAllCategoriesResponse> GetAllCategories();
        public Task<GetSingleCategoryResponse> GetFurnitureById(int id);
        public Task<GetSingleCategoryResponse> GetFurnitureByUrl(string url);
    }
}
