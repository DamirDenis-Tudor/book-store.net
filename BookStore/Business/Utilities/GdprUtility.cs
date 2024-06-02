/**************************************************************************
 *                                                                        *
 *  Description: GDPR Utility                                             *
 *  Website:     https://github.com/DamirDenis-Tudor/PetShop-ProiectIP    *
 *  Copyright:   (c) 2024, Damir Denis-Tudor                              *
 *                                                                        *
 *  This code and information is provided "as is" without warranty of     *
 *  any kind, either expressed or implied, including but not limited      *
 *  to the implied warranties of merchantability or fitness for a         *
 *  particular purpose. You are free to use this source code in your      *
 *  applications as long as the original copyright notice is included.    *
 *                                                                        *
 **************************************************************************/

using System.Security.Cryptography;
using System.Text;

namespace Business.Utilities;

/// <summary>
/// Utility class for providing GDPR (General Data Protection Regulation) compliant operations.
/// </summary>
internal static class GdprUtility
{
    private static readonly byte[] Iv = Convert.FromBase64String("AQIDBAUGBwgJCgsMDQ4PEA==");
        
    /// <summary>
    /// Computes the SHA-256 hash of the input string.
    /// </summary>
    /// <param name="input">The input string to be hashed.</param>
    /// <returns>The SHA-256 hash of the input string.</returns>
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

    /// <summary>
    /// Encrypts the input string using AES encryption.
    /// </summary>
    /// <param name="input">The input string to be encrypted.</param>
    /// <param name="key">Encryption key.</param>
    /// <returns>The Base64 encoded encrypted string.</returns>
    public static string Encrypt(string input, string key)
    {
        using var aes = Aes.Create();
        
        var sizedKey = new byte[16];
        Array.Copy(Convert.FromBase64String(key),sizedKey , 16);
        
        aes.Key = sizedKey;
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
        
    /// <summary>
    /// Decrypts the input string using AES decryption.
    /// </summary>
    /// <param name="input">The Base64 encoded encrypted string.</param>
    /// <param name="key">Decryption key.</param>
    /// <returns>The decrypted string.</returns>
    public static string Decrypt(string input,string key)
    {
        using var aes = Aes.Create();
        
        var sizedKey = new byte[16];
        Array.Copy(Convert.FromBase64String(key),sizedKey , 16);
        
        aes.Key = sizedKey;
        aes.IV = Iv;
        
        var decrypt = aes.CreateDecryptor(aes.Key, aes.IV);

        using var memoryStream = new MemoryStream(Convert.FromBase64String(input));
        using var cryptoStream = new CryptoStream(memoryStream, decrypt, CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream);
            
        return streamReader.ReadToEnd();
    }
}