using Microsoft.AspNetCore.Mvc;
using System.Web.Helpers;
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
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ImportController/Create
        public ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportW(ImportWalletModel wallet)
        {
            var hash = Hash(wallet.PassWallet);
            
            using (var db = new ContextDB())
            {
                //Not wordking good //Todo
                var v = db.Wallets.Where(a => a.PassPhrase.Equals(hash));
                if (v != null)
                {
                    return RedirectToAction("Details", v);
                }

            }

            //Index is voor testing empty View
            return View("Index");
        }
        private string Hash(String hash)
        {
            return Crypto.HashPassword(hash);

        }






    }
}
