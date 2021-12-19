using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using WebWallet.Models;

namespace WebWallet.Controllers
{
    public class ImportController : Controller
    {
        // GET: ImportController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ImportController/Details/5
        public ActionResult Details()
        {
            return View();
        }

        // GET: ImportController/Create
        public ActionResult Import()
        {
            return View();
        }

        public ActionResult ImportW(ImportWalletModel importedWallet)
        {
            var hash = Hash(importedWallet.PassWallet);
          


            using (var db = new ContextDB())
            {

                var wallets = db.Wallets.Select(a => a).Where(a => a.PassPhrase.Equals(hash)).ToList();

                if (wallets != null)
                {
                    foreach (var wallet in wallets)
                    {
                        TempData["wallet"] = wallet.WalletName;
                        TempData["Balance"] = wallet.Balance;
                        TempData["key"] = wallet.PublicKey;

                    }


                    return View("Details");


                }
            }

            return View("Index");
        }
        private string Hash(String hash)
        {
            //One way hash using SHA-256
            using var sha = SHA256.Create();
            var asBytes = Encoding.Default.GetBytes(hash);
            var hashed = sha.ComputeHash(asBytes);
            return Convert.ToBase64String(hashed);


        }



        public ActionResult Send(ImportWalletModel importWallet)
        {
            return View("Index");

        }

        public ActionResult Receive()
        {
            return View("Index");
        }

        public void CreateTransaction(TransactionModel transaction)
        {
            var transcation = new TransactionModel
            {
                version = 1,
                previousHash = Hash("Een"),
                senderPublicKey = transaction.senderPublicKey,
                recieverPublicKey = transaction.recieverPublicKey,
                delegates = false,
                Amount = transaction.Amount,
                Date = transaction.Date,

            };
            using (var db = new ContextDB())
            {
                //update wallet balance for sender
                var balanceSender = db.Wallets.Select(a => a).Where(a => a.PublicKey.Equals(transaction.senderPublicKey));
                if (balanceSender != null)
                {
                    foreach (var item in balanceSender)
                    {
                        WalletModel wallet = db.Wallets.Find(item.Id);
                        wallet.Balance = wallet.Balance - transaction.Amount;
                    }
                    db.SaveChanges();

                }
                // update wallet balance for reciver 
                var balanceReciever = db.Wallets.Select(a => a).Where(a => a.PublicKey.Equals(transaction.recieverPublicKey));
                if (balanceReciever != null)
                {
                    foreach (var item in balanceReciever)
                    {
                        WalletModel wallet = db.Wallets.Find(item.Id);
                        wallet.Balance = wallet.Balance + transaction.Amount;
                    }
                    db.SaveChanges();

                }
            }
            var startTime = DateTime.Now;
            Console.WriteLine(JsonConvert.SerializeObject(transaction, Formatting.Indented));
            var endTime = DateTime.Now;
            Console.WriteLine($"Duration: {endTime - startTime}");

        }



    }
}
