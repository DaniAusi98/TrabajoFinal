using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class entidadesCalendarioConfiguracionvisitasguiadas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ConfiguracionVisitasGrupalesGuiadas",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.AddColumn<string>(
                name: "DiasDisponibles",
                table: "ConfiguracionVisitasGrupalesGuiadas",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TurnosVisitasGuiadas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    HoraInicio = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    HoraFin = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    ConfiguracionVisitasGrupalesGuiadasId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TurnosVisitasGuiadas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TurnosVisitasGuiadas_ConfiguracionVisitasGrupalesGuiadas_Con~",
                        column: x => x.ConfiguracionVisitasGrupalesGuiadasId,
                        principalTable: "ConfiguracionVisitasGrupalesGuiadas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TurnosVisitasGuiadas_ConfiguracionVisitasGrupalesGuiadasId",
                table: "TurnosVisitasGuiadas",
                column: "ConfiguracionVisitasGrupalesGuiadasId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TurnosVisitasGuiadas");

            migrationBuilder.DropColumn(
                name: "DiasDisponibles",
                table: "ConfiguracionVisitasGrupalesGuiadas");

            migrationBuilder.InsertData(
                table: "ConfiguracionVisitasGrupalesGuiadas",
                columns: new[] { "Id", "CapacidadMaximaPorTurno", "CapacidadPorGuia", "MinGuiasParaCapacidadCompleta" },
                values: new object[] { -1, 50, 25, 2 });
        }
    }
}
