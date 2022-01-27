namespace WebWallet.Models
{
    public class TransactionModel
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset CreationTime { get; set; }
        public string? Name { get; set; }
        public string MerkleHash { get; set; }
        public string Input { get; set; }
        public int Amount { get; set; }
        public string Output { get; set; }
        public string privateKey { get; set; }
        public bool IsDelegating { get; set; }

        public TransactionModel() {
            Id = Guid.NewGuid();
            Version = 1;
            MerkleHash = string.Empty;
            Input = string.Empty;
            Amount = 0;
            Output = string.Empty;
            IsDelegating = false;
        }
    }
}
