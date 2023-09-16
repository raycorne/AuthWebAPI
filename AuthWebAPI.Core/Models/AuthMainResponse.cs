namespace AuthWebAPI.Core.Models
{
    public class AuthMainResponse
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public object? Content { get; set; }
    }
}
