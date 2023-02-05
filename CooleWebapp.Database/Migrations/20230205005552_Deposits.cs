using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CooleWebapp.Database.Migrations
{
    public partial class Deposits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Deposits",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CoolUserId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    MonthlyClosingId = table.Column<ulong>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deposits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deposits_CoolUsers_CoolUserId",
                        column: x => x.CoolUserId,
                        principalTable: "CoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Deposits_MonthlyClosings_MonthlyClosingId",
                        column: x => x.MonthlyClosingId,
                        principalTable: "MonthlyClosings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deposits_CoolUserId",
                table: "Deposits",
                column: "CoolUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Deposits_MonthlyClosingId",
                table: "Deposits",
                column: "MonthlyClosingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deposits");
        }
    }
}
