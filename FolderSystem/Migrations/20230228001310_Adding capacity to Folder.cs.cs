using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FolderSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddingcapacitytoFoldercs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Folders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Folders");
        }
    }
}
