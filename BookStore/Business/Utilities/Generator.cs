using System.Security.Cryptography;

namespace Business.Utilities;
internal static class Generator
{
    public static string GetToken(int length = 32)
    {
        var tokenData = new byte[length];
        
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(tokenData);
        }

        return Convert.ToBase64String(tokenData);
    }
}