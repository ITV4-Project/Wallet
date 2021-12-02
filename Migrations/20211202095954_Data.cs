using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebWallet.Migrations
{
    public partial class Data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserFName = table.Column<string>(type: "TEXT", nullable: false),
                    UserLName = table.Column<string>(type: "TEXT", nullable: false),
                    WalletName = table.Column<string>(type: "TEXT", nullable: false),
                    PassPhrase = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Wallets");
        }
    }
}
