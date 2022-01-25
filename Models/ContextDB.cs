using Microsoft.EntityFrameworkCore;
using WebWallet.Models;

namespace WebWallet.Models
{
    public class ContextDB :DbContext
    {
        public DbSet<WalletModel> Wallets { get; set; }
       



        protected override void OnConfiguring(DbContextOptionsBuilder options)
                 => options.UseSqlite(@"Data Source = C:\Users\Lenovo\source\repos\Wallet\Data\Wallets.db");
       



        public DbSet<WebWallet.Models.TransactionApi> TransactionApi { get; set; }
       



        public DbSet<WebWallet.Models.TransactionModel> TransactionModel { get; set; }
    }
}
