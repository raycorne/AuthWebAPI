﻿using FurnitureRepo.Core.Data;

namespace FurnitureRepo.Core.Models.FurnitureModels
{
    public class FurnitureDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public int Price { get; set; }
        public bool IsActive { get; set; } = false;
        public string Url { get; set; } = null!;
        public List<FurnitureImageDTO>? Images { get; set; }
        public int FurnitureCollectionId { get; set; }
    }
}
