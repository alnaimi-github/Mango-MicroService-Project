using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mango.Services.CouponAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedCouponData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    CouponId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CouponCode = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    DiscountAmount = table.Column<double>(type: "float", nullable: false),
                    MinAmount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.CouponId);
                });

            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "CouponId", "CouponCode", "DiscountAmount", "MinAmount" },
                values: new object[,]
                {
                    { 1, "CODE1", 10.0, 10 },
                    { 2, "CODE2", 20.0, 20 },
                    { 3, "CODE3", 30.0, 30 },
                    { 4, "CODE4", 40.0, 40 },
                    { 5, "CODE5", 50.0, 50 },
                    { 6, "CODE6", 60.0, 60 },
                    { 7, "CODE7", 70.0, 70 },
                    { 8, "CODE8", 80.0, 80 },
                    { 9, "CODE9", 90.0, 90 },
                    { 10, "CODE10", 100.0, 100 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coupons");
        }
    }
}
