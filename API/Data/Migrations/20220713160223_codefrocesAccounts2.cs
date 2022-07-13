using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class codefrocesAccounts2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CodeforceseAccounts_AspNetUsers_CodeforcesAccountForeignKey",
                table: "CodeforceseAccounts");

            migrationBuilder.DropIndex(
                name: "IX_CodeforceseAccounts_CodeforcesAccountForeignKey",
                table: "CodeforceseAccounts");

            migrationBuilder.DropColumn(
                name: "CodeforcesAccountForeignKey",
                table: "CodeforceseAccounts");

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "CodeforceseAccounts",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CodeforceseAccounts_OwnerId",
                table: "CodeforceseAccounts",
                column: "OwnerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CodeforceseAccounts_AspNetUsers_OwnerId",
                table: "CodeforceseAccounts",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CodeforceseAccounts_AspNetUsers_OwnerId",
                table: "CodeforceseAccounts");

            migrationBuilder.DropIndex(
                name: "IX_CodeforceseAccounts_OwnerId",
                table: "CodeforceseAccounts");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "CodeforceseAccounts");

            migrationBuilder.AddColumn<int>(
                name: "CodeforcesAccountForeignKey",
                table: "CodeforceseAccounts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CodeforceseAccounts_CodeforcesAccountForeignKey",
                table: "CodeforceseAccounts",
                column: "CodeforcesAccountForeignKey",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CodeforceseAccounts_AspNetUsers_CodeforcesAccountForeignKey",
                table: "CodeforceseAccounts",
                column: "CodeforcesAccountForeignKey",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
