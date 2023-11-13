using FurnitureRepo.Core.Models.FurnitureCategoryModels;

namespace FurnitureRepo.Core.Responses
{
    public class GetSingleCategoryResponse
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public FurnitureCategoryDTO? FurnitureCategory { get; set; }
    }
}
