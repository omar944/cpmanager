using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class test3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_TrainingGroups_TrainingGroupId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TrainingGroupId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TrainingGroupId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "TrainingGroupUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TrainingGroupId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingGroupUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingGroupUser_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingGroupUser_TrainingGroups_TrainingGroupId",
                        column: x => x.TrainingGroupId,
                        principalTable: "TrainingGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingGroupUser_TrainingGroupId",
                table: "TrainingGroupUser",
                column: "TrainingGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingGroupUser_UserId",
                table: "TrainingGroupUser",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingGroupUser");

            migrationBuilder.AddColumn<int>(
                name: "TrainingGroupId",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TrainingGroupId",
                table: "AspNetUsers",
                column: "TrainingGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_TrainingGroups_TrainingGroupId",
                table: "AspNetUsers",
                column: "TrainingGroupId",
                principalTable: "TrainingGroups",
                principalColumn: "Id");
        }
    }
}
