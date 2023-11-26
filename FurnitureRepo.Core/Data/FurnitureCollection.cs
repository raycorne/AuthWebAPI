using System.ComponentModel.DataAnnotations;

namespace FurnitureRepo.Core.Data
{
    public class FurnitureCollection
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? ImageUri { get; set; }    }
}
