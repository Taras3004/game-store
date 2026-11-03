using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameStore.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ParentGenreId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Genres_Genres_ParentGenreId",
                        column: x => x.ParentGenreId,
                        principalTable: "Genres",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Platforms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platforms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameGenres",
                columns: table => new
                {
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GenreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameGenres", x => new { x.GameId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_GameGenres_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameGenres_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GamePlatforms",
                columns: table => new
                {
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlatformId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePlatforms", x => new { x.GameId, x.PlatformId });
                    table.ForeignKey(
                        name: "FK_GamePlatforms_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GamePlatforms_Platforms_PlatformId",
                        column: x => x.PlatformId,
                        principalTable: "Platforms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name", "ParentGenreId" },
                values: new object[,]
                {
                    { new Guid("03a26411-5cc5-4d33-9ea4-6a37c0ef4b80"), "RPG", null },
                    { new Guid("60ab9513-66ba-4be0-9d24-12022b7b08e0"), "Action", null },
                    { new Guid("9975044e-ce10-4e53-b016-cb297bb24342"), "Races", null },
                    { new Guid("aee1de92-b2fc-4c1c-8123-9e650434f66e"), "Puzzle & Skill", null },
                    { new Guid("b2fad613-f3bb-4bb1-8934-26bdedcf8e4e"), "Strategy", null },
                    { new Guid("bb677f47-21b3-439b-b5a1-beca6676d458"), "Sports", null },
                    { new Guid("ee4da31c-68a1-488c-9371-43be311989e9"), "Adventure", null },
                    { new Guid("eead926d-2812-447d-b22d-1b32fa2d4867"), "Arcade", null }
                });

            migrationBuilder.InsertData(
                table: "Platforms",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { new Guid("075297b1-4919-468a-a735-066e90293b1e"), "Browser" },
                    { new Guid("5afe59e6-50cb-43f1-90cd-f6bf3f04ac3e"), "Mobile" },
                    { new Guid("7d55d6a2-204c-40c8-88e0-7e84d10f9289"), "Desktop" },
                    { new Guid("b23d6c95-dbf7-4844-88e0-7fdf3347a1f0"), "Console" }
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name", "ParentGenreId" },
                values: new object[,]
                {
                    { new Guid("391e724c-1f17-4404-8ccc-e3c1f3921c42"), "Formula", new Guid("9975044e-ce10-4e53-b016-cb297bb24342") },
                    { new Guid("46d251bd-ce71-46f8-a18a-2a7af3677677"), "Off-road", new Guid("9975044e-ce10-4e53-b016-cb297bb24342") },
                    { new Guid("88f158e3-8fe7-4e0f-ba0f-5810819e6824"), "TPS", new Guid("60ab9513-66ba-4be0-9d24-12022b7b08e0") },
                    { new Guid("92bde005-3f33-409f-89fe-3e0835acf46a"), "Rally", new Guid("9975044e-ce10-4e53-b016-cb297bb24342") },
                    { new Guid("f56ae5b6-c13c-442f-8bcc-c21a6c19ec68"), "RTS", new Guid("b2fad613-f3bb-4bb1-8934-26bdedcf8e4e") },
                    { new Guid("f7b6abd9-8b9d-4bf4-9084-d3d651b59ef7"), "TBS", new Guid("b2fad613-f3bb-4bb1-8934-26bdedcf8e4e") },
                    { new Guid("f8004a98-1a49-4370-97c6-94589d0a6335"), "FPS", new Guid("60ab9513-66ba-4be0-9d24-12022b7b08e0") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameGenres_GenreId",
                table: "GameGenres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_GamePlatforms_PlatformId",
                table: "GamePlatforms",
                column: "PlatformId");

            migrationBuilder.CreateIndex(
                name: "IX_Genres_ParentGenreId",
                table: "Genres",
                column: "ParentGenreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameGenres");

            migrationBuilder.DropTable(
                name: "GamePlatforms");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Platforms");
        }
    }
}
