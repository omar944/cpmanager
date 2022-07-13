using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class codefrocesAccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CodeforceseAccounts_AspNetUsers_CodeforcesAccountForeignKey",
                table: "CodeforceseAccounts");

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

            migrationBuilder.AddForeignKey(
                name: "FK_CodeforceseAccounts_AspNetUsers_CodeforcesAccountForeignKey",
                table: "CodeforceseAccounts",
                column: "CodeforcesAccountForeignKey",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
