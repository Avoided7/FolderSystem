using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FolderSystem.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFolderIDprop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Folders_Folders_FolderId",
                table: "Folders");

            migrationBuilder.RenameColumn(
                name: "FolderId",
                table: "Folders",
                newName: "BaseFolderId");

            migrationBuilder.RenameIndex(
                name: "IX_Folders_FolderId",
                table: "Folders",
                newName: "IX_Folders_BaseFolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Folders_Folders_BaseFolderId",
                table: "Folders",
                column: "BaseFolderId",
                principalTable: "Folders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Folders_Folders_BaseFolderId",
                table: "Folders");

            migrationBuilder.RenameColumn(
                name: "BaseFolderId",
                table: "Folders",
                newName: "FolderId");

            migrationBuilder.RenameIndex(
                name: "IX_Folders_BaseFolderId",
                table: "Folders",
                newName: "IX_Folders_FolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Folders_Folders_FolderId",
                table: "Folders",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id");
        }
    }
}
