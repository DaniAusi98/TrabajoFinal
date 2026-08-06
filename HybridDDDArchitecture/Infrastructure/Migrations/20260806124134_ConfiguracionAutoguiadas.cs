using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConfiguracionAutoguiadas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HoraCierre",
                table: "CalendariosMuseo",
                newName: "HoraInicio");

            migrationBuilder.RenameColumn(
                name: "HoraApertura",
                table: "CalendariosMuseo",
                newName: "HoraFin");

            migrationBuilder.CreateTable(
                name: "ConfiguracionVisitaAutoguiada",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DiasDisponibles = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HoraInicio = table.Column<TimeOnly>(type: "time(6)", nullable: true),
                    HoraFin = table.Column<TimeOnly>(type: "time(6)", nullable: true),
                    CapacidadMaximaPorGrupo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracionVisitaAutoguiada", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfiguracionVisitaAutoguiada");

            migrationBuilder.RenameColumn(
                name: "HoraInicio",
                table: "CalendariosMuseo",
                newName: "HoraCierre");

            migrationBuilder.RenameColumn(
                name: "HoraFin",
                table: "CalendariosMuseo",
                newName: "HoraApertura");
        }
    }
}
