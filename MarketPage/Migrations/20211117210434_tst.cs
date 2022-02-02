using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace MarketPage.Migrations
{
    public partial class tst : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string[] column = new[] { "Username", "Password", "Email", "Nome", "Sobrenome", "DataNascimento", "RoleAcess", "StatusAtivo", "PermiteEmail", "ConcordaRegras", "Telefone" };
            var data = new DateTime(1990, 01, 01);
            var admin = new object[] { "admin", "mudar123", "admin@mudar.com", "Admin", "Primary", data, "Admin", true, true, true, "19996134321" };
            migrationBuilder.InsertData("Usuarios", column, admin);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
