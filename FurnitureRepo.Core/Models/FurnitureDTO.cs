using System.ComponentModel.DataAnnotations;

namespace FurnitureRepo.Core.Models
{
	public class FurnitureDTO
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = null!;
		public string? Description { get; set; }
		public string Type { get; set; } = null!;
		public int Cost { get; set; }
		public bool IsActive { get; set; } = false;
		public string? ImgUrl { get; set; }
	}
}
