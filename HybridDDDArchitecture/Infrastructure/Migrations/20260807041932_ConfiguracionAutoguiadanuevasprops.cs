using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConfiguracionAutoguiadanuevasprops : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DuracionSlot",
                table: "ConfiguracionVisitaAutoguiada",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "VisitasSimultaneasMaximas",
                table: "ConfiguracionVisitaAutoguiada",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BloqueosVisitasAutoguiadas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FechaDesde = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaHasta = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Motivo = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConfiguracionHorarioAutoguiadaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloqueosVisitasAutoguiadas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BloqueosVisitasAutoguiadas_ConfiguracionVisitaAutoguiada_Con~",
                        column: x => x.ConfiguracionHorarioAutoguiadaId,
                        principalTable: "ConfiguracionVisitaAutoguiada",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BloqueosVisitasGuiadas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FechaDesde = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaHasta = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Motivo = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConfiguracionVisitasGrupalesGuiadasId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloqueosVisitasGuiadas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BloqueosVisitasGuiadas_ConfiguracionVisitasGrupalesGuiadas_C~",
                        column: x => x.ConfiguracionVisitasGrupalesGuiadasId,
                        principalTable: "ConfiguracionVisitasGrupalesGuiadas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_BloqueosVisitasAutoguiadas_ConfiguracionHorarioAutoguiadaId",
                table: "BloqueosVisitasAutoguiadas",
                column: "ConfiguracionHorarioAutoguiadaId");

            migrationBuilder.CreateIndex(
                name: "IX_BloqueosVisitasGuiadas_ConfiguracionVisitasGrupalesGuiadasId",
                table: "BloqueosVisitasGuiadas",
                column: "ConfiguracionVisitasGrupalesGuiadasId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BloqueosVisitasAutoguiadas");

            migrationBuilder.DropTable(
                name: "BloqueosVisitasGuiadas");

            migrationBuilder.DropColumn(
                name: "DuracionSlot",
                table: "ConfiguracionVisitaAutoguiada");

            migrationBuilder.DropColumn(
                name: "VisitasSimultaneasMaximas",
                table: "ConfiguracionVisitaAutoguiada");
        }
    }
}
