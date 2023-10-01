using AuthWebAPI.Core.Data;
using FurnitureRepo.Core.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MobileAppWebAPI.Context
{
	public partial class MobileAppDBContext : IdentityDbContext<User>
	{
		public MobileAppDBContext(DbContextOptions<MobileAppDBContext> options)
			: base(options)
		{
		}

		public DbSet<Furniture> Furnitures { get; set; }
		public DbSet<FurnitureCategory> FurnitureCategories { get; set; }
		public DbSet<FurnitureImage> FurnitureImages { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}

	}
}
