using System.Text;
using System.Security.Cryptography;

namespace WebApi.Tools.Hasher
{
    public class SecurityHasher
    {
        public string GetSha256Hash(string value)
        {
            var algoritm = new SHA256CryptoServiceProvider();
            var byteValue = Encoding.UTF8.GetBytes(value);
            var byteHash = algoritm.ComputeHash(byteValue);
            return Convert.ToBase64String(byteHash);
        }
    }
}
