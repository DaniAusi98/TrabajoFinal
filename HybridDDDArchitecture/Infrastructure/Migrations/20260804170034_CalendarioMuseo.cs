using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CalendarioMuseo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CalendarioMuseoId",
                table: "diascierremuseo",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CalendariosMuseo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    HoraApertura = table.Column<TimeOnly>(type: "time(6)", nullable: true),
                    HoraCierre = table.Column<TimeOnly>(type: "time(6)", nullable: true),
                    DiasApertura = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendariosMuseo", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_diascierremuseo_CalendarioMuseoId",
                table: "diascierremuseo",
                column: "CalendarioMuseoId");

            migrationBuilder.AddForeignKey(
                name: "FK_diascierremuseo_CalendariosMuseo_CalendarioMuseoId",
                table: "diascierremuseo",
                column: "CalendarioMuseoId",
                principalTable: "CalendariosMuseo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_diascierremuseo_CalendariosMuseo_CalendarioMuseoId",
                table: "diascierremuseo");

            migrationBuilder.DropTable(
                name: "CalendariosMuseo");

            migrationBuilder.DropIndex(
                name: "IX_diascierremuseo_CalendarioMuseoId",
                table: "diascierremuseo");

            migrationBuilder.DropColumn(
                name: "CalendarioMuseoId",
                table: "diascierremuseo");
        }
    }
}
