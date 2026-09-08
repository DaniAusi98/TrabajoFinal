using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class rrule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActividadRecurrencias");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "ActividadException");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaExcluir",
                table: "ActividadException",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Motivo",
                table: "ActividadException",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "RRule",
                table: "ActividadesMuseo",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaExcluir",
                table: "ActividadException");

            migrationBuilder.DropColumn(
                name: "Motivo",
                table: "ActividadException");

            migrationBuilder.DropColumn(
                name: "RRule",
                table: "ActividadesMuseo");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "ActividadException",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.CreateTable(
                name: "ActividadRecurrencias",
                columns: table => new
                {
                    ActividadMuseoId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ByDays = table.Column<string>(type: "json", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Frequency = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Interval = table.Column<int>(type: "int", nullable: false),
                    LastWeekOfMonth = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    MonthDay = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    WeekOfMonth = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActividadRecurrencias", x => x.ActividadMuseoId);
                    table.ForeignKey(
                        name: "FK_ActividadRecurrencias_ActividadesMuseo_ActividadMuseoId",
                        column: x => x.ActividadMuseoId,
                        principalTable: "ActividadesMuseo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
