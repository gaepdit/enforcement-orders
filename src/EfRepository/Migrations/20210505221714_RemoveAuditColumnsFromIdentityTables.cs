using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Enfo.EfRepository.Migrations
{
    public partial class RemoveAuditColumnsFromIdentityTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AppUserTokens");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AppUserTokens");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AppUserTokens");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AppUserTokens");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AppUserRoles");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AppUserRoles");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AppUserRoles");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AppUserRoles");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AppUserLogins");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AppUserLogins");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AppUserLogins");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AppUserLogins");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AppUserClaims");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AppUserClaims");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AppUserClaims");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AppUserClaims");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AppRoles");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AppRoles");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AppRoles");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AppRoles");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AppRoleClaims");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AppRoleClaims");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AppRoleClaims");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AppRoleClaims");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "AppUserTokens",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AppUserTokens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "AppUserTokens",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AppUserTokens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "AppUserRoles",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AppUserRoles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "AppUserRoles",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AppUserRoles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "AppUserLogins",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AppUserLogins",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "AppUserLogins",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AppUserLogins",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "AppUserClaims",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AppUserClaims",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "AppUserClaims",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AppUserClaims",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "AppRoles",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AppRoles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "AppRoles",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AppRoles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "AppRoleClaims",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AppRoleClaims",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "AppRoleClaims",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AppRoleClaims",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
