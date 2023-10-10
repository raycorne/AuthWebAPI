using System.ComponentModel.DataAnnotations;

namespace FurnitureRepo.Core.Data
{
    public class FurnitureImage
    {
        [Key]
        public Guid Id { get; set; }
        public string Uri { get; set; } = null!;

        //Navigation Properties
        public Guid FurnitureId { get; set; }
        public Furniture Furniture { get; set; }
    }
}
