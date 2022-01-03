using System.ComponentModel.DataAnnotations;

namespace WebWallet.Models
{
    public class TransactionApi
    {
        public Guid Id { get; init; }
        public int Version { get; init; }
        public DateTimeOffset CreationDate { get; init; }
        public string? Name { get; init; }
        public byte[] MerkleHash { get; init; }
        public byte[] Input { get; init; }
        public int Amount { get; init; }
        public byte[] Output { get; init;}
        public bool Delegate { get; init; }
        public byte[] Signature { get; init; }
    }


}
