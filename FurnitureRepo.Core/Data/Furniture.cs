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
		public FurnitureCategory Category { get; set; } = null!;
		public int CategoryId { get; set; }
		public int Price { get; set; }
		public bool IsActive { get; set; } = false;
		public List<FurnitureImage>? Images { get; set; }
	}
}