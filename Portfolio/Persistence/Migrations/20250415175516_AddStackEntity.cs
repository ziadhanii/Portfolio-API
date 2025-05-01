using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStackEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StackId",
                table: "Skills",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Stacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stacks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Skills_StackId",
                table: "Skills",
                column: "StackId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Stacks_StackId",
                table: "Skills",
                column: "StackId",
                principalTable: "Stacks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Stacks_StackId",
                table: "Skills");

            migrationBuilder.DropTable(
                name: "Stacks");

            migrationBuilder.DropIndex(
                name: "IX_Skills_StackId",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "StackId",
                table: "Skills");
        }
    }
}
