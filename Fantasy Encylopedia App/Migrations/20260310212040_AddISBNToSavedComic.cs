using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fantasy_Encylopedia_App.Migrations
{
    /// <inheritdoc />
    public partial class AddISBNToSavedComic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ISBN",
                table: "SaveComic",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ISBN",
                table: "SaveComic");
        }
    }
}
