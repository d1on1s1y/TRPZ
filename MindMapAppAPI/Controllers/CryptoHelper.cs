using System.Security.Cryptography;
using System.Text;

namespace MindMapApp.Server.Helpers
{
    public static class CryptoHelper
    {
           //хардкод ключа шифрування
        private static readonly string Key = "E546C8DF278CD5931069B522E695D4F2";
        public static string Decrypt(string cipherText)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(Key);
                aes.IV = new byte[16];
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var ms = new MemoryStream(Convert.FromBase64String(cipherText)))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}