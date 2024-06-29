using Mango.Services.ShoppingCartAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartAPI.Data
{
	public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
	{
		public DbSet<CartHeader>? CartHeaders { get; set; }
		public DbSet<CartDetails>? CartDetails { get; set; }
	}
	
}
