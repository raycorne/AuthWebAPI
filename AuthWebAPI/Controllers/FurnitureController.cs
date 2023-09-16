using FurnitureRepo.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileAppWebAPI.Services.Furnitures;

namespace MobileAppWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FurnitureController : ControllerBase
	{
		private readonly IFurnitureRepository _furnitureRepository;

        public FurnitureController(IFurnitureRepository furnitureRepository)
        {
			_furnitureRepository = furnitureRepository;
        }

		[HttpPost("AddFurniture")]
		public async Task<IActionResult> AddFurniture([FromBody] FurnitureDTO furnitureDTO)
		{
			try
			{
				var response = await _furnitureRepository.AddFurniture(furnitureDTO);
				return Ok(response);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPut("UpdateFurniture")]
		public async Task<IActionResult> UpdateFurniture([FromBody] FurnitureDTO furnitureDTO)
		{
			try
			{
				var response = await _furnitureRepository.UpdateFurniture(furnitureDTO);
				return Ok(response);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpDelete("DeleteFurniture")]
		public async Task<IActionResult> DeleteFurniture([FromBody] DeleteFurnitureDTO deleteFurnitureDTO)
		{
			try
			{
				var response = await _furnitureRepository.DeleteFurniture(deleteFurnitureDTO);
				return Ok(response);
			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);	
			}
		}

		[HttpGet("GetAllFurnitures")]
		public async Task<IActionResult> GetAllFurnitures()
		{
			try
			{
				var response = await _furnitureRepository.GetAllFurnitures();
				return Ok(response);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("GetFurnitureById/{FurnitureId}")]
		public async Task<IActionResult> GetFurnitureById(Guid FurnitureId)
		{
			try
			{
				var response = await _furnitureRepository.GetFurnitureById(FurnitureId);
				return Ok(response);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
    }
}
