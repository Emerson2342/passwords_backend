namespace passwords_backend.Helper;

using System.Security.Cryptography;
using System.Text;

public class CryptoHelper(IConfiguration configuration)
{

    private readonly string _key = configuration["EncryptionKey"] ?? throw new Exception("Encryption Key not found!");

    public string Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(_key.PadRight(32).Substring(0, 32));
        aes.GenerateIV();

        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(plainText);
        }
        return $"{Convert.ToBase64String(aes.IV)}:{Convert.ToBase64String(ms.ToArray())}";
    }

}