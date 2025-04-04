using System.Security.Cryptography;

namespace GoAnyway.Framework.Encryption.Algorithms.AES;

public sealed class AesCbcPkcs7EncryptionAlgorithm : IEncryptionAlgorithm
{
    private readonly Aes _aes;

    private AesCbcPkcs7EncryptionAlgorithm(Aes aes)
    {
        _aes = aes;
    }

    public static AesCbcPkcs7EncryptionAlgorithm FromToken(AesEncryptionToken token)
    {
        var aes = Aes.Create();
        aes.KeySize = 256;
        aes.BlockSize = 128;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.Key = token.Key;
        aes.IV = token.Iv;

        return new(aes);
    }

    public ReadOnlyMemory<byte> Encrypt(byte[] data)
    {
        using var encryptor = _aes.CreateEncryptor();
        var cipherBytes = encryptor.TransformFinalBlock(data, 0, data.Length);

        return cipherBytes;
    }

    public void Dispose()
    {
        _aes.Dispose();
    }
}