using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCvUrlToUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CvUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CvUrl",
                table: "AspNetUsers");
        }
    }
}
