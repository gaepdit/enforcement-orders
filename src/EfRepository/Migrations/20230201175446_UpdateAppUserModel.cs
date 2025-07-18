using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Enfo.EfRepository.Migrations
{
    public partial class UpdateAppUserModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "AppUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubjectId",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
