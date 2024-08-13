using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VoiceFlex.Migrations
{
    /// <inheritdoc />
    public partial class CreateTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VOICEFLEX_Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "varchar(1023)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VOICEFLEX_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VOICEFLEX_PhoneNumbers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<string>(type: "varchar(11)", nullable: true),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VOICEFLEX_PhoneNumbers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VOICEFLEX_PhoneNumbers_VOICEFLEX_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "VOICEFLEX_Accounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_VOICEFLEX_PhoneNumbers_AccountId",
                table: "VOICEFLEX_PhoneNumbers",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VOICEFLEX_PhoneNumbers");

            migrationBuilder.DropTable(
                name: "VOICEFLEX_Accounts");
        }
    }
}
