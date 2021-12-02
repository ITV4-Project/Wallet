using Microsoft.EntityFrameworkCore;

namespace WebWallet.Models
{
    public class ContextDB :DbContext
    {
        public DbSet<WalletModel> Wallets { get; set; }
       



        protected override void OnConfiguring(DbContextOptionsBuilder options)
                 => options.UseSqlite(@"Data Source = D:\University books\Jaar4\project4\WebWallet\Data\Wallets.db");
    }
}
