using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Enfo.EfRepository.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Counties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CountyName = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Counties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnfoUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    EmailAddress = table.Column<string>(maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastLoggedInDate = table.Column<DateTime>(nullable: true),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    LastPasswordChangedDate = table.Column<DateTime>(nullable: true),
                    PasswordHash = table.Column<string>(maxLength: 100, nullable: true),
                    RequirePasswordChange = table.Column<bool>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnfoUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    City = table.Column<string>(maxLength: 50, nullable: false),
                    CreatedById = table.Column<Guid>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    PostalCode = table.Column<string>(maxLength: 10, nullable: true),
                    State = table.Column<string>(maxLength: 2, nullable: true),
                    Street = table.Column<string>(maxLength: 100, nullable: false),
                    Street2 = table.Column<string>(maxLength: 100, nullable: true),
                    UpdatedById = table.Column<Guid>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_EnfoUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "EnfoUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Addresses_EnfoUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "EnfoUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LegalAuthorities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    AuthorityName = table.Column<string>(maxLength: 100, nullable: false),
                    CreatedById = table.Column<Guid>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    OrderNumberTemplate = table.Column<string>(maxLength: 40, nullable: true),
                    UpdatedById = table.Column<Guid>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalAuthorities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LegalAuthorities_EnfoUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "EnfoUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LegalAuthorities_EnfoUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "EnfoUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EpdContacts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    AddressId = table.Column<int>(nullable: false),
                    ContactName = table.Column<string>(maxLength: 50, nullable: false),
                    CreatedById = table.Column<Guid>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    Organization = table.Column<string>(maxLength: 100, nullable: false),
                    Telephone = table.Column<string>(maxLength: 50, nullable: true),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    UpdatedById = table.Column<Guid>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
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
                    table.ForeignKey(
                        name: "FK_EpdContacts_EnfoUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "EnfoUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EpdContacts_EnfoUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "EnfoUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EnforcementOrders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Cause = table.Column<string>(maxLength: 3990, nullable: true),
                    CommentContactId = table.Column<int>(nullable: true),
                    CommentPeriodClosesDate = table.Column<DateTime>(nullable: true),
                    County = table.Column<string>(maxLength: 25, nullable: false),
                    CreatedById = table.Column<Guid>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    ExecutedDate = table.Column<DateTime>(nullable: true),
                    ExecutedOrderPostedDate = table.Column<DateTime>(nullable: true),
                    FacilityName = table.Column<string>(maxLength: 205, nullable: false),
                    HearingCommentPeriodClosesDate = table.Column<DateTime>(nullable: true),
                    HearingContactId = table.Column<int>(nullable: true),
                    HearingDate = table.Column<DateTime>(nullable: true),
                    HearingLocation = table.Column<string>(maxLength: 3990, nullable: true),
                    IsExecutedOrder = table.Column<bool>(nullable: false),
                    IsHearingScheduled = table.Column<bool>(nullable: false),
                    IsProposedOrder = table.Column<bool>(nullable: false),
                    LegalAuthorityId = table.Column<int>(nullable: false),
                    OrderNumber = table.Column<string>(maxLength: 50, nullable: false),
                    ProposedOrderPostedDate = table.Column<DateTime>(nullable: true),
                    PublicationStatus = table.Column<int>(nullable: false),
                    Requirements = table.Column<string>(maxLength: 3990, nullable: true),
                    SettlementAmount = table.Column<decimal>(type: "money", nullable: true),
                    UpdatedById = table.Column<Guid>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
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
                        name: "FK_EnforcementOrders_EnfoUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "EnfoUsers",
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
                    table.ForeignKey(
                        name: "FK_EnforcementOrders_EnfoUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "EnfoUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CreatedById",
                table: "Addresses",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UpdatedById",
                table: "Addresses",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_EnforcementOrders_CommentContactId",
                table: "EnforcementOrders",
                column: "CommentContactId");

            migrationBuilder.CreateIndex(
                name: "IX_EnforcementOrders_CreatedById",
                table: "EnforcementOrders",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_EnforcementOrders_HearingContactId",
                table: "EnforcementOrders",
                column: "HearingContactId");

            migrationBuilder.CreateIndex(
                name: "IX_EnforcementOrders_LegalAuthorityId",
                table: "EnforcementOrders",
                column: "LegalAuthorityId");

            migrationBuilder.CreateIndex(
                name: "IX_EnforcementOrders_UpdatedById",
                table: "EnforcementOrders",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_EpdContacts_AddressId",
                table: "EpdContacts",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_EpdContacts_CreatedById",
                table: "EpdContacts",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_EpdContacts_UpdatedById",
                table: "EpdContacts",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LegalAuthorities_CreatedById",
                table: "LegalAuthorities",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LegalAuthorities_UpdatedById",
                table: "LegalAuthorities",
                column: "UpdatedById");
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

            migrationBuilder.DropTable(
                name: "EnfoUsers");
        }
    }
}
