using System.Security.Cryptography;
using System.Text;

namespace NArtadoSearch.Core.Security.Encryption;

public class AesEncryption
{
    private const int NonceSize = 12;
    private const int TagSize = 16;

    public static (byte[] cipherText, byte[] nonce) Encrypt(string plainText, byte[] key)
    {
        using var aes = new AesGcm(key);
        byte[] nonce = new byte[NonceSize];
        RandomNumberGenerator.Fill(nonce);
        
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] cipherText = new byte[plainTextBytes.Length];
        byte[] tag = new byte[TagSize];

        aes.Encrypt(nonce, plainTextBytes, cipherText, tag);

        return (Combine(cipherText, tag), nonce);
    }

    public static string Decrypt(byte[] cipherTextWithTag, byte[] nonce, byte[] key)
    {
        using var aes = new AesGcm(key);
        
        byte[] tag = new byte[TagSize];
        byte[] cipherText = new byte[cipherTextWithTag.Length - TagSize];
        
        Array.Copy(cipherTextWithTag, 0, cipherText, 0, cipherText.Length);
        Array.Copy(cipherTextWithTag, cipherText.Length, tag, 0, tag.Length);

        byte[] plainTextBytes = new byte[cipherText.Length];
        
        aes.Decrypt(nonce, cipherText, tag, plainTextBytes);

        return Encoding.UTF8.GetString(plainTextBytes);
    }
    
    private static byte[] Combine(byte[] cipherText, byte[] tag)
    {
        byte[] combined = new byte[cipherText.Length + tag.Length];
        Array.Copy(cipherText, combined, cipherText.Length);
        Array.Copy(tag, 0, combined, cipherText.Length, tag.Length);
        return combined;
    }
}