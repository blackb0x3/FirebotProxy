﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirebotProxy.Data.Access.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CacheEntries",
                columns: table => new
                {
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastUpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CacheEntries", x => x.Key);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CacheEntries_CreatedOn",
                table: "CacheEntries",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_CacheEntries_LastUpdatedOn",
                table: "CacheEntries",
                column: "LastUpdatedOn");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CacheEntries");
        }
    }
}
