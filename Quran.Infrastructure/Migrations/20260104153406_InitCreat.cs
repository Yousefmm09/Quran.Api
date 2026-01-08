using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quran.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitCreat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Surah",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Transliteration = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RevelationPlaceAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RevelationPlaceEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VersesCount = table.Column<int>(type: "int", nullable: false),
                    WordsCount = table.Column<int>(type: "int", nullable: false),
                    LettersCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surah", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AudioRecitations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReciterId = table.Column<int>(type: "int", nullable: false),
                    ReciterNameAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ReciterNameEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RewayaAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RewayaEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Server = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SurahId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AudioRecitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AudioRecitations_Surah_SurahId",
                        column: x => x.SurahId,
                        principalTable: "Surah",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Verses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    TextAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TextEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Juz = table.Column<int>(type: "int", nullable: false),
                    Page = table.Column<int>(type: "int", nullable: false),
                    HasSajda = table.Column<bool>(type: "bit", nullable: false),
                    SajdaId = table.Column<int>(type: "int", nullable: true),
                    SajdaRecommended = table.Column<bool>(type: "bit", nullable: true),
                    SajdaObligatory = table.Column<bool>(type: "bit", nullable: true),
                    SurahId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Verses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Verses_Surah_SurahId",
                        column: x => x.SurahId,
                        principalTable: "Surah",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AudioRecitations_SurahId",
                table: "AudioRecitations",
                column: "SurahId");

            migrationBuilder.CreateIndex(
                name: "IX_Verses_HasSajda",
                table: "Verses",
                column: "HasSajda");

            migrationBuilder.CreateIndex(
                name: "IX_Verses_SurahId",
                table: "Verses",
                column: "SurahId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AudioRecitations");

            migrationBuilder.DropTable(
                name: "Verses");

            migrationBuilder.DropTable(
                name: "Surah");
        }
    }
}
