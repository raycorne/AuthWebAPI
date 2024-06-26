﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AuthWebAPI.Core.Data
{
    public class User : IdentityUser
    {
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;
        [MaxLength(50)]
        public string LastName { get; set; } = null!;
        public string? Address { get; set; }
        public string? RefreshToken { get; set; }
    }
}
