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
    private static readonly byte[] Key = Convert.FromBase64String("y+4VFip9Kiav9xWAic9PPA==");
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
    /// <returns>The Base64 encoded encrypted string.</returns>
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
        
    /// <summary>
    /// Decrypts the input string using AES decryption.
    /// </summary>
    /// <param name="input">The Base64 encoded encrypted string.</param>
    /// <returns>The decrypted string.</returns>
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