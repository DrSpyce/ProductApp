using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProductModelEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_DbName",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "DbName",
                table: "Products",
                newName: "ShownName");

            migrationBuilder.AddColumn<int>(
                name: "NumberName",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ShownName",
                table: "Products",
                column: "ShownName",
                unique: true,
                filter: "[ShownName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_ShownName",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NumberName",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "ShownName",
                table: "Products",
                newName: "DbName");

            migrationBuilder.CreateIndex(
                name: "IX_Products_DbName",
                table: "Products",
                column: "DbName",
                unique: true,
                filter: "[DbName] IS NOT NULL");
        }
    }
}
