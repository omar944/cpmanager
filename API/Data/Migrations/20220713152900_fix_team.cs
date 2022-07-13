using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class fix_team : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participations_Teams_TeamId",
                table: "Participations");

            migrationBuilder.DropIndex(
                name: "IX_Participations_TeamId",
                table: "Participations");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Participations");

            migrationBuilder.AddColumn<string>(
                name: "TeamName",
                table: "Participations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamName",
                table: "Participations");

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "Participations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Participations_TeamId",
                table: "Participations",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Participations_Teams_TeamId",
                table: "Participations",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
