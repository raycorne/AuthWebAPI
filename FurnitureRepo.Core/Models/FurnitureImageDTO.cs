namespace FurnitureRepo.Core.Models
{
    public class FurnitureImageDTO
    {
        public Guid Id { get; set; }
        public string Uri { get; set; } = null!;
        public bool IsMainImage { get; set; } = false;

        public Guid FurnitureId { get; set; }
    }
}
