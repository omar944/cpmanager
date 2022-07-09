using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace API.Data.Migrations
{
    public partial class updateTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamUser_AspNetUsers_MembersId",
                table: "TeamUser");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamUser_Teams_TeamsId",
                table: "TeamUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamUser",
                table: "TeamUser");

            migrationBuilder.RenameColumn(
                name: "TeamsId",
                table: "TeamUser",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "MembersId",
                table: "TeamUser",
                newName: "TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_TeamUser_TeamsId",
                table: "TeamUser",
                newName: "IX_TeamUser_UserId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TeamUser",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "TeamUser",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CoachId",
                table: "Teams",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamUser",
                table: "TeamUser",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TeamUser_TeamId",
                table: "TeamUser",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_CoachId",
                table: "Teams",
                column: "CoachId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_AspNetUsers_CoachId",
                table: "Teams",
                column: "CoachId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamUser_AspNetUsers_UserId",
                table: "TeamUser",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamUser_Teams_TeamId",
                table: "TeamUser",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_AspNetUsers_CoachId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamUser_AspNetUsers_UserId",
                table: "TeamUser");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamUser_Teams_TeamId",
                table: "TeamUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamUser",
                table: "TeamUser");

            migrationBuilder.DropIndex(
                name: "IX_TeamUser_TeamId",
                table: "TeamUser");

            migrationBuilder.DropIndex(
                name: "IX_Teams_CoachId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TeamUser");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TeamUser");

            migrationBuilder.DropColumn(
                name: "CoachId",
                table: "Teams");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TeamUser",
                newName: "TeamsId");

            migrationBuilder.RenameColumn(
                name: "TeamId",
                table: "TeamUser",
                newName: "MembersId");

            migrationBuilder.RenameIndex(
                name: "IX_TeamUser_UserId",
                table: "TeamUser",
                newName: "IX_TeamUser_TeamsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamUser",
                table: "TeamUser",
                columns: new[] { "MembersId", "TeamsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TeamUser_AspNetUsers_MembersId",
                table: "TeamUser",
                column: "MembersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamUser_Teams_TeamsId",
                table: "TeamUser",
                column: "TeamsId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
