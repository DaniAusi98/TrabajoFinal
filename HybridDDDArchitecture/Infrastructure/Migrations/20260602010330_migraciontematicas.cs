using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class migraciontematicas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VisitaGuiadaTematicas_TematicaVisita_TematicaId",
                table: "VisitaGuiadaTematicas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TematicaVisita",
                table: "TematicaVisita");

            migrationBuilder.RenameTable(
                name: "TematicaVisita",
                newName: "TematicasVisita");

            migrationBuilder.UpdateData(
                table: "TematicasVisita",
                keyColumn: "Nombre",
                keyValue: null,
                column: "Nombre",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "TematicasVisita",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "TematicasVisita",
                keyColumn: "Descripcion",
                keyValue: null,
                column: "Descripcion",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "TematicasVisita",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "Disponible",
                table: "TematicasVisita",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TematicasVisita",
                table: "TematicasVisita",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitaGuiadaTematicas_TematicasVisita_TematicaId",
                table: "VisitaGuiadaTematicas",
                column: "TematicaId",
                principalTable: "TematicasVisita",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VisitaGuiadaTematicas_TematicasVisita_TematicaId",
                table: "VisitaGuiadaTematicas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TematicasVisita",
                table: "TematicasVisita");

            migrationBuilder.DropColumn(
                name: "Disponible",
                table: "TematicasVisita");

            migrationBuilder.RenameTable(
                name: "TematicasVisita",
                newName: "TematicaVisita");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "TematicaVisita",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "TematicaVisita",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TematicaVisita",
                table: "TematicaVisita",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitaGuiadaTematicas_TematicaVisita_TematicaId",
                table: "VisitaGuiadaTematicas",
                column: "TematicaId",
                principalTable: "TematicaVisita",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
