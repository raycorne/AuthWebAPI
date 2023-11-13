using FurnitureRepo.Core.Models.FurnitureCategoryModels;

namespace FurnitureRepo.Core.Responses
{
    public class GetAllCategoriesResponse
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public List<FurnitureCategoryDTO>? FurnitureCategories { get; set; }
    }
}
