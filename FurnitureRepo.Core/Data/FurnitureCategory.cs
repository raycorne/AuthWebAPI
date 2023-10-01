using System.ComponentModel.DataAnnotations;

namespace FurnitureRepo.Core.Data
{
    public class FurnitureCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Image { get; set; }
        public List<Furniture> Furnitures { get; set; }
    }
}
