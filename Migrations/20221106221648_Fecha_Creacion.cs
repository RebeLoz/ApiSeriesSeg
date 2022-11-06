using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiSeries.Migrations
{
    public partial class Fecha_Creacion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Categorias",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Categorias");
        }
    }
}
