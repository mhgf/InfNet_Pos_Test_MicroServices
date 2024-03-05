using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Product.Infra.Migrations
{
    /// <inheritdoc />
    public partial class Changecolumname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Products",
                newName: "productValue");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "productValue",
                table: "Products",
                newName: "Value");
        }
    }
}
