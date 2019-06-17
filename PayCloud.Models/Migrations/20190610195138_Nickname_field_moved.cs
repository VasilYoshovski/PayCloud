using Microsoft.EntityFrameworkCore.Migrations;

namespace PayCloud.Data.Migrations
{
    public partial class Nickname_field_moved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NickName",
                table: "Accounts");

            migrationBuilder.AddColumn<string>(
                name: "AccountNickname",
                table: "UsersAccounts",
                maxLength: 35,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNickname",
                table: "UsersAccounts");

            migrationBuilder.AddColumn<string>(
                name: "NickName",
                table: "Accounts",
                maxLength: 35,
                nullable: false,
                defaultValue: "");
        }
    }
}
