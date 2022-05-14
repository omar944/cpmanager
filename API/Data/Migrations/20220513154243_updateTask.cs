using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class updateTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Problems_DailyTasks_DailyTaskId",
                table: "Problems");

            migrationBuilder.DropIndex(
                name: "IX_Problems_DailyTaskId",
                table: "Problems");

            migrationBuilder.DropColumn(
                name: "DailyTaskId",
                table: "Problems");

            migrationBuilder.CreateTable(
                name: "DailyTaskProblem",
                columns: table => new
                {
                    ProblemsId = table.Column<int>(type: "INTEGER", nullable: false),
                    TasksId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyTaskProblem", x => new { x.ProblemsId, x.TasksId });
                    table.ForeignKey(
                        name: "FK_DailyTaskProblem_DailyTasks_TasksId",
                        column: x => x.TasksId,
                        principalTable: "DailyTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyTaskProblem_Problems_ProblemsId",
                        column: x => x.ProblemsId,
                        principalTable: "Problems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyTaskProblem_TasksId",
                table: "DailyTaskProblem",
                column: "TasksId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyTaskProblem");

            migrationBuilder.AddColumn<int>(
                name: "DailyTaskId",
                table: "Problems",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Problems_DailyTaskId",
                table: "Problems",
                column: "DailyTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Problems_DailyTasks_DailyTaskId",
                table: "Problems",
                column: "DailyTaskId",
                principalTable: "DailyTasks",
                principalColumn: "Id");
        }
    }
}
