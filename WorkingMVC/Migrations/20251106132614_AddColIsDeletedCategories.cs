using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkingMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddColIsDeletedCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "tbl_categories",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "tbl_categories");
        }
    }
}
