namespace FurnitureRepo.Core.Responses
{
    public class RepositoryMainResponse
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public object? Content { get; set; }
    }
}
