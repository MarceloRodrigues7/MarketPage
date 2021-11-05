using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketPage.Migrations
{
    public partial class up_tbFreteValores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FreteValores",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CepInicio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CepFinal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PesoMin = table.Column<int>(type: "int", nullable: false),
                    PesoMax = table.Column<int>(type: "int", nullable: false),
                    PrecoFrete = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrazoMin = table.Column<int>(type: "int", nullable: false),
                    PrazoMax = table.Column<int>(type: "int", nullable: false),
                    Servico = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreteValores", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FreteValores");
        }
    }
}
