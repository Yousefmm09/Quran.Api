using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quran.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_TextArabi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // إضافة العمود
            migrationBuilder.AddColumn<string>(
                name: "TextArabicSearch",
                table: "Verses",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            // إنشاء Index
            migrationBuilder.CreateIndex(
                name: "IX_Verses_TextArabicSearch",
                table: "Verses",
                column: "TextArabicSearch");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Verses_TextArabicSearch",
                table: "Verses");

            migrationBuilder.DropColumn(
                name: "TextArabicSearch",
                table: "Verses");
        }
    }
}
