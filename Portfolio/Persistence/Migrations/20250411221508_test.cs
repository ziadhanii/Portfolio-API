using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Certificates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Issuer = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CertificateUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectAnalytics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    ViewsCount = table.Column<int>(type: "int", nullable: false),
                    VisitsCount = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectAnalytics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectAnalytics_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WebsiteAnalytics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PageTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ViewsCount = table.Column<int>(type: "int", nullable: false),
                    UniqueVisitsCount = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebsiteAnalytics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectVisits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectAnalyticsId = table.Column<int>(type: "int", nullable: false),
                    VisitDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Referrer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectVisits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectVisits_ProjectAnalytics_ProjectAnalyticsId",
                        column: x => x.ProjectAnalyticsId,
                        principalTable: "ProjectAnalytics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WebsiteVisits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PageTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VisitDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Referrer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeSpent = table.Column<int>(type: "int", nullable: true),
                    WebsiteAnalyticsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebsiteVisits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebsiteVisits_WebsiteAnalytics_WebsiteAnalyticsId",
                        column: x => x.WebsiteAnalyticsId,
                        principalTable: "WebsiteAnalytics",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectAnalytics_ProjectId",
                table: "ProjectAnalytics",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectVisits_ProjectAnalyticsId",
                table: "ProjectVisits",
                column: "ProjectAnalyticsId");

            migrationBuilder.CreateIndex(
                name: "IX_WebsiteVisits_WebsiteAnalyticsId",
                table: "WebsiteVisits",
                column: "WebsiteAnalyticsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Certificates");

            migrationBuilder.DropTable(
                name: "ProjectVisits");

            migrationBuilder.DropTable(
                name: "WebsiteVisits");

            migrationBuilder.DropTable(
                name: "ProjectAnalytics");

            migrationBuilder.DropTable(
                name: "WebsiteAnalytics");
        }
    }
}
