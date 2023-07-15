using System.Text;

namespace Construmart.Core.Commons.Utils
{
    public class ApplicationUtility : IApplicationUtility
    {
        public string ConvertByteArrayToString(byte[] bytes)
        {
            StringBuilder builder = new();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            var result = builder.ToString();
            return result;
        }
    }
}