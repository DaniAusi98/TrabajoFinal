using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConfiguracionAutoguiadanintervalosescalonados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DuracionSlot",
                table: "ConfiguracionVisitaAutoguiada",
                newName: "IntervaloReservas");

            migrationBuilder.AddColumn<long>(
                name: "DuracionVisita",
                table: "ConfiguracionVisitaAutoguiada",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DuracionVisita",
                table: "ConfiguracionVisitaAutoguiada");

            migrationBuilder.RenameColumn(
                name: "IntervaloReservas",
                table: "ConfiguracionVisitaAutoguiada",
                newName: "DuracionSlot");
        }
    }
}
