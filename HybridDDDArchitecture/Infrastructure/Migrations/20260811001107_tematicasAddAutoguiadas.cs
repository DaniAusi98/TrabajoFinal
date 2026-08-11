using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class tematicasAddAutoguiadas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VisitaGrupalAutoguiadaTematicas",
                columns: table => new
                {
                    VisitaGrupalAutoguiadaId = table.Column<int>(type: "int", nullable: false),
                    TematicaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitaGrupalAutoguiadaTematicas", x => new { x.VisitaGrupalAutoguiadaId, x.TematicaId });
                    table.ForeignKey(
                        name: "FK_VisitaGrupalAutoguiadaTematicas_TematicasVisita_TematicaId",
                        column: x => x.TematicaId,
                        principalTable: "TematicasVisita",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VisitaGrupalAutoguiadaTematicas_VisitasGrupalesAutoguiadas_V~",
                        column: x => x.VisitaGrupalAutoguiadaId,
                        principalTable: "VisitasGrupalesAutoguiadas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_VisitaGrupalAutoguiadaTematicas_TematicaId",
                table: "VisitaGrupalAutoguiadaTematicas",
                column: "TematicaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VisitaGrupalAutoguiadaTematicas");
        }
    }
}
