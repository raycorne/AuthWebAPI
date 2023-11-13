using FurnitureRepo.Core.Models.FurnitureModels;
using Microsoft.AspNetCore.Http;

namespace FurnitureRepo.Core.Requests
{
    public class FurnitureUploadRequest
    {
        public FurnitureDTO FurnitureDTO { get; set; } = null!;
        public IFormFile[]? Files { get; set; }
    }
}
