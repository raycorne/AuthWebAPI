using AuthWebAPI.Core.Data;
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

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}

	}
}
