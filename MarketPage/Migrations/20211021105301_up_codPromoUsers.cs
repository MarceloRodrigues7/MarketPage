using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketPage.Migrations
{
    public partial class up_codPromoUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdPedido",
                table: "FretesPedidosUsuarios",
                newName: "IdCarrinho");

            migrationBuilder.AddColumn<decimal>(
                name: "Peso",
                table: "Itens",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "CodPromoUsuarios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCodPromocao = table.Column<long>(type: "bigint", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdCarrinho = table.Column<long>(type: "bigint", nullable: false),
                    DataUtilizacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodPromoUsuarios", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodPromoUsuarios");

            migrationBuilder.DropColumn(
                name: "Peso",
                table: "Itens");

            migrationBuilder.RenameColumn(
                name: "IdCarrinho",
                table: "FretesPedidosUsuarios",
                newName: "IdPedido");
        }
    }
}
