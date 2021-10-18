using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketPage.Migrations
{
    public partial class up_fretesPedidosUsuarios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodPromocional",
                table: "PedidosUsuario");

            migrationBuilder.DropColumn(
                name: "TipoFrete",
                table: "PedidosUsuario");

            migrationBuilder.CreateTable(
                name: "FretesPedidosUsuarios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    TipoFrete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FretesPedidosUsuarios", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FretesPedidosUsuarios");

            migrationBuilder.AddColumn<string>(
                name: "CodPromocional",
                table: "PedidosUsuario",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoFrete",
                table: "PedidosUsuario",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
