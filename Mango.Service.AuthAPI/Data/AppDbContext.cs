using Mango.Service.AuthAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mango.Service.AuthAPI.Data
{
	public class AppDbContext(DbContextOptions<AppDbContext> options) 
        : IdentityDbContext<ApplicationUser>(options: options)
	{
		public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

		}
	}
}
