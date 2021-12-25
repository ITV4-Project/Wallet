using Microsoft.EntityFrameworkCore;

namespace WebWallet.Models
{
    public class ContextDB :DbContext
    {
        public DbSet<WalletModel> Wallets { get; set; }
       



        protected override void OnConfiguring(DbContextOptionsBuilder options)
                 => options.UseSqlite(@"Data Source = C:\Users\49152\source\repos\Wallet\Data\Wallets.db");
    }
}
