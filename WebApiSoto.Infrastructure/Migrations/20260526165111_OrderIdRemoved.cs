using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiSoto.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OrderIdRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Sales");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Sales",
                type: "int",
                nullable: true);
        }
    }
}
