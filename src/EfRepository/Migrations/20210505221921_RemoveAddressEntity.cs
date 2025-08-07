using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Enfo.EfRepository.Migrations
{
    [SuppressMessage("ReSharper", "S125")]
    public partial class RemoveAddressEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EpdContacts_Addresses_AddressId",
                table: "EpdContacts");

            migrationBuilder.DropIndex(
                name: "IX_EpdContacts_AddressId",
                table: "EpdContacts");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "EpdContacts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "EpdContacts",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "EpdContacts",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "EpdContacts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Street2",
                table: "EpdContacts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.Sql(@"update c
                set c.City = a.City, c.PostalCode = a.PostalCode, c.State = a.State, 
                    c.Street = a.Street, c.Street2 = a.Street2
                from EpdContacts c inner join Addresses a on c.AddressId = a.Id");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "EpdContacts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "EpdContacts");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "EpdContacts");

            migrationBuilder.DropColumn(
                name: "State",
                table: "EpdContacts");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "EpdContacts");

            migrationBuilder.DropColumn(
                name: "Street2",
                table: "EpdContacts");

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "EpdContacts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    State = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Street = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Street2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EpdContacts_AddressId",
                table: "EpdContacts",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_EpdContacts_Addresses_AddressId",
                table: "EpdContacts",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
