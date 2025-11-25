using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProductService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Laptop", 3999.99m },
                    { 2, "Programming Book From Cracow University of Technology", 59.99m },
                    { 3, "Wireless Mouse", 129.99m },
                    { 4, "Regular Keyboard", 449.99m },
                    { 5, "USB-C Cable", 29.99m },
                    { 6, "Monitor 27inch", 1299.99m },
                    { 7, "Headphones", 299.99m },
                    { 8, "Webcam HD", 199.99m },
                    { 9, "Monitor 24inch", 599.99m },
                    { 10, "Gaming Laptop", 3999.99m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
