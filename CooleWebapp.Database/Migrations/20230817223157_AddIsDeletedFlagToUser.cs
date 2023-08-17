using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CooleWebapp.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedFlagToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CoolUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CoolUsers");
        }
    }
}
