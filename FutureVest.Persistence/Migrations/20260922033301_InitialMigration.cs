using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FutureVest.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsoCode = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Macroindicators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Weight = table.Column<decimal>(type: "decimal(5,4)", precision: 5, scale: 4, nullable: false),
                    IsHigherBetter = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Macroindicators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReturnRateSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinReturnRate = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: false),
                    MaxReturnRate = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnRateSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CountryIndicators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    MacroindicatorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryIndicators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CountryIndicators_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CountryIndicators_Macroindicators_MacroindicatorId",
                        column: x => x.MacroindicatorId,
                        principalTable: "Macroindicators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RankingSimulationMacroindicators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MacroindicatorId = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(5,4)", precision: 5, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RankingSimulationMacroindicators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RankingSimulationMacroindicators_Macroindicators_MacroindicatorId",
                        column: x => x.MacroindicatorId,
                        principalTable: "Macroindicators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Countries_IsoCode",
                table: "Countries",
                column: "IsoCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CountryIndicators_CountryId_MacroindicatorId_Year",
                table: "CountryIndicators",
                columns: new[] { "CountryId", "MacroindicatorId", "Year" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CountryIndicators_MacroindicatorId",
                table: "CountryIndicators",
                column: "MacroindicatorId");

            migrationBuilder.CreateIndex(
                name: "IX_RankingSimulationMacroindicators_MacroindicatorId",
                table: "RankingSimulationMacroindicators",
                column: "MacroindicatorId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CountryIndicators");

            migrationBuilder.DropTable(
                name: "RankingSimulationMacroindicators");

            migrationBuilder.DropTable(
                name: "ReturnRateSettings");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Macroindicators");
        }
    }
}
