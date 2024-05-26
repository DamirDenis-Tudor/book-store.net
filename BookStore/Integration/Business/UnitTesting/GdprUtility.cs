namespace UnitTesting.Business.UnitTesting;

public class GdprUtility
{
    [Test]
    public void EncryptDecrypt()
    {
        var value = "test_1234";

        var encrypted = global::Business.Utilities.GdprUtility.Encrypt(value);
        
        Console.WriteLine(encrypted);
        var decrypted = global::Business.Utilities.GdprUtility.Decrypt(encrypted);
        Console.WriteLine(decrypted);
        Assert.That(decrypted, Is.EqualTo(value));
    }
    
    [Test]
    public void HashPassword()
    {
        const string value = "parola_mea";

        var hashed = global::Business.Utilities.GdprUtility.Hash(value);

        Assert.That(hashed, Is.EqualTo(global::Business.Utilities.GdprUtility.Hash(value)));
    }
}