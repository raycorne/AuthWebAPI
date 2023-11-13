using FurnitureRepo.Core.Data;
using FurnitureRepo.Core.Models.FurnitureCategoryModels;
using FurnitureRepo.Core.Responses;
using Microsoft.EntityFrameworkCore;
using MobileAppWebAPI.Context;

namespace MobileAppWebAPI.Services.FurnitureCategories
{
    public class FurnitureCategoryRepository : IFurnitureCategoryRepository
    {
        private MobileAppDBContext _context;

        public FurnitureCategoryRepository(
            MobileAppDBContext context)
        {
            _context = context;
        }

        public async Task<RepositoryMainResponse> AddCategory(FurnitureCategoryDTO categoryDTO)
        {
            var response = new RepositoryMainResponse();
            try
            {
                await _context.FurnitureCategories.AddAsync(
                    new FurnitureCategory
                    {
                        Id = categoryDTO.Id,
                        Name = categoryDTO.Name,
                        Image = categoryDTO.Image,
                        IsDisplayedInNavbar = categoryDTO.IsDisplayedInNavbar,
                        Url = categoryDTO.Url,
                        Furnitures = categoryDTO.Furnitures
                    });

                await _context.SaveChangesAsync();

                response.IsSuccess = true;
                response.Content = "Category added";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        public async Task<RepositoryMainResponse> DeleteCategory(DeleteFurnitureCategoryDTO deleteCategoryDTO)
        {
            var response = new RepositoryMainResponse();
            try
            {
                var existingCategory = await _context.FurnitureCategories.Where((f) => f.Id == deleteCategoryDTO.Id)
                    .FirstOrDefaultAsync();

                if(existingCategory != null)
                {
                    _context.FurnitureCategories.Remove(existingCategory);
                    await _context.SaveChangesAsync();

                    response.IsSuccess = true;
                    response.Content = "Category deleted";
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = "Such category doesn't exists";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        public async Task<RepositoryMainResponse> UpdateCategry(FurnitureCategoryDTO categoryDTO)
        {
            var response = new RepositoryMainResponse();
            try
            {
                var existingCategory = await _context.FurnitureCategories
                    .Where((f) => f.Id == categoryDTO.Id).FirstOrDefaultAsync();

                if(existingCategory != null)
                {
                    existingCategory.Name = categoryDTO.Name;
                    existingCategory.Image = categoryDTO.Image;
                    existingCategory.IsDisplayedInNavbar = categoryDTO.IsDisplayedInNavbar;
                    existingCategory.Url = categoryDTO.Url;
                    existingCategory.Furnitures = categoryDTO.Furnitures;

                    await _context.SaveChangesAsync();

                    response.IsSuccess = true;
                    response.Content = "Category deleted";
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = "Such category doesn't exists";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        public async Task<GetAllCategoriesResponse> GetAllCategories()
        {
            var response = new GetAllCategoriesResponse();
            try
            {
                List<FurnitureCategoryDTO> categoryDTOs = new();
                var categories = await _context.FurnitureCategories.ToListAsync();
                foreach (var category in categories)
                {
                    categoryDTOs.Add(new FurnitureCategoryDTO
                    {
                        Id = category.Id,
                        Name = category.Name,
                        Image = category.Image,
                        IsDisplayedInNavbar = category.IsDisplayedInNavbar,
                        Url = category.Url,
                        Furnitures = category.Furnitures
                    });
                }

                response.FurnitureCategories = categoryDTOs;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        public async Task<GetSingleCategoryResponse> GetFurnitureById(int id)
        {
            var response = new GetSingleCategoryResponse();
            try
            {
                var category = await _context.FurnitureCategories
                    .Where(c => c.Id == id).
                    FirstOrDefaultAsync();

                if (category != null)
                {
                    var categoryDTO = new FurnitureCategoryDTO
                    {
                        Id = category.Id,
                        Name = category.Name,
                        Image = category.Image,
                        IsDisplayedInNavbar = category.IsDisplayedInNavbar,
                        Url = category.Url
                    };
                    
                    response.FurnitureCategory = categoryDTO;
                    response.IsSuccess = true;
                }
                else
                {
                    response.ErrorMessage = "Мебель не найдена";
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
