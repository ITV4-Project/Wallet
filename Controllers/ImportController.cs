﻿using Microsoft.AspNetCore.Mvc;
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
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ImportController/Create
        public ActionResult Import()
        {
            return View();
        }

        public ActionResult ImportW(ImportWalletModel wallet)
        {
            var hash = Hash(wallet.PassWallet);
            List<WalletModel> wallets = new List<WalletModel>();


            using (var db = new ContextDB())
            {

                wallets = db.Wallets.Select(a => a).Where(a => a.PassPhrase.Equals(hash)).ToList();
                if (wallets != null)
                {
                    TempData["wallets"] = wallets;
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



        public ActionResult Send()
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

            var startTime = DateTime.Now;
            Console.WriteLine(JsonConvert.SerializeObject(transaction, Formatting.Indented));
            var endTime = DateTime.Now;
            Console.WriteLine($"Duration: {endTime - startTime}");

        }



    }
}
