using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using WebWallet.Models;

namespace WebWallet.Controllers
{
    public class WalletController : Controller
    {
        private ECDsa key = ECDsa.Create(ECCurve.NamedCurves.nistP256);

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
                PublicKey = GetPublicKey(),

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

        public void ECDsaKey(String exisitingKey, bool isPrivateKey)
        {
            if (isPrivateKey)
            {
                ECParameters parameters = new ECParameters();
                parameters.Curve = ECCurve.NamedCurves.nistP256;
                parameters.D = Convert.FromHexString(exisitingKey);

                key = ECDsa.Create(parameters);
            }
            else
            {
                ECParameters parameters = new ECParameters();
                parameters.Curve = ECCurve.NamedCurves.nistP256;

                byte[] publicKeyBytes = Convert.FromHexString(exisitingKey);
                parameters.Q = new ECPoint
                {
                    X = publicKeyBytes.Skip(1).Take(16).ToArray(),
                    Y = publicKeyBytes.Skip(17).ToArray()
                };

                key = ECDsa.Create(parameters);
            }
        }

        public string GetPrivateKey()
        {
            ECParameters p = key.ExportParameters(true);
            var privateKey = p.D;
            return Convert.ToHexString(privateKey);
        }

        public string GetPublicKey()
        {
            ECParameters p = key.ExportParameters(true);
            byte[] prefix = { 0x04 };
            byte[] x = p.Q.X;
            byte[] y = p.Q.Y;
            byte[] publicKey = prefix.Concat(x).Concat(y).ToArray();
            return Convert.ToHexString(publicKey);
        }











    }
}
