using Microsoft.EntityFrameworkCore;
using WebWallet.Models;

namespace WebWallet.Models
{
    public class ContextDB :DbContext
    {
        public ContextDB() {
            Database.EnsureCreated();
        }

        public DbSet<WalletModel> Wallets => Set<WalletModel>();
        public DbSet<TransactionRecord> TransactionRecord => Set<TransactionRecord>();
        public DbSet<TransactionModel> TransactionModel => Set<TransactionModel>();

        protected override void OnConfiguring(DbContextOptionsBuilder options)
                 => options.UseSqlite($"Data Source={GetLocalAppDataDatabase()}");
       
        public static string GetLocalAppDataDatabase() {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            return Path.Join(path, "wallet.db");
        }
    }
}