using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class test4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingGroups_AspNetUsers_CoachId",
                table: "TrainingGroups");

            migrationBuilder.AlterColumn<int>(
                name: "CoachId",
                table: "TrainingGroups",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingGroups_AspNetUsers_CoachId",
                table: "TrainingGroups",
                column: "CoachId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingGroups_AspNetUsers_CoachId",
                table: "TrainingGroups");

            migrationBuilder.AlterColumn<int>(
                name: "CoachId",
                table: "TrainingGroups",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingGroups_AspNetUsers_CoachId",
                table: "TrainingGroups",
                column: "CoachId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
