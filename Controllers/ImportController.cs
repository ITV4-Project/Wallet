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

        // GET: ImportController
        public ActionResult Transcation()
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

            return View("Import");
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
            return View("Transaction");

        }

        public ActionResult Receive()
        {
            //to do add QR code view
            return View();
        }

        //create transaction
        public ActionResult CreateTransaction(TransactionModel transaction)
        {
            var transcation = new TransactionModel
            {
                Version = 1,
                MerkleHash = null,
                Input = transaction.Input,
                Output = transaction.Output,
                Delegate = false,
                Amount = transaction.Amount,
                CreationDate = transaction.CreationDate,


            };
            using (var db = new ContextDB())
            {
                //update wallet balance for sender
                var balanceSender = db.Wallets.Select(a => a).Where(a => a.PublicKey.Equals(transaction.Input));
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
                var balanceReciever = db.Wallets.Select(a => a).Where(a => a.PublicKey.Equals(transaction.Output));
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

            //Post Transaction to Api
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7157/Transactions");
                //HTTP POST
                var postTask = client.PostAsJsonAsync<TransactionModel>("transactions", transcation);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                    return RedirectToAction("Index");

            }
            ModelState.AddModelError(string.Empty, "Error");
            var startTime = DateTime.Now;
            Console.WriteLine(JsonConvert.SerializeObject(transaction, Formatting.Indented));
            var endTime = DateTime.Now;
            Console.WriteLine($"Duration: {endTime - startTime}");

            return View("Index");

        }

        //Get transaction 

        public ActionResult GetTransaction()
        {
            IEnumerable<TransactionModel> transactions = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7157/");
                //HTTP GET
                var responseTask = client.GetAsync("transactions");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<TransactionModel>>();
                    readTask.Wait();
                    transactions = readTask.Result;
                    //testing result
                    //  foreach(var transcation in transactions)
                    //{
                    //  Console.WriteLine(transcation);
                    //}

                }
                //web api sent error response 
                else
                {
                    transactions = Enumerable.Empty<TransactionModel>();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }

            }
            return View("Index");
        }



    }
}
