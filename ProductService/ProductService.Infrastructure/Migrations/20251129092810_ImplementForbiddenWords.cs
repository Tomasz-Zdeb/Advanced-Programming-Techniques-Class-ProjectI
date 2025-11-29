using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProductService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ImplementForbiddenWords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ForbiddenWords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Word = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForbiddenWords", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ForbiddenWords",
                columns: new[] { "Id", "Word" },
                values: new object[,]
                {
                    { 1, "python" },
                    { 2, "javascript" },
                    { 3, "mvc" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ForbiddenWords_Word",
                table: "ForbiddenWords",
                column: "Word",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForbiddenWords");
        }
    }
}
