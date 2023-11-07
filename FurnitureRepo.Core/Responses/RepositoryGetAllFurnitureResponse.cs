using FurnitureRepo.Core.Data;
using FurnitureRepo.Core.Models;

namespace FurnitureRepo.Core.Responses
{
    public class RepositoryGetAllFurnitureResponse
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public List<FurnitureDTO>? Furnitures { get; set; }
    }
}
