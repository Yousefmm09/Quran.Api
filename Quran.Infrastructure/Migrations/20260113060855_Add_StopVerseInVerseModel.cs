using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quran.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_StopVerseInVerseModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SajdaObligatory",
                table: "Verses");

            migrationBuilder.DropColumn(
                name: "SajdaRecommended",
                table: "Verses");

            migrationBuilder.AlterColumn<string>(
                name: "TextArabicSearch",
                table: "Verses",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AddColumn<bool>(
                name: "StopVerse",
                table: "Verses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StopVerse",
                table: "Verses");

            migrationBuilder.AlterColumn<string>(
                name: "TextArabicSearch",
                table: "Verses",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SajdaObligatory",
                table: "Verses",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SajdaRecommended",
                table: "Verses",
                type: "bit",
                nullable: true);
        }
    }
}
