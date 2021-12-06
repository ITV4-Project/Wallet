using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class Alice
{
    public static void Main(string[] args)
    {
        Bob bob = new Bob();
        Trudy trudy = new Trudy();

        using (ECDsaCng dsa = new ECDsaCng(CngKey.Create(CngAlgorithm.ECDsaP256)))
        {
            dsa.HashAlgorithm = CngAlgorithm.Sha256;
            bob.key = dsa.Key.Export(CngKeyBlobFormat.EccPublicBlob);

            byte[] data = new byte[] { 21, 5, 8, 12, 207 };

            byte[] signature = dsa.SignData(data);

            bob.Receive(data, signature);
            trudy.Receive(data, signature);
        }
    }
}
public class Bob
{
    public byte[] key;

    public void Receive(byte[] data, byte[] signature)
    {
        using (ECDsaCng ecsdKey = new ECDsaCng(CngKey.Import(key, CngKeyBlobFormat.EccPublicBlob)))
        {
            Console.Write(this.GetType().Name);
            if (ecsdKey.VerifyData(data, signature))
                Console.WriteLine(" data is good");
            else
                Console.WriteLine(" data is bad");
        }
    }
}

public class Trudy
{
    public byte[] key;

    public void Receive(byte[] data, byte[] signature)
    {
        using (ECDsaCng ecsdKey = new ECDsaCng(CngKey.Create(CngAlgorithm.ECDsaP256)))
        {
            Console.Write(this.GetType().Name);
            if (ecsdKey.VerifyData(data, signature))
                Console.WriteLine(" data is good");
            else
                Console.WriteLine(" data is bad");
        }
    }
}