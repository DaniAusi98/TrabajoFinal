using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class rruleactividadexceptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActividadException_ActividadesMuseo_ActividadMuseoId",
                table: "ActividadException");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActividadException",
                table: "ActividadException");

            migrationBuilder.DropIndex(
                name: "IX_ActividadException_ActividadMuseoId",
                table: "ActividadException");

            migrationBuilder.RenameTable(
                name: "ActividadException",
                newName: "ActividadExceptions");

            migrationBuilder.AlterColumn<string>(
                name: "Motivo",
                table: "ActividadExceptions",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaExcluir",
                table: "ActividadExceptions",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.UpdateData(
                table: "ActividadExceptions",
                keyColumn: "ActividadMuseoId",
                keyValue: null,
                column: "ActividadMuseoId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ActividadMuseoId",
                table: "ActividadExceptions",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "ActividadExceptions",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActividadExceptions",
                table: "ActividadExceptions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ActividadExceptions_ActividadId_Fecha",
                table: "ActividadExceptions",
                columns: new[] { "ActividadMuseoId", "FechaExcluir" });

            migrationBuilder.AddForeignKey(
                name: "FK_ActividadExceptions_ActividadesMuseo_ActividadMuseoId",
                table: "ActividadExceptions",
                column: "ActividadMuseoId",
                principalTable: "ActividadesMuseo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActividadExceptions_ActividadesMuseo_ActividadMuseoId",
                table: "ActividadExceptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActividadExceptions",
                table: "ActividadExceptions");

            migrationBuilder.DropIndex(
                name: "IX_ActividadExceptions_ActividadId_Fecha",
                table: "ActividadExceptions");

            migrationBuilder.RenameTable(
                name: "ActividadExceptions",
                newName: "ActividadException");

            migrationBuilder.AlterColumn<string>(
                name: "Motivo",
                table: "ActividadException",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaExcluir",
                table: "ActividadException",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "ActividadMuseoId",
                table: "ActividadException",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "ActividadException",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActividadException",
                table: "ActividadException",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ActividadException_ActividadMuseoId",
                table: "ActividadException",
                column: "ActividadMuseoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActividadException_ActividadesMuseo_ActividadMuseoId",
                table: "ActividadException",
                column: "ActividadMuseoId",
                principalTable: "ActividadesMuseo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
