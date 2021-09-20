using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketPage.Migrations
{
    public partial class upmessageContato_melhoriaVisualizacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataEnvio",
                table: "MessagesContato",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DataVisualizado",
                table: "MessagesContato",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Visualizado",
                table: "MessagesContato",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataEnvio",
                table: "MessagesContato");

            migrationBuilder.DropColumn(
                name: "DataVisualizado",
                table: "MessagesContato");

            migrationBuilder.DropColumn(
                name: "Visualizado",
                table: "MessagesContato");
        }
    }
}
