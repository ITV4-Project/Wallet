using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebWallet.Migrations
{
    public partial class privateKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PrivateKey",
                table: "Wallets",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "TransactionModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Version = table.Column<int>(type: "INTEGER", nullable: false),
                    CreationTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    MerkleHash = table.Column<string>(type: "TEXT", nullable: false),
                    Input = table.Column<string>(type: "TEXT", nullable: false),
                    Amount = table.Column<int>(type: "INTEGER", nullable: false),
                    Output = table.Column<string>(type: "TEXT", nullable: false),
                    privateKey = table.Column<string>(type: "TEXT", nullable: false),
                    IsDelegating = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Version = table.Column<int>(type: "INTEGER", nullable: false),
                    CreationTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    MerkleHash = table.Column<byte[]>(type: "BLOB", nullable: false),
                    Input = table.Column<byte[]>(type: "BLOB", nullable: false),
                    Output = table.Column<byte[]>(type: "BLOB", nullable: false),
                    Amount = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDelegating = table.Column<bool>(type: "INTEGER", nullable: false),
                    Signature = table.Column<byte[]>(type: "BLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionRecord", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionModel");

            migrationBuilder.DropTable(
                name: "TransactionRecord");

            migrationBuilder.DropColumn(
                name: "PrivateKey",
                table: "Wallets");
        }
    }
}
