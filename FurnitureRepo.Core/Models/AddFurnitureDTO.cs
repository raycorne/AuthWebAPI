namespace FurnitureRepo.Core.Models
{
    public class AddFurnitureDTO
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public int Price { get; set; }
        public bool IsActive { get; set; } = false;
        public List<AddImageDTO>? Images { get; set; }
    }
}
