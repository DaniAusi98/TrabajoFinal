using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class clasesrefactorizadas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HorariosGuia");

            migrationBuilder.DropColumn(
                name: "FechaFin",
                table: "AusenciasGuia");

            migrationBuilder.DropColumn(
                name: "FechaInicio",
                table: "AusenciasGuia");

            migrationBuilder.DropColumn(
                name: "HoraFin",
                table: "AusenciasGuia");

            migrationBuilder.DropColumn(
                name: "HoraInicio",
                table: "AusenciasGuia");

            migrationBuilder.DropColumn(
                name: "Fecha",
                table: "ActividadTimeSlots");

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Guias",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaDesde",
                table: "AusenciasGuia",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaHasta",
                table: "AusenciasGuia",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Inicio",
                table: "ActividadTimeSlots",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time(6)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fin",
                table: "ActividadTimeSlots",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time(6)");

            migrationBuilder.CreateTable(
                name: "diascierremuseo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Fecha = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaHasta = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Motivo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Observaciones = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_diascierremuseo", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "salabloqueada",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FechaDesde = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaHasta = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Motivo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Observaciones = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salabloqueada", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SalaBloqueada_Sala",
                columns: table => new
                {
                    SalaBloqueadaMuseoId = table.Column<int>(type: "int", nullable: false),
                    SalasBloqueadasId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaBloqueada_Sala", x => new { x.SalaBloqueadaMuseoId, x.SalasBloqueadasId });
                    table.ForeignKey(
                        name: "FK_SalaBloqueada_Sala_Salas_SalasBloqueadasId",
                        column: x => x.SalasBloqueadasId,
                        principalTable: "Salas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalaBloqueada_Sala_salabloqueada_SalaBloqueadaMuseoId",
                        column: x => x.SalaBloqueadaMuseoId,
                        principalTable: "salabloqueada",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SalaBloqueada_Sala_SalasBloqueadasId",
                table: "SalaBloqueada_Sala",
                column: "SalasBloqueadasId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "diascierremuseo");

            migrationBuilder.DropTable(
                name: "SalaBloqueada_Sala");

            migrationBuilder.DropTable(
                name: "salabloqueada");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Guias");

            migrationBuilder.DropColumn(
                name: "FechaDesde",
                table: "AusenciasGuia");

            migrationBuilder.DropColumn(
                name: "FechaHasta",
                table: "AusenciasGuia");

            migrationBuilder.AddColumn<DateOnly>(
                name: "FechaFin",
                table: "AusenciasGuia",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "FechaInicio",
                table: "AusenciasGuia",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "HoraFin",
                table: "AusenciasGuia",
                type: "time(6)",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "HoraInicio",
                table: "AusenciasGuia",
                type: "time(6)",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "Inicio",
                table: "ActividadTimeSlots",
                type: "time(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "Fin",
                table: "ActividadTimeSlots",
                type: "time(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Fecha",
                table: "ActividadTimeSlots",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.CreateTable(
                name: "HorariosGuia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DiaSemana = table.Column<int>(type: "int", nullable: false),
                    GuiaId = table.Column<int>(type: "int", nullable: false),
                    HoraFin = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    HoraInicio = table.Column<TimeOnly>(type: "time(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorariosGuia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HorariosGuia_Guias_GuiaId",
                        column: x => x.GuiaId,
                        principalTable: "Guias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_HorariosGuia_GuiaId",
                table: "HorariosGuia",
                column: "GuiaId");
        }
    }
}
