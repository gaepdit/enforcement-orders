using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Enfo.EfRepository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserAuditing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ObjectId",
                table: "AppUsers");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "AccountCreatedAt",
                table: "AppUsers",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "AccountUpdatedAt",
                table: "AppUsers",
                type: "datetimeoffset",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountCreatedAt",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "AccountUpdatedAt",
                table: "AppUsers");

            migrationBuilder.AddColumn<string>(
                name: "ObjectId",
                table: "AppUsers",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: true);
        }
    }
}
