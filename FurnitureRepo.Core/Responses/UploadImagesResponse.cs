namespace FurnitureRepo.Core.Responses
{
    public class UploadImagesResponse
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public List<string>? ImagesPaths { get; set; }
    }
}
