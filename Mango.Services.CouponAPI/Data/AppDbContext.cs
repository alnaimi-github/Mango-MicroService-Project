using Mango.Services.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Data
{
	public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
	{
		public DbSet<Coupon>? Coupons { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			//seed data 10 rows
			modelBuilder.Entity<Coupon>().HasData(
				new Coupon { CouponId = 1, CouponCode = "CODE1", DiscountAmount = 10 ,MinAmount = 10},
				new Coupon { CouponId = 2, CouponCode = "CODE2", DiscountAmount = 20, MinAmount =20 },
				new Coupon { CouponId = 3, CouponCode = "CODE3", DiscountAmount = 30, MinAmount =30 },
				new Coupon { CouponId = 4, CouponCode = "CODE4", DiscountAmount = 40 , MinAmount =40},
				new Coupon { CouponId = 5, CouponCode = "CODE5", DiscountAmount = 50, MinAmount =50},
				new Coupon { CouponId = 6, CouponCode = "CODE6", DiscountAmount = 60, MinAmount =60},
				new Coupon { CouponId = 7, CouponCode = "CODE7", DiscountAmount = 70, MinAmount =70 },
				new Coupon { CouponId = 8, CouponCode = "CODE8", DiscountAmount = 80, MinAmount =80 },
				new Coupon { CouponId = 9, CouponCode = "CODE9", DiscountAmount = 90, MinAmount =90 },
				new Coupon { CouponId = 10,CouponCode = "CODE10", DiscountAmount = 100, MinAmount =100 }
			);
		}
	}
}
