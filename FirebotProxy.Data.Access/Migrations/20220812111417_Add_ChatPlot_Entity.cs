using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirebotProxy.Data.Access.Migrations
{
    public partial class Add_ChatPlot_Entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatPlots",
                columns: table => new
                {
                    ViewerUsername = table.Column<string>(type: "TEXT", nullable: false),
                    ChartUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatPlots", x => x.ViewerUsername);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatPlots");
        }
    }
}
