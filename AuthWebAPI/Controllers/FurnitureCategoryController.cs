using FurnitureRepo.Core.Models.FurnitureCategoryModels;
using Microsoft.AspNetCore.Mvc;
using MobileAppWebAPI.Services.FurnitureCategories;

namespace MobileAppWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FurnitureCategoryController : ControllerBase
    {
        private readonly IFurnitureCategoryRepository _categoryRepository;

        public FurnitureCategoryController(
            IFurnitureCategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromBody] FurnitureCategoryDTO categoryDTO)
        {
            try
            {
                var response = await _categoryRepository.AddCategory(categoryDTO);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteCategory")]
        public async Task<IActionResult> DeleteCatecory(DeleteFurnitureCategoryDTO deleteCategoryDTO)
        {
            try
            {
                var response = await _categoryRepository.DeleteCategory(deleteCategoryDTO);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(FurnitureCategoryDTO categoryDTO)
        {
            try
            {
                var response = await _categoryRepository.UpdateCategry(categoryDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var response = await _categoryRepository.GetAllCategories();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetCategoryById/{Id}")]
        public async Task<IActionResult> GetCategoryById(int Id)
        {
            try
            {
                var response = await _categoryRepository.GetFurnitureById(Id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
