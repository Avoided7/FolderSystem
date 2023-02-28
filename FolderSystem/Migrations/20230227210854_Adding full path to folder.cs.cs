using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FolderSystem.Migrations
{
    /// <inheritdoc />
    public partial class Addingfullpathtofoldercs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullPath",
                table: "Folders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullPath",
                table: "Folders");
        }
    }
}
