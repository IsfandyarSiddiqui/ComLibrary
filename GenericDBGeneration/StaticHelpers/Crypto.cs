using System.Security.Cryptography;
using System.Text;

namespace GenericDBGeneration.Utils;

public static class Crypto
{
    public static string sha256(string str)
    {
        using var sha256Generator = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(str);
        var hash = sha256Generator.ComputeHash(bytes);
        return Convert.ToHexString(hash);
    }
}
