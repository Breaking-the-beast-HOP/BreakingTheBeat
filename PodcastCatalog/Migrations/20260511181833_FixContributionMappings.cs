using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PodcastCatalog.Migrations
{
    /// <inheritdoc />
    public partial class FixContributionMappings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contribution_Contributor_ContributorId",
                table: "Contribution");

            migrationBuilder.AddForeignKey(
                name: "FK_Contribution_Contributor_ContributorId",
                table: "Contribution",
                column: "ContributorId",
                principalTable: "Contributor",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contribution_Contributor_ContributorId",
                table: "Contribution");

            migrationBuilder.AddForeignKey(
                name: "FK_Contribution_Contributor_ContributorId",
                table: "Contribution",
                column: "ContributorId",
                principalTable: "Contributor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
