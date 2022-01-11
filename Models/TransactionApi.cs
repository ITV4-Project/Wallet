using System.ComponentModel.DataAnnotations;

namespace WebWallet.Models
{
    public class TransactionApi
    {
        public Guid Id { get; init; }
        public int Version { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public string? Name { get; set; }
        public byte[] MerkleHash { get; set; }
        public byte[] Input { get; set; }
        public int Amount { get; set; }
        public byte[] Output { get; set; }
        public bool Delegate { get; set; }
        public byte[] Signature { get; set; }
    }

}
