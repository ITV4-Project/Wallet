using System.ComponentModel.DataAnnotations;

namespace WebWallet.Models
{
    public class WalletModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string UserFName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string UserLName { get; set; }
        [Required]
        [Display(Name = "Wallet Name")]
        public string WalletName { get; set; }

        [Required, DataType(DataType.Password)]
        public string PassPhrase { get; set; }

        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public int Balance { get; set; }
        public WalletModel()
        {

        }

    }
}
