using Microsoft.EntityFrameworkCore.Migrations;

namespace PluralVideos.Data.Migrations
{
    public partial class AddedIsLocalToUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLocal",
                table: "User",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLocal",
                table: "User");
        }
    }
}
