using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CooleWebapp.Database.Migrations
{
    /// <inheritdoc />
    public partial class MakeWebappUserHardDeletable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoolUsers_AspNetUsers_WebappUserId",
                table: "CoolUsers");

            migrationBuilder.DropIndex(
                name: "IX_CoolUsers_WebappUserId",
                table: "CoolUsers");

            migrationBuilder.AlterColumn<string>(
                name: "WebappUserId",
                table: "CoolUsers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_CoolUsers_WebappUserId",
                table: "CoolUsers",
                column: "WebappUserId",
                unique: true,
                filter: "[WebappUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_CoolUsers_AspNetUsers_WebappUserId",
                table: "CoolUsers",
                column: "WebappUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoolUsers_AspNetUsers_WebappUserId",
                table: "CoolUsers");

            migrationBuilder.DropIndex(
                name: "IX_CoolUsers_WebappUserId",
                table: "CoolUsers");

            migrationBuilder.AlterColumn<string>(
                name: "WebappUserId",
                table: "CoolUsers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoolUsers_WebappUserId",
                table: "CoolUsers",
                column: "WebappUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CoolUsers_AspNetUsers_WebappUserId",
                table: "CoolUsers",
                column: "WebappUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
