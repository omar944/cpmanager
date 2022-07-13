using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class codefrocesAccounts3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CodeforceseAccounts_AspNetUsers_OwnerId",
                table: "CodeforceseAccounts");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "CodeforceseAccounts",
                newName: "CodeforcesAccountForeignKey");

            migrationBuilder.RenameIndex(
                name: "IX_CodeforceseAccounts_OwnerId",
                table: "CodeforceseAccounts",
                newName: "IX_CodeforceseAccounts_CodeforcesAccountForeignKey");

            migrationBuilder.AddForeignKey(
                name: "FK_CodeforceseAccounts_AspNetUsers_CodeforcesAccountForeignKey",
                table: "CodeforceseAccounts",
                column: "CodeforcesAccountForeignKey",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CodeforceseAccounts_AspNetUsers_CodeforcesAccountForeignKey",
                table: "CodeforceseAccounts");

            migrationBuilder.RenameColumn(
                name: "CodeforcesAccountForeignKey",
                table: "CodeforceseAccounts",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_CodeforceseAccounts_CodeforcesAccountForeignKey",
                table: "CodeforceseAccounts",
                newName: "IX_CodeforceseAccounts_OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CodeforceseAccounts_AspNetUsers_OwnerId",
                table: "CodeforceseAccounts",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
