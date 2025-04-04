using System.Text;

namespace GoAnyway.Framework.Encryption.Algorithms.AES;

public sealed class AesEncryptionToken
{
    public byte[] Key { get; }
    public byte[] Iv { get; }

    private AesEncryptionToken(
        byte[] key,
        byte[] iv)
    {
        Key = key;
        Iv = iv;
    }

    public static AesEncryptionToken Read(string token)
    {
        var parts = token.Split('-');
        if (parts.Length < 2)
            throw new ArgumentException("Invalid token format.");

        return new(
            key: Decode(parts[0]),
            iv: Decode(parts[1])
        );
    }

    private static byte[] Decode(string encoded)
    {
        return Encoding.UTF8.GetBytes(encoded);
    }
}