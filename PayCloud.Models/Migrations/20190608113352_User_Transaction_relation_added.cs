using Microsoft.EntityFrameworkCore.Migrations;

namespace PayCloud.Data.Migrations
{
    public partial class User_Transaction_relation_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "Transactions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CreatedByUserId",
                table: "Transactions",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PayCloudUsers_Username",
                table: "PayCloudUsers",
                column: "Username",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_PayCloudUsers_CreatedByUserId",
                table: "Transactions",
                column: "CreatedByUserId",
                principalTable: "PayCloudUsers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_PayCloudUsers_CreatedByUserId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_CreatedByUserId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_PayCloudUsers_Username",
                table: "PayCloudUsers");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Transactions");
        }
    }
}
