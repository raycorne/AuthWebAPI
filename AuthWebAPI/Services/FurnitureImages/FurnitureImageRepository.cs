using FurnitureRepo.Core.Responses;
using Microsoft.EntityFrameworkCore;
using MobileAppWebAPI.Context;

namespace MobileAppWebAPI.Services.FurnitureImages
{
    public class FurnitureImageRepository : IFurnitureImageRepository
    {
        private readonly MobileAppDBContext _context;

        public FurnitureImageRepository(MobileAppDBContext context)
        {
            _context = context;
        }

        public async Task<RepositoryMainResponse> DeleteImage(Guid id)
        {
            var response = new RepositoryMainResponse();
            try
            {
                var existingImage = await _context.FurnitureImages.Where(i => i.Id == id).FirstOrDefaultAsync();
                if (existingImage != null)
                {
                    _context.FurnitureImages.Remove(existingImage);
                    await _context.SaveChangesAsync();

                    if (System.IO.File.Exists($"{existingImage.Uri}"))
                        System.IO.File.Delete($"{existingImage.Uri}");

                    response.Content = "Image was deleted";
                    response.IsSuccess = true;  
                }
                else
                {
                    response.ErrorMessage = "Image wasn't founded";
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }
    }
}
