using System.Text;

namespace Service.Authify.Data.Helpers;

public class GenerateKeyHelper
{
    public byte[] GenerateKey(string secretKey)
    {
        return Encoding.ASCII.GetBytes(secretKey);
    }
}