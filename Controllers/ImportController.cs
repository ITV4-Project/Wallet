using Core;
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
                        TempData["Name"] = wallet.UserFName + " " + wallet.UserLName;
                        return View("Details");
                    }


                }
                else { return View("Import"); }
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
        [HttpPost]
        public ActionResult CreateTransaction(TransactionModel transactionDto)
        {
            //using database to find the private key that related to public key
            using (var db = new ContextDB())
            {
                //using database to find the private key that related to public key
                var privateKey = db.Wallets.Select(a => a).Where(a => a.PublicKey.Equals(transactionDto.Input)).ToList();
                foreach (var item in privateKey)
                {
                    WalletModel wallet = db.Wallets.Find(item.Id);
                    transactionDto.privateKey = wallet.PrivateKey;
                    
                }
                //using private key to sign
                ECDsaKey key = ECDsaKey.FromPrivateKey(transactionDto.privateKey);


                transactionDto.MerkleHash = "SlUgtuy9iKyXHYa9wyrguAO0dHIoipt0VPVEGr9vCbrunF/zrlZZ6wGkDd/aNzaXwRpmVz4gDJzLzhYNXM27Jg=="; // Testing
                
                Transaction transaction = new
                    (
                       
                        merkleHash: Convert.FromBase64String(transactionDto.MerkleHash),
                        input: Convert.FromHexString(transactionDto.Input),
                        output: Convert.FromHexString(transactionDto.Output),
                        amount: transactionDto.Amount,
                        isDelegating: transactionDto.IsDelegating
                    );

                // Dit werkt nu
                // De transactie moet ondertekend worden met de key die bij de Input hoort. Niet een lege ECDsaKey()
                transaction.Sign(key);

                //update wallet balance for sender
                var balanceSender = db.Wallets.Select(a => a).Where(a => a.PublicKey.Equals(transactionDto.Input));
                if (balanceSender != null)
                {
                    foreach (var item in balanceSender)
                    {
                        WalletModel wallet = db.Wallets.Find(item.Id);
                        wallet.Balance = wallet.Balance - transactionDto.Amount;
                        TempData["Balance"] = wallet.Balance;
                    }
                    db.SaveChanges();

                }
                // update wallet balance for reciver 
                var balanceReciever = db.Wallets.Select(a => a).Where(a => a.PublicKey.Equals(transactionDto.Output));
                if (balanceReciever != null)
                {
                    foreach (var item in balanceReciever)
                    {
                        WalletModel wallet = db.Wallets.Find(item.Id);
                        wallet.Balance = wallet.Balance + transactionDto.Amount;
                    }
                    db.SaveChanges();

                }


                //Post Transaction to Api
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7157/transactions");
                    //HTTP POST
                    var postTask = client.PostAsJsonAsync("transactions", transaction);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                        return RedirectToAction("Details");

                }
                ModelState.AddModelError(string.Empty, "Error");
                var startTime = DateTime.Now;
                Console.WriteLine(JsonConvert.SerializeObject(transaction, Formatting.Indented));
                var endTime = DateTime.Now;
                Console.WriteLine($"Duration: {endTime - startTime}");

                return View("Details");


            }
        }


        //Get transaction 
        public ActionResult GetTransaction()
        {

            IEnumerable<TransactionRecord> transactions = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7157/");
                //HTTP GET
                var responseTask = client.GetAsync("Transactions");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadFromJsonAsync<IList<TransactionRecord>>();
                    readTask.Wait();
                    transactions = readTask.Result;
                    foreach (var transcation in transactions)
                    {
                        ViewData["Version"] = transcation.Version;
                        ViewData["CreationDate"] = transcation.CreationTime;
                        ViewData["MerkleHash"] = Convert.ToBase64String(transcation.MerkleHash).ToString();
                        ViewData["Amount"] = transcation.Amount;
                        ViewData["Input"] = Convert.ToBase64String(transcation.Input).ToString();
                        ViewData["Output"] = Convert.ToBase64String(transcation.Output).ToString();
                        ViewData["Delegate"] = transcation.IsDelegating;
                        ViewData["Signature"] = Convert.ToBase64String(transcation.Signature).ToString();

                    }

                    
                }
                //web api sent error response 
                else
                {
                    transactions = Enumerable.Empty<TransactionRecord>();
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
                TempData["sender"] = transaction.Input;
                QRCodeGenerator codeGenerator = new QRCodeGenerator();
                string qrstring = "Receiver is : " + transaction.Input + "Amount: " + transaction.Amount + "Date : " + transaction.CreationTime;
                string url = "https://10.51.20.71:7048/Import/Import";
                QRCodeData codeData = codeGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
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
