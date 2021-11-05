using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketPage.Migrations
{
    public partial class up_Endereco_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cep",
                table: "EnderecosUsuario",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cep",
                table: "EnderecosUsuario");
        }
    }
}
