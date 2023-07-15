using System;
using System.Security.Cryptography;
using System.Text;

namespace Construmart.Core.Commons.Utils
{
    public class EncryptionUtility : IEncryptionUtility
    {
        private readonly IApplicationUtility _applicationUtility;

        public EncryptionUtility(IApplicationUtility applicationUtility)
        {
            _applicationUtility = applicationUtility;
        }

        private static byte[] GenerateSalt()
        {
            var randomNumber = new byte[32];
            using var randomNumberGenerator = new RNGCryptoServiceProvider();
            randomNumberGenerator.GetBytes(randomNumber);
            return randomNumber;
        }

        public string HashWithSalt(string rawText, string salt = null)
        {
            var saltBytes = !string.IsNullOrEmpty(salt) ? Convert.FromBase64String(salt) : GenerateSalt();
            var toBeHashed = Encoding.UTF8.GetBytes(rawText);
            using var rfc2898 = new Rfc2898DeriveBytes(toBeHashed, saltBytes, 10000);
            var resultBytes = rfc2898.GetBytes(32);
            var hashString = $"{Convert.ToBase64String(resultBytes)}|{Convert.ToBase64String((byte[])saltBytes)}";
            return hashString;
        }

        public string ComputeHmacSha512(string dataToBeHashed, string key = null)
        {
            var dataBytes = Encoding.UTF8.GetBytes(dataToBeHashed);
            var hashKeyBytes = key == null ? Encoding.UTF8.GetBytes(string.Empty) : Encoding.UTF8.GetBytes(key);
            using var hmac = new HMACSHA512(hashKeyBytes);
            var resultBytes = hmac.ComputeHash(dataBytes);
            var resultString = _applicationUtility.ConvertByteArrayToString(resultBytes);
            return resultString;
        }

        public string ComputeSHA512(string data)
        {
            var byteData = Encoding.UTF8.GetBytes(data);
            using var sha512managed = new SHA512Managed();
            var hashedBytes = sha512managed.ComputeHash(byteData);
            string hash = _applicationUtility.ConvertByteArrayToString(hashedBytes);
            return hash;
        }
    }
}