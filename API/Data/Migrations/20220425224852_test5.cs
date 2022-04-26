using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class test5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participation_AspNetUsers_UserId",
                table: "Participation");

            migrationBuilder.DropForeignKey(
                name: "FK_Participation_Teams_TeamId",
                table: "Participation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Participation",
                table: "Participation");

            migrationBuilder.RenameTable(
                name: "Participation",
                newName: "Participations");

            migrationBuilder.RenameIndex(
                name: "IX_Participation_UserId",
                table: "Participations",
                newName: "IX_Participations_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Participation_TeamId",
                table: "Participations",
                newName: "IX_Participations_TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Participations",
                table: "Participations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Participations_AspNetUsers_UserId",
                table: "Participations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Participations_Teams_TeamId",
                table: "Participations",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participations_AspNetUsers_UserId",
                table: "Participations");

            migrationBuilder.DropForeignKey(
                name: "FK_Participations_Teams_TeamId",
                table: "Participations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Participations",
                table: "Participations");

            migrationBuilder.RenameTable(
                name: "Participations",
                newName: "Participation");

            migrationBuilder.RenameIndex(
                name: "IX_Participations_UserId",
                table: "Participation",
                newName: "IX_Participation_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Participations_TeamId",
                table: "Participation",
                newName: "IX_Participation_TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Participation",
                table: "Participation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Participation_AspNetUsers_UserId",
                table: "Participation",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Participation_Teams_TeamId",
                table: "Participation",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id");
        }
    }
}
