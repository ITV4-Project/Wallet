using Core;

namespace WebWallet.Models {

	public record TransactionRecord : Transaction {

		public TransactionRecord() {
			Name = "";
		}

		public string? Name { get; set; }

		public static TransactionRecord FromModel(TransactionModel transactionModel) {
			return new TransactionRecord() {
				Id = transactionModel.Id,
				Version = transactionModel.Version,
				Name = transactionModel.Name,
				MerkleHash = Convert.FromBase64String(transactionModel.MerkleHash),
				Input = Convert.FromHexString(transactionModel.Input),
				Output = Convert.FromHexString(transactionModel.Output),
				IsDelegating = transactionModel.IsDelegating,
			};
		}
	}
}
