using FurnitureRepo.Core.Data;
using FurnitureRepo.Core.Responses;
using MobileAppWebAPI.Context;

namespace MobileAppWebAPI.Services.Furnitures
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
