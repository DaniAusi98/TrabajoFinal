using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Provinciadepartamentolocalidaddto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CiudadInstitucion",
                table: "VisitasGrupalesAutoguiadas",
                newName: "LocalidadInstitucion");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioVisitanteId",
                table: "VisitasGrupalesAutoguiadas",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LocalidadInstitucion",
                table: "VisitasGrupalesAutoguiadas",
                newName: "CiudadInstitucion");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioVisitanteId",
                table: "VisitasGrupalesAutoguiadas",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
