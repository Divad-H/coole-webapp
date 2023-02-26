using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CooleWebapp.Database.Migrations
{
    public partial class AddUserSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CoolUserId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    BuyOnFridgePermission = table.Column<int>(type: "INTEGER", nullable: false),
                    BuyOnFridgePinCodeHash = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSettings_CoolUsers_CoolUserId",
                        column: x => x.CoolUserId,
                        principalTable: "CoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_CoolUserId",
                table: "UserSettings",
                column: "CoolUserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSettings");
        }
    }
}
