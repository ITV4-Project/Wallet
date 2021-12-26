namespace WebWallet.Models
{
    public class TransactionModel
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTime CreationDate { get; set; }
        public string? Name { get; set; }
        public string MerkleHash { get; set; }
        public string Input { get; set; }
        public int Amount { get; set; }
        public string Output { get; set; }
        public bool Delegate { get; set; }
        public string Signature { get; set; }

        public TransactionModel() { }
    }
}
