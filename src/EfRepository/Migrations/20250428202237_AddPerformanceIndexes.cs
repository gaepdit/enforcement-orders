using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Enfo.EfRepository.Migrations
{
    /// <inheritdoc />
    public partial class AddPerformanceIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "missing_index_1099_1098",
                table: "EnforcementOrders",
                columns: new[] { "Deleted", "LegalAuthorityId", "PublicationStatus" })
                .Annotation("SqlServer:Include", new[] { "ExecutedDate", "ExecutedOrderPostedDate", "IsExecutedOrder", "IsProposedOrder", "ProposedOrderPostedDate" });

            migrationBuilder.CreateIndex(
                name: "missing_index_1102_1101",
                table: "EnforcementOrders",
                columns: new[] { "Deleted", "LegalAuthorityId", "PublicationStatus" })
                .Annotation("SqlServer:Include", new[] { "ExecutedDate", "ExecutedOrderPostedDate", "FacilityName", "IsExecutedOrder", "IsProposedOrder", "ProposedOrderPostedDate" });

            migrationBuilder.CreateIndex(
                name: "missing_index_1176_1175",
                table: "EnforcementOrders",
                columns: new[] { "Deleted", "PublicationStatus", "County" })
                .Annotation("SqlServer:Include", new[] { "ExecutedDate", "ExecutedOrderPostedDate", "IsExecutedOrder", "IsProposedOrder", "ProposedOrderPostedDate" });

            migrationBuilder.CreateIndex(
                name: "missing_index_1271_1270",
                table: "EnforcementOrders",
                columns: new[] { "Deleted", "PublicationStatus", "County" })
                .Annotation("SqlServer:Include", new[] { "ExecutedOrderPostedDate", "IsExecutedOrder", "IsProposedOrder", "ProposedOrderPostedDate" });

            migrationBuilder.CreateIndex(
                name: "missing_index_345_344",
                table: "EnforcementOrders",
                columns: new[] { "Deleted", "OrderNumber" });

            migrationBuilder.CreateIndex(
                name: "missing_index_561_560",
                table: "EnforcementOrders",
                column: "Deleted")
                .Annotation("SqlServer:Include", new[] { "OrderNumber" });

            migrationBuilder.CreateIndex(
                name: "missing_index_617_616",
                table: "EnforcementOrders",
                columns: new[] { "Deleted", "OrderNumber", "Id" });

            migrationBuilder.CreateIndex(
                name: "missing_index_699_698",
                table: "EnforcementOrders",
                columns: new[] { "Deleted", "PublicationStatus" })
                .Annotation("SqlServer:Include", new[] { "ExecutedOrderPostedDate", "IsExecutedOrder", "IsProposedOrder", "OrderNumber", "ProposedOrderPostedDate" });

            migrationBuilder.CreateIndex(
                name: "missing_index_737_736",
                table: "EnforcementOrders",
                columns: new[] { "Deleted", "LegalAuthorityId", "PublicationStatus" })
                .Annotation("SqlServer:Include", new[] { "ExecutedOrderPostedDate", "IsExecutedOrder", "IsProposedOrder", "ProposedOrderPostedDate" });

            migrationBuilder.CreateIndex(
                name: "missing_index_741_740",
                table: "EnforcementOrders",
                columns: new[] { "Deleted", "PublicationStatus" })
                .Annotation("SqlServer:Include", new[] { "ExecutedOrderPostedDate", "IsExecutedOrder", "IsProposedOrder", "ProposedOrderPostedDate" });

            migrationBuilder.CreateIndex(
                name: "missing_index_898_897",
                table: "EnforcementOrders",
                columns: new[] { "Deleted", "LegalAuthorityId", "PublicationStatus", "County" })
                .Annotation("SqlServer:Include", new[] { "ExecutedOrderPostedDate", "IsExecutedOrder", "IsProposedOrder", "ProposedOrderPostedDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "missing_index_1099_1098",
                table: "EnforcementOrders");

            migrationBuilder.DropIndex(
                name: "missing_index_1102_1101",
                table: "EnforcementOrders");

            migrationBuilder.DropIndex(
                name: "missing_index_1176_1175",
                table: "EnforcementOrders");

            migrationBuilder.DropIndex(
                name: "missing_index_1271_1270",
                table: "EnforcementOrders");

            migrationBuilder.DropIndex(
                name: "missing_index_345_344",
                table: "EnforcementOrders");

            migrationBuilder.DropIndex(
                name: "missing_index_561_560",
                table: "EnforcementOrders");

            migrationBuilder.DropIndex(
                name: "missing_index_617_616",
                table: "EnforcementOrders");

            migrationBuilder.DropIndex(
                name: "missing_index_699_698",
                table: "EnforcementOrders");

            migrationBuilder.DropIndex(
                name: "missing_index_737_736",
                table: "EnforcementOrders");

            migrationBuilder.DropIndex(
                name: "missing_index_741_740",
                table: "EnforcementOrders");

            migrationBuilder.DropIndex(
                name: "missing_index_898_897",
                table: "EnforcementOrders");
        }
    }
}
