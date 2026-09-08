using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class configuraciondesalas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfiguracionesSalaActividad",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SalaId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoActividad = table.Column<int>(type: "int", nullable: false),
                    Habilitada = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CapacidadMaxima = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracionesSalaActividad", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracionesSalaActividad_SalaId",
                table: "ConfiguracionesSalaActividad",
                column: "SalaId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracionesSalaActividad_SalaId_TipoActividad",
                table: "ConfiguracionesSalaActividad",
                columns: new[] { "SalaId", "TipoActividad" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracionesSalaActividad_TipoActividad",
                table: "ConfiguracionesSalaActividad",
                column: "TipoActividad");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfiguracionesSalaActividad");
        }
    }
}
