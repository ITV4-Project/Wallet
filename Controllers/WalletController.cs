using Microsoft.AspNetCore.Mvc;
using System.Web.Helpers;
using WebWallet.Models;

namespace WebWallet.Controllers
{
    public class WalletController : Controller
    {
        // GET: WalletController
        public ActionResult Create()
        {
            return View();
        }



        [HttpPost]
        public ActionResult CreateW(WalletModel Wallet)
        {
             var hash = Hash(Wallet.PassPhrase);
             var w = new WalletModel
                {
                    UserFName = Wallet.UserFName,
                    UserLName = Wallet.UserLName,
                    WalletName = Wallet.WalletName,
                    PassPhrase = hash,

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
            return Crypto.HashPassword(hash);
            
        }


            

                


          

        

    }
}
