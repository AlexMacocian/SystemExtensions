using System.IO;
using System.Threading.Tasks;

namespace SystemExtensions.NetStandard.Security.Encryption
{
    public interface ISymmetricEncrypter
    {
        Stream GetEncryptionStream(string key, string iv, Stream outStream);
        Stream GetEncryptionStream(byte[] key, byte[] iv, Stream outStream);
        Stream GetDecryptionStream(string key, string iv, Stream inStream);
        Stream GetDecryptionStream(byte[] key, byte[] iv, Stream inStream);
        Task<string> Encrypt(string key, string iv, string value);
        Task<string> Encrypt(byte[] key, byte[] iv, string value);
        Task<byte[]> Encrypt(string key, string iv, byte[] value);
        Task<byte[]> Encrypt(byte[] key, byte[] iv, byte[] value);
        Task<Stream> Encrypt(string key, string iv, Stream value);
        Task<Stream> Encrypt(byte[] key, byte[] iv, Stream value);
        Task<string> Decrypt(string key, string iv, string value);
        Task<string> Decrypt(byte[] key, byte[] iv, string value);
        Task<byte[]> Decrypt(string key, string iv, byte[] value);
        Task<byte[]> Decrypt(byte[] key, byte[] iv, byte[] value);
        Task<Stream> Decrypt(string key, string iv, Stream value);
        Task<Stream> Decrypt(byte[] key, byte[] iv, Stream value);
    }
}
