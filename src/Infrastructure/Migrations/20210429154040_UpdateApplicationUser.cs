using Microsoft.EntityFrameworkCore.Migrations;

namespace Enfo.Infrastructure.Migrations
{
    public partial class UpdateApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "AppUsers");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "AppUsers",
                newName: "GivenName");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "AppUsers",
                newName: "FamilyName");

            migrationBuilder.AddColumn<string>(
                name: "ObjectId",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubjectId",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ObjectId",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "AppUsers");

            migrationBuilder.RenameColumn(
                name: "GivenName",
                table: "AppUsers",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "FamilyName",
                table: "AppUsers",
                newName: "LastName");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "AppUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
