using Microsoft.AspNetCore.Mvc;

namespace AuthWebAPI.Core.Models
{
    public class ErrorResponse
    {
        public static BadRequestObjectResult ReturnErrorResponse(string errorMessage)
        {
            return new BadRequestObjectResult(new AuthMainResponse
            {
                ErrorMessage = errorMessage,
                IsSuccess = true
            });
        }
    }
}
