using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PodcastCatalog.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contributor",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contributor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContributorRole",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContributorRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ISO = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Podcast",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    LanguageId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Podcast", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Podcast_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Podcast_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Season",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ProductionYear = table.Column<DateOnly>(type: "date", nullable: false),
                    PodcastId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Season", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Season_Podcast_PodcastId",
                        column: x => x.PodcastId,
                        principalTable: "Podcast",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Episode",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Duration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    AiringDate = table.Column<DateOnly>(type: "date", nullable: false),
                    TagId = table.Column<Guid>(type: "uuid", nullable: false),
                    SeasonId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Episode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Episode_Season_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Season",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Episode_Tag_TagId",
                        column: x => x.TagId,
                        principalTable: "Tag",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Contribution",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContributorId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContributorRoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    PodcastId = table.Column<Guid>(type: "uuid", nullable: true),
                    EpisodeId = table.Column<Guid>(type: "uuid", nullable: true),
                    HasRights = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contribution", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contribution_ContributorRole_ContributorRoleId",
                        column: x => x.ContributorRoleId,
                        principalTable: "ContributorRole",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Contribution_Contributor_ContributorId",
                        column: x => x.ContributorId,
                        principalTable: "Contributor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contribution_Episode_EpisodeId",
                        column: x => x.EpisodeId,
                        principalTable: "Episode",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Contribution_Podcast_PodcastId",
                        column: x => x.PodcastId,
                        principalTable: "Podcast",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contribution_ContributorId",
                table: "Contribution",
                column: "ContributorId");

            migrationBuilder.CreateIndex(
                name: "IX_Contribution_ContributorRoleId",
                table: "Contribution",
                column: "ContributorRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Contribution_EpisodeId",
                table: "Contribution",
                column: "EpisodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Contribution_PodcastId",
                table: "Contribution",
                column: "PodcastId");

            migrationBuilder.CreateIndex(
                name: "IX_Episode_SeasonId",
                table: "Episode",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Episode_TagId",
                table: "Episode",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Podcast_CategoryId",
                table: "Podcast",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Podcast_LanguageId",
                table: "Podcast",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Season_PodcastId",
                table: "Season",
                column: "PodcastId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contribution");

            migrationBuilder.DropTable(
                name: "ContributorRole");

            migrationBuilder.DropTable(
                name: "Contributor");

            migrationBuilder.DropTable(
                name: "Episode");

            migrationBuilder.DropTable(
                name: "Season");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "Podcast");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Language");
        }
    }
}
