using FurnitureRepo.Core.Responses;
using Microsoft.EntityFrameworkCore;
using MobileAppWebAPI.Context;

namespace MobileAppWebAPI.Services.FurnitureImages
{
    public class FurnitureImagesRepository : IFurnitureImagesRepository
    {
        private readonly MobileAppDBContext _context;

        public FurnitureImagesRepository(MobileAppDBContext context)
        {
            _context = context;
        }
    }
}
