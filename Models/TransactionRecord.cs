using Core;

namespace WebWallet.Models {

	public record TransactionRecord : Transaction {

		public TransactionRecord() {
			Name = "";
		}

		public string? Name { get; set; }

		public static TransactionRecord FromModel(TransactionModel transactionModel) {
			return new TransactionRecord() {
				Version = transactionModel.Version,
				CreationTime = DateTimeOffset.UtcNow,
				MerkleHash = Convert.FromBase64String(transactionModel.MerkleHash),
				Input = Convert.FromHexString(transactionModel.Input),
				Amount = transactionModel.Amount,
				Output = Convert.FromHexString(transactionModel.Output),
				IsDelegating = transactionModel.IsDelegating,
			};
		}
	}
}
