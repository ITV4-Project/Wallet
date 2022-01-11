using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
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
        public ActionResult Transaction()
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
            TransactionApi convertedTransaction = new TransactionApi
            {
                Version = transaction.Version != null ? transaction.Version : 0,
                Name = transaction.Name != null ? transaction.Name : "Bob",
                MerkleHash = Encoding.ASCII.GetBytes("t4or5p62SBIIvb6hKNxl/6pXt+7wsRwLQTUeq0O1Unmzu6XGWo+oI8g7QAECFY2DxkVlfmYus9Rc79MgV9XvGg=="), //testing
                Input = Encoding.ASCII.GetBytes(transaction.Input),
                Amount = transaction.Amount,
                Output = Encoding.ASCII.GetBytes(transaction.Output),
                Delegate = transaction.Delegate != null ? transaction.Delegate : false,
                Signature = Encoding.ASCII.GetBytes("t4or5p62SBIIvb6hKNxl/6pXt+7wsRwLQTUeq0O1Unmzu6XGWo+oI8g7QAECFY2DxkVlfmYus9Rc79MgV9XvGg=="), //testing

           
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
                        TempData["Balance"] = wallet.Balance;
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
                var postTask = client.PostAsJsonAsync<TransactionApi>("transactions", convertedTransaction);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                    return RedirectToAction("Details");

            }
            ModelState.AddModelError(string.Empty, "Error");
            var startTime = DateTime.Now;
            Console.WriteLine(JsonConvert.SerializeObject(convertedTransaction, Formatting.Indented));
            var endTime = DateTime.Now;
            Console.WriteLine($"Duration: {endTime - startTime}");

            return View("Details");

        }

        //Get transaction 

        public ActionResult GetTransaction()
        {

            IEnumerable<TransactionApi> transactions = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7157/");
                //HTTP GET
                var responseTask = client.GetAsync("transactions");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<TransactionApi>>();
                    readTask.Wait();
                    transactions = readTask.Result;





                    foreach (var transcation in transactions)

                    {

                        TempData["Version"] = transcation.Version;
                        TempData["CreationDate"] = transcation.CreationDate;
                        TempData["Name"] = transcation.Name;
                        TempData["MerkleHash"] = Encoding.Default.GetString(transcation.MerkleHash);
                        TempData["Amount"] = transcation.Amount;
                        TempData["Input"] = Convert.ToBase64String(transcation.Input);
                        TempData["Output"] = Convert.ToBase64String(transcation.Output);
                        TempData["Delegate"] = transcation.Delegate;
                        TempData["Signature"] = Encoding.Default.GetString(transcation.Signature);


                        Console.WriteLine(transcation);
                    }
                    //  return View("Index");

                }
                //web api sent error response 
                else
                {
                    transactions = Enumerable.Empty<TransactionApi>();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }

            }
            return View("Index");

        }

        [HttpPost]
        public ActionResult QR(TransactionModel transaction)
        {


            using (MemoryStream ms = new MemoryStream())
            {
                TempData["key"] = transaction.Input;
                QRCodeGenerator codeGenerator = new QRCodeGenerator();
                string qrstring = "Receiver is : " + transaction.Input + "Amount: " + transaction.Amount + "Date : " + transaction.CreationDate;
                QRCodeData codeData = codeGenerator.CreateQrCode(qrstring, QRCodeGenerator.ECCLevel.Q);
                QRCode qr = new QRCode(codeData);

                using (Bitmap bitmap = qr.GetGraphic(20))
                {
                    bitmap.Save(ms, ImageFormat.Png);
                    ViewBag.QRCodeImage = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                }
            }



            return View("Receive");
        }


    }
}
