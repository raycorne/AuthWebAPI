using FurnitureRepo.Core.Models.FurnitureModels;

namespace FurnitureRepo.Core.Responses
{
    public class RepositoryGetAllFurnitureResponse
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public List<FurnitureDTO>? Furnitures { get; set; }
    }
}
