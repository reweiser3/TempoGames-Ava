using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ava.Migrations
{
    /// <inheritdoc />
    public partial class AddOpponents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OpponentId",
                table: "Games",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Games_OpponentId",
                table: "Games",
                column: "OpponentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_AspNetUsers_OpponentId",
                table: "Games",
                column: "OpponentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_AspNetUsers_OpponentId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_OpponentId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "OpponentId",
                table: "Games");
        }
    }
}