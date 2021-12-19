namespace WebWallet.Models
{
    public class TransactionModel
    {

        public int version;
        public string previousHash { get; set; }
        public string senderPublicKey { get; set; }
        public string recieverPublicKey { get; set; }

        public bool delegates { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }

        public TransactionModel() { }
    }
}
