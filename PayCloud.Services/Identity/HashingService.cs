using PayCloud.Services.Identity.Contracts;
using PayCloud.Services.Utils;
using System.Security.Cryptography;
using System.Text;

namespace PayCloud.Services.Identity
{
    public class HashingService : IHashingService
    {
        private byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public string GetHashedString(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                throw new ServiceErrorException(Constants.WrongArguments);
            }
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
