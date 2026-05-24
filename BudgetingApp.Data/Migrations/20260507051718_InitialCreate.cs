using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetingApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlaidUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ClientUserId = table.Column<string>(type: "TEXT", nullable: false),
                    LinkToken = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaidUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlaidUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    BudgetPeriodStartDay = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSettings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlaidItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AccessToken = table.Column<string>(type: "TEXT", nullable: false),
                    PlaidItemId = table.Column<string>(type: "TEXT", nullable: false),
                    InstitutionId = table.Column<string>(type: "TEXT", nullable: true),
                    InstitutionName = table.Column<string>(type: "TEXT", nullable: true),
                    NextCursor = table.Column<string>(type: "TEXT", nullable: true),
                    HasMore = table.Column<bool>(type: "INTEGER", nullable: true),
                    ItemStatus = table.Column<string>(type: "TEXT", nullable: false),
                    ErrorCode = table.Column<string>(type: "TEXT", nullable: false),
                    ConsentExpirationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastSyncedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PlaidUserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaidItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlaidItems_PlaidUsers_PlaidUserId",
                        column: x => x.PlaidUserId,
                        principalTable: "PlaidUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlaidAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AccountId = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    CurrentBalance = table.Column<decimal>(type: "TEXT", nullable: true),
                    BalanceAvailable = table.Column<decimal>(type: "TEXT", nullable: false),
                    BalanceLimit = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsHidden = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastSyncedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Mask = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    VerificationStatus = table.Column<string>(type: "TEXT", nullable: true),
                    PlaidItemId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaidAccounts", x => x.Id);
                    table.UniqueConstraint("AK_PlaidAccounts_AccountId", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_PlaidAccounts_PlaidItems_PlaidItemId",
                        column: x => x.PlaidItemId,
                        principalTable: "PlaidItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlaidTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TransactionId = table.Column<string>(type: "TEXT", nullable: true),
                    MerchantName = table.Column<string>(type: "TEXT", nullable: true),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: true),
                    AuthorizedDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    Pending = table.Column<bool>(type: "INTEGER", nullable: true),
                    PlaidAccountId = table.Column<string>(type: "TEXT", nullable: true),
                    AccountId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaidTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlaidTransactions_PlaidAccounts_PlaidAccountId",
                        column: x => x.PlaidAccountId,
                        principalTable: "PlaidAccounts",
                        principalColumn: "AccountId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlaidAccounts_PlaidItemId",
                table: "PlaidAccounts",
                column: "PlaidItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaidItems_PlaidUserId",
                table: "PlaidItems",
                column: "PlaidUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaidTransactions_PlaidAccountId",
                table: "PlaidTransactions",
                column: "PlaidAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaidUsers_UserId",
                table: "PlaidUsers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_UserId",
                table: "UserSettings",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlaidTransactions");

            migrationBuilder.DropTable(
                name: "UserSettings");

            migrationBuilder.DropTable(
                name: "PlaidAccounts");

            migrationBuilder.DropTable(
                name: "PlaidItems");

            migrationBuilder.DropTable(
                name: "PlaidUsers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
