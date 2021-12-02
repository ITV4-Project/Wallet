using System.ComponentModel.DataAnnotations;

namespace WebWallet.Models
{
    public class ImportWalletModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string PassWallet { get; set; }

        public ImportWalletModel() { }
    }
}
