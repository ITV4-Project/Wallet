using Core;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using WebWallet.Models;

namespace WebWallet.Controllers
{
    public class WalletController : Controller
    {
      //  private ECDsa key = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        ECDsaKey key = new ECDsaKey();



        // GET: WalletController
        public ActionResult Create()
        {
            return View();
        }




        [HttpPost]
        public ActionResult CreateW(WalletModel Wallet)
        {
            var hash = Hash(Wallet.PassPhrase);
            List<WalletModel> wallets = new List<WalletModel>();

            var w = new WalletModel
            {
                UserFName = Wallet.UserFName,
                UserLName = Wallet.UserLName,
                WalletName = Wallet.WalletName,
                PassPhrase = hash,
                PublicKey =  key.GetPublicKey(),
                //default balance value by creating wallet 
                Balance = 10, 

            };

            using (var db = new ContextDB())
            {
                db.Wallets.Add(w);
                db.SaveChanges();

                return RedirectToAction("Import", "Import");

            }

            return View("Create");
        }

      
        

        private string Hash(String hash)
        {
            //One way hash using SHA-256
            using var sha = SHA256.Create();
            var asBytes = Encoding.Default.GetBytes(hash);
            var hashed = sha.ComputeHash(asBytes);
            return Convert.ToBase64String(hashed);


        }

       

     











    }
}
