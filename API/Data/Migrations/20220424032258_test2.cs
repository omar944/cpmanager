using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class test2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrainingGroupId",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Problems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ContestId = table.Column<int>(type: "INTEGER", nullable: false),
                    Index = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    Rating = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Problems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CoachId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingGroups_AspNetUsers_CoachId",
                        column: x => x.CoachId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Submissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProblemId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProgrammingLanguage = table.Column<string>(type: "TEXT", nullable: true),
                    Verdict = table.Column<string>(type: "TEXT", nullable: true),
                    AuthorId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Submissions_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Submissions_Problems_ProblemId",
                        column: x => x.ProblemId,
                        principalTable: "Problems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProblemTag",
                columns: table => new
                {
                    ProblemsId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProblemTag", x => new { x.ProblemsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_ProblemTag_Problems_ProblemsId",
                        column: x => x.ProblemsId,
                        principalTable: "Problems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProblemTag_Tag_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TrainingGroupId",
                table: "AspNetUsers",
                column: "TrainingGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ProblemTag_TagsId",
                table: "ProblemTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_AuthorId",
                table: "Submissions",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_ProblemId",
                table: "Submissions",
                column: "ProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingGroups_CoachId",
                table: "TrainingGroups",
                column: "CoachId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_TrainingGroups_TrainingGroupId",
                table: "AspNetUsers",
                column: "TrainingGroupId",
                principalTable: "TrainingGroups",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_TrainingGroups_TrainingGroupId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ProblemTag");

            migrationBuilder.DropTable(
                name: "Submissions");

            migrationBuilder.DropTable(
                name: "TrainingGroups");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "Problems");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TrainingGroupId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TrainingGroupId",
                table: "AspNetUsers");
        }
    }
}
