using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketPage.Migrations
{
    public partial class up_login_termos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ConcordaRegras",
                table: "Usuarios",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PermiteEmail",
                table: "Usuarios",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConcordaRegras",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "PermiteEmail",
                table: "Usuarios");
        }
    }
}
