using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Enfo.API.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UpdatedDate = table.Column<DateTimeOffset>(nullable: true),
                    UpdatedById = table.Column<Guid>(nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: true),
                    CreatedById = table.Column<Guid>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    Street = table.Column<string>(maxLength: 100, nullable: false),
                    Street2 = table.Column<string>(maxLength: 100, nullable: true),
                    City = table.Column<string>(maxLength: 50, nullable: false),
                    State = table.Column<string>(maxLength: 2, nullable: true),
                    PostalCode = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Counties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UpdatedDate = table.Column<DateTimeOffset>(nullable: true),
                    UpdatedById = table.Column<Guid>(nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: true),
                    CreatedById = table.Column<Guid>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CountyName = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Counties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LegalAuthorities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UpdatedDate = table.Column<DateTimeOffset>(nullable: true),
                    UpdatedById = table.Column<Guid>(nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: true),
                    CreatedById = table.Column<Guid>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    AuthorityName = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalAuthorities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EpdContacts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UpdatedDate = table.Column<DateTimeOffset>(nullable: true),
                    UpdatedById = table.Column<Guid>(nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: true),
                    CreatedById = table.Column<Guid>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    ContactName = table.Column<string>(maxLength: 50, nullable: false),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    Organization = table.Column<string>(maxLength: 100, nullable: false),
                    AddressId = table.Column<int>(nullable: false),
                    Telephone = table.Column<string>(maxLength: 50, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EpdContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EpdContacts_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnforcementOrders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UpdatedDate = table.Column<DateTimeOffset>(nullable: true),
                    UpdatedById = table.Column<Guid>(nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: true),
                    CreatedById = table.Column<Guid>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    FacilityName = table.Column<string>(maxLength: 205, nullable: false),
                    County = table.Column<string>(maxLength: 25, nullable: false),
                    LegalAuthorityId = table.Column<int>(nullable: false),
                    Cause = table.Column<string>(maxLength: 3990, nullable: true),
                    Requirements = table.Column<string>(maxLength: 3990, nullable: true),
                    SettlementAmount = table.Column<decimal>(type: "money", nullable: true),
                    Deleted = table.Column<bool>(nullable: false),
                    PublicationStatus = table.Column<int>(nullable: false),
                    OrderNumber = table.Column<string>(maxLength: 50, nullable: false),
                    IsProposedOrder = table.Column<bool>(nullable: false),
                    CommentPeriodClosesDate = table.Column<DateTime>(type: "Date", nullable: true),
                    CommentContactId = table.Column<int>(nullable: true),
                    ProposedOrderPostedDate = table.Column<DateTime>(type: "Date", nullable: true),
                    IsExecutedOrder = table.Column<bool>(nullable: false),
                    ExecutedDate = table.Column<DateTime>(type: "Date", nullable: true),
                    ExecutedOrderPostedDate = table.Column<DateTime>(type: "Date", nullable: true),
                    IsHearingScheduled = table.Column<bool>(nullable: false),
                    HearingDate = table.Column<DateTime>(type: "Date", nullable: true),
                    HearingLocation = table.Column<string>(maxLength: 3990, nullable: true),
                    HearingCommentPeriodClosesDate = table.Column<DateTime>(type: "Date", nullable: true),
                    HearingContactId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnforcementOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnforcementOrders_EpdContacts_CommentContactId",
                        column: x => x.CommentContactId,
                        principalTable: "EpdContacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EnforcementOrders_EpdContacts_HearingContactId",
                        column: x => x.HearingContactId,
                        principalTable: "EpdContacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EnforcementOrders_LegalAuthorities_LegalAuthorityId",
                        column: x => x.LegalAuthorityId,
                        principalTable: "LegalAuthorities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "Active", "City", "CreatedById", "CreatedDate", "PostalCode", "State", "Street", "Street2", "UpdatedById", "UpdatedDate" },
                values: new object[] { 2000, true, "Atlanta", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 13, 3, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), "30354", "GA", "4244 International Parkway", "Suite 120", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 13, 3, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "Active", "City", "CreatedById", "CreatedDate", "PostalCode", "State", "Street", "Street2", "UpdatedById", "UpdatedDate" },
                values: new object[] { 2016, true, "Atlanta", new Guid("c076cda6-8344-4bde-8a3a-2c96dc4de932"), new DateTimeOffset(new DateTime(2017, 5, 3, 14, 48, 43, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), "30334", "GA", "2 Martin Luther King Jr. Drive SE", "Suite 1456 East", new Guid("c076cda6-8344-4bde-8a3a-2c96dc4de932"), new DateTimeOffset(new DateTime(2017, 5, 3, 14, 48, 43, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "Active", "City", "CreatedById", "CreatedDate", "PostalCode", "State", "Street", "Street2", "UpdatedById", "UpdatedDate" },
                values: new object[] { 2015, true, "Atlanta", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 13, 3, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), "30354-3906", "GA", "4244 International Pkwy", "Suite 120", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 13, 3, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "Active", "City", "CreatedById", "CreatedDate", "PostalCode", "State", "Street", "Street2", "UpdatedById", "UpdatedDate" },
                values: new object[] { 2014, false, "Augusta", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 13, 3, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), "30909", "GA", "3525 Walton Way Ext.", "", new Guid("c076cda6-8344-4bde-8a3a-2c96dc4de932"), new DateTimeOffset(new DateTime(2017, 5, 3, 14, 44, 45, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "Active", "City", "CreatedById", "CreatedDate", "PostalCode", "State", "Street", "Street2", "UpdatedById", "UpdatedDate" },
                values: new object[] { 2012, false, "Augusta", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 13, 3, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), "30906", "GA", "1885 Tobacco Road", "", new Guid("c076cda6-8344-4bde-8a3a-2c96dc4de932"), new DateTimeOffset(new DateTime(2017, 5, 3, 14, 44, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "Active", "City", "CreatedById", "CreatedDate", "PostalCode", "State", "Street", "Street2", "UpdatedById", "UpdatedDate" },
                values: new object[] { 2011, false, "Athens", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 13, 3, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), "30605", "GA", "745 Gaines School Road", "", new Guid("c076cda6-8344-4bde-8a3a-2c96dc4de932"), new DateTimeOffset(new DateTime(2017, 5, 3, 14, 44, 39, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "Active", "City", "CreatedById", "CreatedDate", "PostalCode", "State", "Street", "Street2", "UpdatedById", "UpdatedDate" },
                values: new object[] { 2010, false, "Cartersville", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 13, 3, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), "30120", "GA", "Post Office Box 3250", "", new Guid("c076cda6-8344-4bde-8a3a-2c96dc4de932"), new DateTimeOffset(new DateTime(2017, 5, 3, 14, 45, 2, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "Active", "City", "CreatedById", "CreatedDate", "PostalCode", "State", "Street", "Street2", "UpdatedById", "UpdatedDate" },
                values: new object[] { 2013, false, "Cartersville", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 13, 3, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), "30120", "GA", "Post Office Box 3250", "", new Guid("c076cda6-8344-4bde-8a3a-2c96dc4de932"), new DateTimeOffset(new DateTime(2017, 5, 3, 14, 44, 50, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "Active", "City", "CreatedById", "CreatedDate", "PostalCode", "State", "Street", "Street2", "UpdatedById", "UpdatedDate" },
                values: new object[] { 2008, false, "Brunswick", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 13, 3, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), "31523", "GA", "400 Commerce Center Drive", "", new Guid("c076cda6-8344-4bde-8a3a-2c96dc4de932"), new DateTimeOffset(new DateTime(2017, 5, 3, 14, 45, 12, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "Active", "City", "CreatedById", "CreatedDate", "PostalCode", "State", "Street", "Street2", "UpdatedById", "UpdatedDate" },
                values: new object[] { 2007, false, "Albany", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 13, 3, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), "31701-3576", "GA", "2024 Newton Road", "", new Guid("c076cda6-8344-4bde-8a3a-2c96dc4de932"), new DateTimeOffset(new DateTime(2017, 5, 3, 14, 45, 18, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "Active", "City", "CreatedById", "CreatedDate", "PostalCode", "State", "Street", "Street2", "UpdatedById", "UpdatedDate" },
                values: new object[] { 2004, true, "Atlanta", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 13, 3, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), "30334", "GA", "2 Martin Luther King Jr. Drive SE", "Suite 1152 East", new Guid("c076cda6-8344-4bde-8a3a-2c96dc4de932"), new DateTimeOffset(new DateTime(2017, 5, 3, 14, 48, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "Active", "City", "CreatedById", "CreatedDate", "PostalCode", "State", "Street", "Street2", "UpdatedById", "UpdatedDate" },
                values: new object[] { 2003, false, "Unknown", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 13, 3, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), "00000", "GA", "Unknown", "", new Guid("c076cda6-8344-4bde-8a3a-2c96dc4de932"), new DateTimeOffset(new DateTime(2017, 5, 3, 14, 45, 37, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "Active", "City", "CreatedById", "CreatedDate", "PostalCode", "State", "Street", "Street2", "UpdatedById", "UpdatedDate" },
                values: new object[] { 2002, true, "Atlanta", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 13, 3, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), "30334", "GA", "2 Martin Luther King Jr. Drive SE", "Suite 1054 East", new Guid("c076cda6-8344-4bde-8a3a-2c96dc4de932"), new DateTimeOffset(new DateTime(2017, 5, 3, 14, 47, 33, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "Active", "City", "CreatedById", "CreatedDate", "PostalCode", "State", "Street", "Street2", "UpdatedById", "UpdatedDate" },
                values: new object[] { 2001, false, "Atlanta", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 13, 3, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), "30354", "GA", "4244 International Parkway", "Suite 104", new Guid("c076cda6-8344-4bde-8a3a-2c96dc4de932"), new DateTimeOffset(new DateTime(2017, 5, 3, 14, 35, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "Active", "City", "CreatedById", "CreatedDate", "PostalCode", "State", "Street", "Street2", "UpdatedById", "UpdatedDate" },
                values: new object[] { 2009, false, "Macon", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 13, 3, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), "31211", "GA", "2640 Shurling Drive", "", new Guid("c076cda6-8344-4bde-8a3a-2c96dc4de932"), new DateTimeOffset(new DateTime(2017, 5, 3, 14, 45, 7, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 109, "Oglethorpe", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 103, "Montgomery", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 104, "Morgan", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 105, "Murray", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 106, "Muscogee", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 107, "Newton", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 108, "Oconee", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 110, "Paulding", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 117, "Putnam", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 112, "Pickens", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 113, "Pierce", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 114, "Pike", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 115, "Polk", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 116, "Pulaski", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 102, "Monroe", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 118, "Quitman", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 111, "Peach", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 101, "Mitchell", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 94, "Macon", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 99, "Meriwether", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 82, "Jenkins", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 84, "Jones", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 85, "Lamar", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 86, "Lanier", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 87, "Laurens", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 88, "Lee", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 89, "Liberty", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 100, "Miller", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 90, "Lincoln", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 92, "Lowndes", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 93, "Lumpkin", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 119, "Rabun", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 95, "Madison", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 96, "Marion", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 97, "McDuffie", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 98, "McIntosh", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 91, "Long", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 120, "Randolph", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 128, "Stewart", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 122, "Rockdale", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 144, "Union", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 145, "Upson", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 146, "Walker", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 147, "Walton", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 148, "Ware", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 149, "Warren", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 150, "Washington", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 143, "Twiggs", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 151, "Wayne", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 153, "Wheeler", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 154, "White", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 155, "Whitfield", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 156, "Wilcox", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 157, "Wilkes", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 158, "Wilkinson", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 159, "Worth", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 152, "Webster", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 142, "Turner", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 141, "Troup", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 140, "Treutlen", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 123, "Schley", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 124, "Screven", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 125, "Seminole", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 126, "Spalding", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 127, "Stephens", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 81, "Jefferson", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 129, "Sumter", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 130, "Talbot", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 131, "Taliaferro", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 132, "Tattnall", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 133, "Taylor", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 134, "Telfair", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 135, "Terrell", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 136, "Thomas", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 137, "Tift", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 138, "Toombs", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 139, "Towns", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 121, "Richmond", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 80, "Jeff Davis", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 83, "Johnson", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 78, "Jackson", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 22, "Carroll", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 23, "Catoosa", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 24, "Charlton", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 25, "Chatham", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 26, "Chattahoochee", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 27, "Chattooga", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 28, "Cherokee", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 21, "Candler", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 79, "Jasper", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 31, "Clayton", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 32, "Clinch", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 33, "Cobb", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 34, "Coffee", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 35, "Colquitt", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 36, "Columbia", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 37, "Cook", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 30, "Clay", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 20, "Camden", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 19, "Calhoun", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 18, "Butts", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 1, "Appling", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 2, "Atkinson", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 3, "Bacon", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 4, "Baker", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 5, "Baldwin", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 6, "Banks", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 7, "Barrow", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 8, "Bartow", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 9, "Ben Hill", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 10, "Berrien", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 11, "Bibb", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 12, "Bleckley", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 13, "Brantley", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 14, "Brooks", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 15, "Bryan", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 16, "Bulloch", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 17, "Burke", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 38, "Coweta", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 39, "Crawford", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 29, "Clarke", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 41, "Dade", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 62, "Glascock", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 63, "Glynn", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 64, "Gordon", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 65, "Grady", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 66, "Greene", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 67, "Gwinnett", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 68, "Habersham", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 61, "Gilmer", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 69, "Hall", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 71, "Haralson", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 72, "Harris", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 73, "Hart", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 74, "Heard", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 75, "Henry", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 76, "Houston", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 77, "Irwin", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 40, "Crisp", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 60, "Fulton", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 70, "Hancock", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 58, "Forsyth", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 59, "Franklin", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 42, "Dawson", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 43, "Decatur", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 44, "DeKalb", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 46, "Dooly", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 47, "Dougherty", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 48, "Douglas", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 49, "Early", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 45, "Dodge", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 51, "Effingham", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 52, "Elbert", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 53, "Emanuel", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 54, "Evans", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 55, "Fannin", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 56, "Fayette", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 50, "Echols", null, null, null, null });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "Id", "CountyName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 57, "Floyd", null, null, null, null });

            migrationBuilder.InsertData(
                table: "LegalAuthorities",
                columns: new[] { "Id", "Active", "AuthorityName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 13, true, "Safe Drinking Water Act", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "LegalAuthorities",
                columns: new[] { "Id", "Active", "AuthorityName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 14, true, "Groundwater Use Act", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "LegalAuthorities",
                columns: new[] { "Id", "Active", "AuthorityName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 15, true, "Oil and Gas and Deep Drilling Act", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "LegalAuthorities",
                columns: new[] { "Id", "Active", "AuthorityName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 16, true, "Radiation Control Act", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "LegalAuthorities",
                columns: new[] { "Id", "Active", "AuthorityName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 19, true, "Lead Poisoning Prevention Act", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "LegalAuthorities",
                columns: new[] { "Id", "Active", "AuthorityName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 18, true, "Georgia Environmental Policy Act", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "LegalAuthorities",
                columns: new[] { "Id", "Active", "AuthorityName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 20, true, "Water Well Standards Act", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "LegalAuthorities",
                columns: new[] { "Id", "Active", "AuthorityName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 12, true, "Safe Dams Act", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "LegalAuthorities",
                columns: new[] { "Id", "Active", "AuthorityName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 17, true, "Oil or Hazardous Materials Spills or Releases Act", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "LegalAuthorities",
                columns: new[] { "Id", "Active", "AuthorityName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 11, true, "Surface Mining Act", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "LegalAuthorities",
                columns: new[] { "Id", "Active", "AuthorityName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 2, true, "Asbestos Safety Act", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "LegalAuthorities",
                columns: new[] { "Id", "Active", "AuthorityName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 9, true, "River Basin Management Planning Act", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "LegalAuthorities",
                columns: new[] { "Id", "Active", "AuthorityName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 8, true, "Water Quality Control Act (including Surface Water Allocation)", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "LegalAuthorities",
                columns: new[] { "Id", "Active", "AuthorityName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 7, true, "Comprehensive Solid Waste Management Act", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "LegalAuthorities",
                columns: new[] { "Id", "Active", "AuthorityName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 6, true, "Underground Storage Tank Act", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "LegalAuthorities",
                columns: new[] { "Id", "Active", "AuthorityName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 5, true, "Hazardous Site Response Act", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "LegalAuthorities",
                columns: new[] { "Id", "Active", "AuthorityName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 4, true, "Hazardous Waste Management Act", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "LegalAuthorities",
                columns: new[] { "Id", "Active", "AuthorityName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 3, true, "Motor Vehicle Inspection and Maintenance Act", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "LegalAuthorities",
                columns: new[] { "Id", "Active", "AuthorityName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 1, true, "Air Quality Act", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2019, 6, 20, 15, 44, 40, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "LegalAuthorities",
                columns: new[] { "Id", "Active", "AuthorityName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 21, false, "Year 2000 Readiness Act", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "LegalAuthorities",
                columns: new[] { "Id", "Active", "AuthorityName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 10, true, "Erosion and Sedimentation Act", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "LegalAuthorities",
                columns: new[] { "Id", "Active", "AuthorityName", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" },
                values: new object[] { 22, true, "Voluntary Remediation Program Act", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 12, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "EpdContacts",
                columns: new[] { "Id", "Active", "AddressId", "ContactName", "CreatedById", "CreatedDate", "Email", "Organization", "Telephone", "Title", "UpdatedById", "UpdatedDate" },
                values: new object[] { 2000, false, 2000, "Mr. Keith M. Bentley", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 13, 7, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), null, "Environmental Protection Division", null, "Chief, Air Protection Branch", new Guid("c076cda6-8344-4bde-8a3a-2c96dc4de932"), new DateTimeOffset(new DateTime(2017, 5, 3, 14, 33, 18, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "EpdContacts",
                columns: new[] { "Id", "Active", "AddressId", "ContactName", "CreatedById", "CreatedDate", "Email", "Organization", "Telephone", "Title", "UpdatedById", "UpdatedDate" },
                values: new object[] { 2001, false, 2001, "Mr. Jeff Cown", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 13, 7, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), null, "Environmental Protection Division", null, "Chief, Land Protection Branch", new Guid("c076cda6-8344-4bde-8a3a-2c96dc4de932"), new DateTimeOffset(new DateTime(2017, 5, 3, 14, 34, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "EpdContacts",
                columns: new[] { "Id", "Active", "AddressId", "ContactName", "CreatedById", "CreatedDate", "Email", "Organization", "Telephone", "Title", "UpdatedById", "UpdatedDate" },
                values: new object[] { 2002, true, 2002, "Mr. Chuck Mueller", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 13, 7, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), "Chuck.mueller@dnr.ga.gov", "Environmental Protection Division", null, "Chief, Land Protection Branch", new Guid("c076cda6-8344-4bde-8a3a-2c96dc4de932"), new DateTimeOffset(new DateTime(2018, 9, 28, 13, 2, 35, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "EpdContacts",
                columns: new[] { "Id", "Active", "AddressId", "ContactName", "CreatedById", "CreatedDate", "Email", "Organization", "Telephone", "Title", "UpdatedById", "UpdatedDate" },
                values: new object[] { 2003, false, 2003, "Unknown", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 13, 7, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), null, "None", null, "None", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 28, 1, 25, 5, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "EpdContacts",
                columns: new[] { "Id", "Active", "AddressId", "ContactName", "CreatedById", "CreatedDate", "Email", "Organization", "Telephone", "Title", "UpdatedById", "UpdatedDate" },
                values: new object[] { 2004, true, 2004, "Mr. James A. Capp", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 13, 7, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), "James.Capp@dnr.ga.gov", "Environmental Protection Division", null, "Chief, Watershed Protection Branch", new Guid("c076cda6-8344-4bde-8a3a-2c96dc4de932"), new DateTimeOffset(new DateTime(2017, 5, 3, 14, 50, 40, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "EpdContacts",
                columns: new[] { "Id", "Active", "AddressId", "ContactName", "CreatedById", "CreatedDate", "Email", "Organization", "Telephone", "Title", "UpdatedById", "UpdatedDate" },
                values: new object[] { 2007, false, 2007, "Ms. Mary Sheffield", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 13, 7, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), null, "Environmental Protection Division", null, "Manager, Southwest District", new Guid("c076cda6-8344-4bde-8a3a-2c96dc4de932"), new DateTimeOffset(new DateTime(2017, 5, 3, 14, 34, 15, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "EpdContacts",
                columns: new[] { "Id", "Active", "AddressId", "ContactName", "CreatedById", "CreatedDate", "Email", "Organization", "Telephone", "Title", "UpdatedById", "UpdatedDate" },
                values: new object[] { 2008, false, 2008, "Mr. Bruce Foisy", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 13, 7, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), null, "Environmental Protection Division", null, "Manager, Coastal District Office", new Guid("c076cda6-8344-4bde-8a3a-2c96dc4de932"), new DateTimeOffset(new DateTime(2017, 5, 3, 14, 34, 22, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "EpdContacts",
                columns: new[] { "Id", "Active", "AddressId", "ContactName", "CreatedById", "CreatedDate", "Email", "Organization", "Telephone", "Title", "UpdatedById", "UpdatedDate" },
                values: new object[] { 2009, false, 2009, "Mr. Todd Bethune", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 13, 7, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), null, "Environmental Protection Division", null, "Manager, West Central District", new Guid("c076cda6-8344-4bde-8a3a-2c96dc4de932"), new DateTimeOffset(new DateTime(2017, 5, 3, 14, 34, 29, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "EpdContacts",
                columns: new[] { "Id", "Active", "AddressId", "ContactName", "CreatedById", "CreatedDate", "Email", "Organization", "Telephone", "Title", "UpdatedById", "UpdatedDate" },
                values: new object[] { 2011, false, 2011, "Mr. Mike Rodock", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 13, 7, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), null, "Environmental Protection Division", null, "Manager, Northeast District", new Guid("c076cda6-8344-4bde-8a3a-2c96dc4de932"), new DateTimeOffset(new DateTime(2017, 5, 3, 14, 34, 36, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "EpdContacts",
                columns: new[] { "Id", "Active", "AddressId", "ContactName", "CreatedById", "CreatedDate", "Email", "Organization", "Telephone", "Title", "UpdatedById", "UpdatedDate" },
                values: new object[] { 2012, false, 2012, "Mr. Don McCarty", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 13, 7, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), null, "Environmental Protection Division", null, "Manager, East Central District", new Guid("c076cda6-8344-4bde-8a3a-2c96dc4de932"), new DateTimeOffset(new DateTime(2017, 5, 3, 14, 34, 47, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "EpdContacts",
                columns: new[] { "Id", "Active", "AddressId", "ContactName", "CreatedById", "CreatedDate", "Email", "Organization", "Telephone", "Title", "UpdatedById", "UpdatedDate" },
                values: new object[] { 2014, false, 2014, "Mr. Jeff Darley", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 13, 7, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), null, "Environmental Protection Division", null, "Manager, East Central District", new Guid("c076cda6-8344-4bde-8a3a-2c96dc4de932"), new DateTimeOffset(new DateTime(2017, 5, 3, 14, 34, 57, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "EpdContacts",
                columns: new[] { "Id", "Active", "AddressId", "ContactName", "CreatedById", "CreatedDate", "Email", "Organization", "Telephone", "Title", "UpdatedById", "UpdatedDate" },
                values: new object[] { 2015, true, 2015, "Ms. Karen Hays", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 13, 7, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), "Karen.Hays@dnr.ga.gov", "Environmental Protection Division", null, "Chief, Air Protection Branch", new Guid("c076cda6-8344-4bde-8a3a-2c96dc4de932"), new DateTimeOffset(new DateTime(2017, 5, 3, 14, 49, 34, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "EpdContacts",
                columns: new[] { "Id", "Active", "AddressId", "ContactName", "CreatedById", "CreatedDate", "Email", "Organization", "Telephone", "Title", "UpdatedById", "UpdatedDate" },
                values: new object[] { 2010, false, 2016, "Dr. Bert Langley", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 13, 7, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), "Bert.Langley@dnr.ga.gov", "Environmental Protection Division", null, "Director Of Compliance", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2018, 3, 6, 8, 55, 1, 0, DateTimeKind.Unspecified), new TimeSpan(0, -5, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "EpdContacts",
                columns: new[] { "Id", "Active", "AddressId", "ContactName", "CreatedById", "CreatedDate", "Email", "Organization", "Telephone", "Title", "UpdatedById", "UpdatedDate" },
                values: new object[] { 2013, true, 2016, "Mr. James Cooley", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2017, 4, 27, 21, 13, 7, 0, DateTimeKind.Unspecified), new TimeSpan(0, -4, 0, 0, 0)), "james.cooley@dnr.ga.gov", "Environmental Protection Division", "(770) 387-4929", "Director of District Operations", new Guid("cecdb2c3-101c-45ef-2f05-08d4881df634"), new DateTimeOffset(new DateTime(2018, 3, 6, 8, 57, 35, 0, DateTimeKind.Unspecified), new TimeSpan(0, -5, 0, 0, 0)) });

            migrationBuilder.CreateIndex(
                name: "IX_EnforcementOrders_CommentContactId",
                table: "EnforcementOrders",
                column: "CommentContactId");

            migrationBuilder.CreateIndex(
                name: "IX_EnforcementOrders_HearingContactId",
                table: "EnforcementOrders",
                column: "HearingContactId");

            migrationBuilder.CreateIndex(
                name: "IX_EnforcementOrders_LegalAuthorityId",
                table: "EnforcementOrders",
                column: "LegalAuthorityId");

            migrationBuilder.CreateIndex(
                name: "IX_EnforcementOrders_OrderNumber",
                table: "EnforcementOrders",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EpdContacts_AddressId",
                table: "EpdContacts",
                column: "AddressId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Counties");

            migrationBuilder.DropTable(
                name: "EnforcementOrders");

            migrationBuilder.DropTable(
                name: "EpdContacts");

            migrationBuilder.DropTable(
                name: "LegalAuthorities");

            migrationBuilder.DropTable(
                name: "Addresses");
        }
    }
}
