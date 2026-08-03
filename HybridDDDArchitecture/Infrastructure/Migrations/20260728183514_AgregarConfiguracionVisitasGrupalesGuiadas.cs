using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarConfiguracionVisitasGrupalesGuiadas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfiguracionVisitasGrupalesGuiadas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MinGuiasParaCapacidadCompleta = table.Column<int>(type: "int", nullable: false),
                    CapacidadPorGuia = table.Column<int>(type: "int", nullable: false),
                    CapacidadMaximaPorTurno = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracionVisitasGrupalesGuiadas", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "ConfiguracionVisitasGrupalesGuiadas",
                columns: new[] { "Id", "CapacidadMaximaPorTurno", "CapacidadPorGuia", "MinGuiasParaCapacidadCompleta" },
                values: new object[] { -1, 50, 25, 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfiguracionVisitasGrupalesGuiadas");
        }
    }
}
