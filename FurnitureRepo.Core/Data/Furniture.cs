using System.ComponentModel.DataAnnotations;

namespace FurnitureRepo.Core.Data
{
	public class Furniture
	{
		public Guid Id { get; set; }
		[MaxLength(100)]
		public string Name { get; set; } = null!;
		public string? Description { get; set; }
		[MaxLength(50)]
		public string Type { get; set; } = null!;
		public int Cost { get; set; }
		public bool IsActive { get; set; } = false;
		public string? ImgUrl { get; set; }
	}
}
