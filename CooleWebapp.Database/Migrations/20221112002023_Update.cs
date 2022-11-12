using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CooleWebapp.Database.Migrations
{
    public partial class Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MonthlyClosing",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Balance = table.Column<decimal>(type: "TEXT", nullable: false),
                    CoolUserId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Number = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyClosing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthlyClosing_CoolUsers_CoolUserId",
                        column: x => x.CoolUserId,
                        principalTable: "CoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyClosing_CoolUserId",
                table: "MonthlyClosing",
                column: "CoolUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyClosing_Number_CoolUserId",
                table: "MonthlyClosing",
                columns: new[] { "Number", "CoolUserId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonthlyClosing");
        }
    }
}
