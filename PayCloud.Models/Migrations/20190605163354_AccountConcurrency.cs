using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PayCloud.Data.Migrations
{
    public partial class AccountConcurrency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "IsSent",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "Transactions",
                newName: "CreatedOn");

            migrationBuilder.AddColumn<DateTime>(
                name: "SendedOn",
                table: "Transactions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusCode",
                table: "Transactions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte[]>(
                name: "ConcurrencyRowVersion",
                table: "Accounts",
                rowVersion: true,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Name",
                table: "Clients",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountNumber",
                table: "Accounts",
                column: "AccountNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Clients_Name",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_AccountNumber",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "SendedOn",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "StatusCode",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ConcurrencyRowVersion",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Transactions",
                newName: "TimeStamp");

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "Transactions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSent",
                table: "Transactions",
                nullable: false,
                defaultValue: false);
        }
    }
}
