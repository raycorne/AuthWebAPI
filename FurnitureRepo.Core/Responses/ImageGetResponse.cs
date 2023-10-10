using FurnitureRepo.Core.Models;

namespace FurnitureRepo.Core.Responses
{
    public class ImageGetResponse
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public List<byte[]>? ImagesBytes { get; set; } = new();
    }
}
