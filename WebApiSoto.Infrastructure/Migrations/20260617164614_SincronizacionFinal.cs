using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiSoto.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SincronizacionFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. COMENTADO: Evitamos el error de renombrar una columna que no existe
            // migrationBuilder.RenameColumn(
            //     name: "ProductPrice",
            //     table: "SaleDetails",
            //     newName: "SalePrice");

            // 2. NUEVO: Como 'SalePrice' no existe en tu tabla 'SaleDetails', la agregamos directamente
            migrationBuilder.AddColumn<decimal>(
                name: "SalePrice",
                table: "SaleDetails",
                type: "decimal(18,2)", // De paso le asignamos el tipo de dato correcto sin advertencias
                nullable: true);

            // Cambios en Categorías (Se mantienen igual)
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Categories");

            // Eliminamos la columna SalePrice si se revierte la migración
            migrationBuilder.DropColumn(
                name: "SalePrice",
                table: "SaleDetails");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}