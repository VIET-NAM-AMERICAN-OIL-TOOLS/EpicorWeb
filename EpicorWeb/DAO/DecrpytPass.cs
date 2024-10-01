using System.Security.Cryptography;
using System.Text;

namespace EpicorWeb.DAO
{
    public class DecrpytPass
    {
        public string ComputeHash(string password, byte[]? saltBytes = null)
        {

            if (saltBytes == null)
            {
                saltBytes = new byte[8];
                new RNGCryptoServiceProvider().GetNonZeroBytes(saltBytes);
            }

            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] buffer = new byte[bytes.Length + saltBytes.Length];
            for (int i = 0; i < bytes.Length; i++)
                buffer[i] = bytes[i];

            for (int j = 0; j < saltBytes.Length; j++)
                buffer[bytes.Length + j] = saltBytes[j];

            byte[] buffer2 = SHA256.HashData(buffer);
            byte[] combinedArr = new byte[buffer2.Length + saltBytes.Length];
            for (int k = 0; k < buffer2.Length; k++)
                combinedArr[k] = buffer2[k];

            for (int m = 0; m < saltBytes.Length; m++)
                combinedArr[buffer2.Length + m] = saltBytes[m];

            return Convert.ToBase64String(combinedArr);
        }

        public bool VerifyHash(string password, string hash)
        {
            byte[] saltBytes = new byte[8];
            byte[] buffer = Convert.FromBase64String(hash);
            int x = 0x100 / 8; // SHA256

            if (buffer.Length < x)
                return false;

            for (int i = 0; i < saltBytes.Length; i++)
                saltBytes[i] = buffer[x + i];

            return (hash == ComputeHash(password, saltBytes));
        }
    }
}
