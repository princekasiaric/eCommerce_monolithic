namespace Construmart.Core.Commons
{
    public interface IEncryptionUtility
    {
        string HashWithSalt(string rawText, string salt = null);
        string ComputeHmacSha512(string dataToBeHashed, string key = null);
        string ComputeSHA512(string data);
    }
}