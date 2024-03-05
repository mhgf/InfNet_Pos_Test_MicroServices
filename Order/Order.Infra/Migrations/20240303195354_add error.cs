using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Order.Infra.Migrations
{
    /// <inheritdoc />
    public partial class adderror : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Error",
                table: "Orders",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Error",
                table: "Orders");
        }
    }
}
