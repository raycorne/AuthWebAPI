﻿namespace FurnitureRepo.Core.Data
{
    public class FurnitureCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Image { get; set; }
        public bool IsDisplayedInNavbar { get; set; } = false;
        public string Url { get; set; } = null!;
        public List<Furniture>? Furnitures { get; set; }
    }
}
