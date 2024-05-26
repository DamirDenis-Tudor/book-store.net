using Business.Utilities;
using Common;

namespace UnitTesting.GdprUtilityTesting;

public class Operations
{
    [Test]
    public void EncryptDecrypt()
    {
        var value = "test_1234";

        var encrypted = GdprUtility.Encrypt(value);
        
        Console.WriteLine(encrypted);
        var decrypted = GdprUtility.Decrypt(encrypted);
        Console.WriteLine(decrypted);
        Assert.That(decrypted, Is.EqualTo(value));
    }
    
    [Test]
    public void HashPassword()
    {
        const string value = "parola_mea";

        var hashed = GdprUtility.Hash(value);

        Assert.That(hashed, Is.EqualTo(GdprUtility.Hash(value)));
    }
}