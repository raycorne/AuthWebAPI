using FurnitureRepo.Core.Models.FurnitureModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureRepo.Core.Responses
{
    public class RepositoryGetSingleFurnitureResponse
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public FurnitureDTO? Furniture { get; set; }
    }
}
