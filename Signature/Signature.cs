using System.Security.Cryptography;
using System.Text;
using static System.Console;
public class Sender
{
    public static byte[] SenderPublicKey;

    static void Main(string[] args)
    {
        Write("Enter message to be encrypted: ");
        string message  = ReadLine();

        using (ECDiffieHellmanCng ecd = new ECDiffieHellmanCng())
        {
            ecd.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
            ecd.HashAlgorithm = CngAlgorithm.Sha256;
            SenderPublicKey = ecd.PublicKey.ToByteArray();

            Receiver receiver = new Receiver();

            CngKey key = CngKey.Import(receiver.ReceiverPublicKey, CngKeyBlobFormat.EccPublicBlob);
            byte[] senderKey = ecd.DeriveKeyMaterial(CngKey.Import(receiver.ReceiverPublicKey, CngKeyBlobFormat.EccPublicBlob));
            Send(senderKey, message, out byte[] encryptedMessage, out byte[] IV);
            
            receiver.Receive(encryptedMessage, IV);
         }
    }

    public static void Send(byte[] key, string secretMessage, out byte[] encryptedMessage, out byte[] IV)
    {
        WriteLine(Environment.NewLine + "Sending message...");

        using (Aes aes = new AesCryptoServiceProvider())
        {
            aes.Key = key;
            IV = aes.IV;

            // Encrypting message
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                byte[] plainTextMessage = Encoding.UTF8.GetBytes(secretMessage);
                cs.Write(plainTextMessage, 0, plainTextMessage.Length);
                cs.Close();
                encryptedMessage = ms.ToArray();
            }
        }
    }
}

public class Receiver
{
    public byte[] ReceiverPublicKey;
    private byte[] Key;

    public Receiver()
    {
        using (ECDiffieHellmanCng ecd = new ECDiffieHellmanCng())
        {
            ecd.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
            ecd.HashAlgorithm = CngAlgorithm.Sha256;
            ReceiverPublicKey = ecd.PublicKey.ToByteArray();
            Key = ecd.DeriveKeyMaterial(CngKey.Import(Sender.SenderPublicKey, CngKeyBlobFormat.EccPublicBlob));

        }

        WriteLine(Environment.NewLine + "Encrypted message: " + Environment.NewLine);

        foreach (byte b in Key)
        {
            Write($"{b}, ");
        }
    }

    public void Receive (byte[] encryptedMessage, byte[] IV)
    {
        WriteLine("Receiving message: ");

        using (Aes aes = new AesCryptoServiceProvider())
        {
            aes.Key = Key;
            aes.IV = IV;

            // Decrypt and show message
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(encryptedMessage, 0, encryptedMessage.Length);
                    cs.Close();

                    string message = Encoding.UTF8.GetString(ms.ToArray());
                    WriteLine(Environment.NewLine + "Decrypted message: ");
                    WriteLine(Environment.NewLine + message + Environment.NewLine);
                }
            }

            WriteLine(Environment.NewLine + "Press any key to close");
            ReadKey();

        }
    }
}

