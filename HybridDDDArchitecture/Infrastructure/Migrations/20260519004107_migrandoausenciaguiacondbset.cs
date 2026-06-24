using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class migrandoausenciaguiacondbset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AusenciaGuia_Guias_GuiaId",
                table: "AusenciaGuia");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AusenciaGuia",
                table: "AusenciaGuia");

            migrationBuilder.RenameTable(
                name: "AusenciaGuia",
                newName: "AusenciasGuia");

            migrationBuilder.RenameColumn(
                name: "UsuarioInternoId",
                table: "Guias",
                newName: "PersonalInternoId");

            migrationBuilder.RenameIndex(
                name: "IX_AusenciaGuia_GuiaId",
                table: "AusenciasGuia",
                newName: "IX_AusenciasGuia_GuiaId");

            migrationBuilder.UpdateData(
                table: "AusenciasGuia",
                keyColumn: "Motivo",
                keyValue: null,
                column: "Motivo",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Motivo",
                table: "AusenciasGuia",
                type: "varchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AusenciasGuia",
                table: "AusenciasGuia",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AusenciasGuia_Guias_GuiaId",
                table: "AusenciasGuia",
                column: "GuiaId",
                principalTable: "Guias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AusenciasGuia_Guias_GuiaId",
                table: "AusenciasGuia");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AusenciasGuia",
                table: "AusenciasGuia");

            migrationBuilder.RenameTable(
                name: "AusenciasGuia",
                newName: "AusenciaGuia");

            migrationBuilder.RenameColumn(
                name: "PersonalInternoId",
                table: "Guias",
                newName: "UsuarioInternoId");

            migrationBuilder.RenameIndex(
                name: "IX_AusenciasGuia_GuiaId",
                table: "AusenciaGuia",
                newName: "IX_AusenciaGuia_GuiaId");

            migrationBuilder.AlterColumn<string>(
                name: "Motivo",
                table: "AusenciaGuia",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(300)",
                oldMaxLength: 300)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AusenciaGuia",
                table: "AusenciaGuia",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AusenciaGuia_Guias_GuiaId",
                table: "AusenciaGuia",
                column: "GuiaId",
                principalTable: "Guias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
