using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PayCloud.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    BannerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UrlLink = table.Column<string>(nullable: false),
                    ImgLocationPath = table.Column<string>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banners", x => x.BannerId);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ClientId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 35, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ClientId);
                });

            migrationBuilder.CreateTable(
                name: "PayCloudAdmins",
                columns: table => new
                {
                    AdminId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 35, nullable: false),
                    Username = table.Column<string>(maxLength: 16, nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Role = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayCloudAdmins", x => x.AdminId);
                });

            migrationBuilder.CreateTable(
                name: "PayCloudUsers",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 35, nullable: false),
                    Username = table.Column<string>(maxLength: 16, nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Role = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayCloudUsers", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountNumber = table.Column<string>(maxLength: 10, nullable: false),
                    Balance = table.Column<decimal>(nullable: false),
                    ClientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Accounts_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersClients",
                columns: table => new
                {
                    PayCloudUserId = table.Column<int>(nullable: false),
                    ClientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersClients", x => new { x.PayCloudUserId, x.ClientId });
                    table.ForeignKey(
                        name: "FK_UsersClients_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersClients_PayCloudUsers_PayCloudUserId",
                        column: x => x.PayCloudUserId,
                        principalTable: "PayCloudUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(nullable: false),
                    Description = table.Column<string>(maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    SentOn = table.Column<DateTime>(nullable: true),
                    StatusCode = table.Column<int>(nullable: false),
                    CreatedByUserId = table.Column<int>(nullable: false),
                    SenderAccountId = table.Column<int>(nullable: false),
                    ReceiverAccountId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transactions_PayCloudUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "PayCloudUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_ReceiverAccountId",
                        column: x => x.ReceiverAccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_SenderAccountId",
                        column: x => x.SenderAccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsersAccounts",
                columns: table => new
                {
                    PayCloudUserId = table.Column<int>(nullable: false),
                    AccountId = table.Column<int>(nullable: false),
                    AddedOn = table.Column<DateTime>(nullable: false),
                    AccountNickname = table.Column<string>(maxLength: 35, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersAccounts", x => new { x.PayCloudUserId, x.AccountId });
                    table.ForeignKey(
                        name: "FK_UsersAccounts_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersAccounts_PayCloudUsers_PayCloudUserId",
                        column: x => x.PayCloudUserId,
                        principalTable: "PayCloudUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountNumber",
                table: "Accounts",
                column: "AccountNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_ClientId",
                table: "Accounts",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Name",
                table: "Clients",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PayCloudAdmins_Username",
                table: "PayCloudAdmins",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PayCloudUsers_Username",
                table: "PayCloudUsers",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CreatedByUserId",
                table: "Transactions",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ReceiverAccountId",
                table: "Transactions",
                column: "ReceiverAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SenderAccountId",
                table: "Transactions",
                column: "SenderAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersAccounts_AccountId",
                table: "UsersAccounts",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersClients_ClientId",
                table: "UsersClients",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banners");

            migrationBuilder.DropTable(
                name: "PayCloudAdmins");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "UsersAccounts");

            migrationBuilder.DropTable(
                name: "UsersClients");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "PayCloudUsers");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
