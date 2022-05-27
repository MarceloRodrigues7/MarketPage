using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketPage.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarrinhoItem",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdItem = table.Column<long>(type: "bigint", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tamanhos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    DataHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdPedido = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarrinhoItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    DataAdicao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CodPromocoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desconto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFinal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    Utilizacoes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodPromocoes", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "EnderecosUsuario",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    Pais = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cidade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bairro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    Cep = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnderecosUsuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FretesPedidosUsuarios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdCarrinho = table.Column<long>(type: "bigint", nullable: false),
                    TipoFrete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FretesPedidosUsuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FreteValores",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CepInicio = table.Column<long>(type: "bigint", nullable: false),
                    CepFinal = table.Column<long>(type: "bigint", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "ImagensItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdItem = table.Column<long>(type: "bigint", nullable: false),
                    Img = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Principal = table.Column<bool>(type: "bit", nullable: false),
                    Tamanhos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataAdicao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagensItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Itens",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tamanhos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    Destaque = table.Column<bool>(type: "bit", nullable: false),
                    DataAdicao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdCategoria = table.Column<int>(type: "int", nullable: false),
                    Peso = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Itens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MessagesContato",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Texto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataEnvio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Visualizado = table.Column<bool>(type: "bit", nullable: false),
                    DataVisualizado = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessagesContato", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PedidosStatus",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomeMercadoLivre = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidosStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PedidosUsuario",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Pais = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cidade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bairro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    DataRealizacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusAtual = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateFinalizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdMercadoPago = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodRastreio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrazoEntrega = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidosUsuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlataformasPagamento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TokenService = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TokenClient = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlataformasPagamento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sobrenome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataNascimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RoleAcess = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusAtivo = table.Column<bool>(type: "bit", nullable: false),
                    PermiteEmail = table.Column<bool>(type: "bit", nullable: false),
                    ConcordaRegras = table.Column<bool>(type: "bit", nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "Ativo", "DataAdicao", "Nome" },
                values: new object[] { 1, true, new DateTime(2022, 5, 27, 17, 40, 14, 86, DateTimeKind.Local).AddTicks(4481), "Destaques" });

            migrationBuilder.InsertData(
                table: "FreteValores",
                columns: new[] { "Id", "CepFinal", "CepInicio", "PesoMax", "PesoMin", "PrazoMax", "PrazoMin", "PrecoFrete", "Servico" },
                values: new object[,]
                {
                    { 1L, 9223372036854775807L, 0L, 999, 0, 999, 0, 10m, "PAC" },
                    { 2L, 9223372036854775807L, 0L, 999, 0, 999, 0, 15m, "SEDEX" }
                });

            migrationBuilder.InsertData(
                table: "PlataformasPagamento",
                columns: new[] { "Id", "Nome", "TokenClient", "TokenService" },
                values: new object[] { 1, "MercadoPago", "APP_USR-8824013a-f82d-4879-a34a-634eaac91242", "APP_USR-1223540178250481-092615-8aee4b2461ec8e00fd5f066bcbd83d26-194500220" });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "ConcordaRegras", "DataNascimento", "Email", "Nome", "Password", "PermiteEmail", "RoleAcess", "Sobrenome", "StatusAtivo", "Telefone", "Username" },
                values: new object[] { 1, true, new DateTime(1990, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@marketpage.com", "Admin", "mudar123", true, "Admin", "teste", true, "10203040", "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarrinhoItem");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "CodPromocoes");

            migrationBuilder.DropTable(
                name: "CodPromoUsuarios");

            migrationBuilder.DropTable(
                name: "EnderecosUsuario");

            migrationBuilder.DropTable(
                name: "FretesPedidosUsuarios");

            migrationBuilder.DropTable(
                name: "FreteValores");

            migrationBuilder.DropTable(
                name: "ImagensItem");

            migrationBuilder.DropTable(
                name: "Itens");

            migrationBuilder.DropTable(
                name: "MessagesContato");

            migrationBuilder.DropTable(
                name: "PedidosStatus");

            migrationBuilder.DropTable(
                name: "PedidosUsuario");

            migrationBuilder.DropTable(
                name: "PlataformasPagamento");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
