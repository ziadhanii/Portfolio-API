using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStackToSkill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Stacks_StackId",
                table: "Skills");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Stacks_StackId",
                table: "Skills",
                column: "StackId",
                principalTable: "Stacks",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Stacks_StackId",
                table: "Skills");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Stacks_StackId",
                table: "Skills",
                column: "StackId",
                principalTable: "Stacks",
                principalColumn: "Id");
        }
    }
}
