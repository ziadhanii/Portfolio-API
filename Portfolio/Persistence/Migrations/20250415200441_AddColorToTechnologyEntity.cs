using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Portfolio.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddColorToTechnologyEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Technologies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Technologies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Technologies",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Technologies",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Technologies",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Technologies",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Technologies",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Technologies",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Technologies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Technologies");

            migrationBuilder.InsertData(
                table: "Technologies",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "C#" },
                    { 2, "ASP.NET Core" },
                    { 3, "Entity Framework Core" },
                    { 4, "Angular" },
                    { 5, "React" },
                    { 6, "Docker" },
                    { 7, "Azure" },
                    { 8, "SQL Server" }
                });
        }
    }
}
