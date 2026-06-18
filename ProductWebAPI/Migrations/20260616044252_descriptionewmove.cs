using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class descriptionewmove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
