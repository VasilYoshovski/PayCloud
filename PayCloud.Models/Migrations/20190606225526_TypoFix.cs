using Microsoft.EntityFrameworkCore.Migrations;

namespace PayCloud.Data.Migrations
{
    public partial class TypoFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SendedOn",
                table: "Transactions",
                newName: "SentOn");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SentOn",
                table: "Transactions",
                newName: "SendedOn");
        }
    }
}
