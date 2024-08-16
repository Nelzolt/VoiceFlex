using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VoiceFlex.Migrations
{
    /// <inheritdoc />
    public partial class NewIndexOnPhoneNumberTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_VOICEFLEX_PhoneNumbers_Number",
                table: "VOICEFLEX_PhoneNumbers",
                column: "Number",
                unique: true,
                filter: "[Number] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VOICEFLEX_PhoneNumbers_Number",
                table: "VOICEFLEX_PhoneNumbers");
        }
    }
}
