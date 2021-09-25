using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketPage.Migrations
{
    public partial class upItens_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tamanhos",
                table: "Itens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tamanhos",
                table: "ImagensItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tamanhos",
                table: "CarrinhoItem",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tamanhos",
                table: "Itens");

            migrationBuilder.DropColumn(
                name: "Tamanhos",
                table: "ImagensItem");

            migrationBuilder.DropColumn(
                name: "Tamanhos",
                table: "CarrinhoItem");
        }
    }
}
