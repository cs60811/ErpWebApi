using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class FinalNamingConvention : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_userProfiles",
                table: "userProfiles");

            migrationBuilder.RenameTable(
                name: "userProfiles",
                newName: "UserProfiles");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "UserProfiles",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "UserProfiles",
                newName: "CreateTime");

            migrationBuilder.RenameIndex(
                name: "IX_userProfiles_UserId",
                table: "UserProfiles",
                newName: "IX_UserProfiles_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProfiles",
                table: "UserProfiles",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProfiles",
                table: "UserProfiles");

            migrationBuilder.RenameTable(
                name: "UserProfiles",
                newName: "userProfiles");

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "userProfiles",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "CreateTime",
                table: "userProfiles",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_UserProfiles_UserId",
                table: "userProfiles",
                newName: "IX_userProfiles_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_userProfiles",
                table: "userProfiles",
                column: "Id");
        }
    }
}
