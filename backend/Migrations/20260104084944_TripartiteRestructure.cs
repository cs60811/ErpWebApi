using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class TripartiteRestructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ErpConfigs");

            migrationBuilder.DropColumn(
                name: "ErpId",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "ErpUid",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "ErpUpwd",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "IsErpBound",
                table: "UserProfiles");

            migrationBuilder.CreateTable(
                name: "ErpUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ErpCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BaseUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CID = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErpUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserCustomers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserProfileId = table.Column<int>(type: "integer", nullable: false),
                    ErpUserId = table.Column<int>(type: "integer", nullable: false),
                    CustomerUid = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CustomerUpwd = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCustomers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCustomers_ErpUsers_ErpUserId",
                        column: x => x.ErpUserId,
                        principalTable: "ErpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCustomers_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ErpUsers_ErpCode",
                table: "ErpUsers",
                column: "ErpCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCustomers_ErpUserId",
                table: "UserCustomers",
                column: "ErpUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCustomers_UserProfileId",
                table: "UserCustomers",
                column: "UserProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCustomers");

            migrationBuilder.DropTable(
                name: "ErpUsers");

            migrationBuilder.AddColumn<string>(
                name: "ErpId",
                table: "UserProfiles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ErpUid",
                table: "UserProfiles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ErpUpwd",
                table: "UserProfiles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsErpBound",
                table: "UserProfiles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ErpConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BaseUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CID = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ErpCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LoginType = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UID = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UPWD = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErpConfigs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ErpConfigs_ErpCode",
                table: "ErpConfigs",
                column: "ErpCode",
                unique: true);
        }
    }
}
