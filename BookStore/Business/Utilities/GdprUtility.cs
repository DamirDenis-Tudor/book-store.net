using System.Security.Cryptography;
using System.Text;

namespace Business.Utilities;

public static class GdprUtility
{
    private static readonly byte[] Key = Convert.FromBase64String("y+4VFip9Kiav9xWAic9PPA==");
    private static readonly byte[] Iv = Convert.FromBase64String("AQIDBAUGBwgJCgsMDQ4PEA==");
    
    
    public static string Hash(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        var builder = new StringBuilder();

        foreach (var b in bytes)
        {
            builder.Append(b.ToString("x2"));
        }

        return builder.ToString();
    }

    public static string Encrypt(string input)
    {
        using var aes = Aes.Create();
        aes.Key = Key;
        aes.IV = Iv;
        
        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        using (var streamWriter = new StreamWriter(cryptoStream))
        {
            streamWriter.Write(input);
        }

        return Convert.ToBase64String(memoryStream.ToArray());
    }
    
    public static string Decrypt(string input)
    {
        using var aes = Aes.Create();
        aes.Key = Key;
        aes.IV = Iv;
        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using var memoryStream = new MemoryStream(Convert.FromBase64String(input));
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream);
        
        return streamReader.ReadToEnd();
    }
}