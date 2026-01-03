using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNamingConvention : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_user_profiles",
                table: "user_profiles");

            migrationBuilder.RenameTable(
                name: "user_profiles",
                newName: "userProfiles");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "userProfiles",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "userProfiles",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "status_message",
                table: "userProfiles",
                newName: "StatusMessage");

            migrationBuilder.RenameColumn(
                name: "picture_url",
                table: "userProfiles",
                newName: "PictureUrl");

            migrationBuilder.RenameColumn(
                name: "display_name",
                table: "userProfiles",
                newName: "DisplayName");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "userProfiles",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_user_profiles_user_id",
                table: "userProfiles",
                newName: "IX_userProfiles_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_userProfiles",
                table: "userProfiles",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_userProfiles",
                table: "userProfiles");

            migrationBuilder.RenameTable(
                name: "userProfiles",
                newName: "user_profiles");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "user_profiles",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "user_profiles",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "StatusMessage",
                table: "user_profiles",
                newName: "status_message");

            migrationBuilder.RenameColumn(
                name: "PictureUrl",
                table: "user_profiles",
                newName: "picture_url");

            migrationBuilder.RenameColumn(
                name: "DisplayName",
                table: "user_profiles",
                newName: "display_name");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "user_profiles",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_userProfiles_UserId",
                table: "user_profiles",
                newName: "IX_user_profiles_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_profiles",
                table: "user_profiles",
                column: "Id");
        }
    }
}
