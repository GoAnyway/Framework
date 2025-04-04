namespace GoAnyway.Framework.Encryption.Algorithms;

public interface IEncryptionAlgorithm : IDisposable
{
    ReadOnlyMemory<byte> Encrypt(byte[] data);
}