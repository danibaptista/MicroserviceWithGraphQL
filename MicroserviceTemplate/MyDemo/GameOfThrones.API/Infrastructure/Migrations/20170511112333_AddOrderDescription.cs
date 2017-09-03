using Microsoft.EntityFrameworkCore.Migrations;

namespace MicroserviceArchitecture.GameOfThrones.API.Migrations
{
    public partial class AddOrderDescription : Migration
    {
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "ordering",
                table: "orders");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "ordering",
                table: "orders",
                nullable: true);
        }
    }
}