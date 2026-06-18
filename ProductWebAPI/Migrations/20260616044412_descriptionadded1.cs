using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class descriptionadded1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "products",
                type: "varchar(max)",
                nullable: true
               );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "products");
        }
    }
}
