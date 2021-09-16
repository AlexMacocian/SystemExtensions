using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using SystemExtensions.NetStandard.Security.Utilities;

namespace SystemExtensions.NetStandard.Security.Encryption
{
    public sealed class AesEncrypter : ISymmetricEncrypter
    {
        public async Task<string> Decrypt(string key, string iv, string value)
        {
            using var valueStream = BytesToStream(StringToBytes(value));
            using var decryptedStream = await DecryptInternal(StringToBytes(key), StringToBytes(iv), valueStream).ConfigureAwait(false);
            return StreamToString(decryptedStream);
        }

        public async Task<string> Decrypt(byte[] key, byte[] iv, string value)
        {
            using var valueStream = BytesToStream(StringToBytes(value));
            using var decryptedStream = await DecryptInternal(key, iv, valueStream).ConfigureAwait(false);
            return StreamToString(decryptedStream);
        }

        public async Task<byte[]> Decrypt(string key, string iv, byte[] value)
        {
            using var valueStream = BytesToStream(value);
            using var decryptedStream = await DecryptInternal(StringToBytes(key), StringToBytes(iv), valueStream).ConfigureAwait(false);
            return StreamToBytes(decryptedStream);
        }

        public async Task<byte[]> Decrypt(byte[] key, byte[] iv, byte[] value)
        {
            using var valueStream = BytesToStream(value);
            using var decryptedStream = await DecryptInternal(key, iv, valueStream).ConfigureAwait(false);
            return StreamToBytes(decryptedStream);
        }

        public async Task<Stream> Decrypt(string key, string iv, Stream value)
        {
            return await DecryptInternal(StringToBytes(key), StringToBytes(iv), value).ConfigureAwait(false);
        }

        public async Task<Stream> Decrypt(byte[] key, byte[] iv, Stream value)
        {
            return await DecryptInternal(key, iv, value).ConfigureAwait(false);
        }

        public async Task<string> Encrypt(string key, string iv, string value)
        {
            using var valueStream = BytesToStream(StringToBytes(value));
            using var encryptedStream = await EncryptInternal(StringToBytes(key), StringToBytes(iv), valueStream).ConfigureAwait(false);
            return StreamToString(encryptedStream);
        }

        public async Task<string> Encrypt(byte[] key, byte[] iv, string value)
        {
            using var valueStream = BytesToStream(StringToBytes(value));
            using var encryptedStream = await EncryptInternal(key, iv, valueStream).ConfigureAwait(false);
            return StreamToString(encryptedStream);
        }

        public async Task<byte[]> Encrypt(string key, string iv, byte[] value)
        {
            using var valueStream = BytesToStream(value);
            using var encryptedStream = await EncryptInternal(StringToBytes(key), StringToBytes(iv), valueStream).ConfigureAwait(false);
            return StreamToBytes(encryptedStream);
        }

        public async Task<byte[]> Encrypt(byte[] key, byte[] iv, byte[] value)
        {
            using var valueStream = BytesToStream(value);
            using var encryptedStream = await EncryptInternal(key, iv, valueStream).ConfigureAwait(false);
            return StreamToBytes(encryptedStream);
        }

        public async Task<Stream> Encrypt(string key, string iv, Stream value)
        {
            return value is null
                ? throw new ArgumentNullException(nameof(value))
                : await EncryptInternal(StringToBytes(key), StringToBytes(iv), value).ConfigureAwait(false);
        }

        public async Task<Stream> Encrypt(byte[] key, byte[] iv, Stream value)
        {
            return value is null
                ? throw new ArgumentNullException(nameof(value))
                : await EncryptInternal(key, iv, value).ConfigureAwait(false);
        }

        public Stream GetEncryptionStream(string key, string iv, Stream outStream)
        {
            return GetEncryptionStreamInternal(StringToBytes(key), StringToBytes(iv), outStream);
        }

        public Stream GetEncryptionStream(byte[] key, byte[] iv, Stream outStream)
        {
            return GetEncryptionStreamInternal(key, iv, outStream);
        }

        public Stream GetDecryptionStream(string key, string iv, Stream inStream)
        {
            return GetDecryptionStreamInternal(StringToBytes(key), StringToBytes(iv), inStream);
        }

        public Stream GetDecryptionStream(byte[] key, byte[] iv, Stream inStream)
        {
            return GetDecryptionStreamInternal(key, iv, inStream);
        }

        private static byte[] StreamToBytes(Stream value)
        {
            value.Position = 0;
            var buffer = new byte[1024];
            using var ms = new MemoryStream();
            var read = -1;
            do
            {
                read = value.Read(buffer, 0, buffer.Length);
                ms.Write(buffer, 0, read);
            } while (read > 0);
            return ms.ToArray();
        }

        private static string StreamToString(Stream value)
        {
            return Convert.ToBase64String(StreamToBytes(value));
        }

        private static byte[] StringToBytes(string value)
        {
            return Convert.FromBase64String(value);
        }

        private static Stream BytesToStream(byte[] value)
        {
            return new MemoryStream(value);
        }

        private static async Task<Stream> EncryptInternal(byte[] key, byte[] iv, Stream toEncryptStream)
        {
            if (key.Length < 32)
            {
                throw new ArgumentException($"{nameof(key)} must be at least 32 bytes");
            }

            if (iv.Length < 16)
            {
                throw new ArgumentException($"{nameof(iv)} must be at least 16 bytes");
            }

            key = key.Take(32).ToArray();
            iv = iv.Take(16).ToArray();


            var encryptedMemoryStream = new MemoryStream();
            using var cryptoStream = GetEncryptionStreamInternal(key, iv, encryptedMemoryStream);
            await toEncryptStream.CopyToAsync(cryptoStream).ConfigureAwait(false);
            if (!cryptoStream.HasFlushedFinalBlock)
            {
                cryptoStream.FlushFinalBlock();
            }

            encryptedMemoryStream.Position = 0;
            return encryptedMemoryStream;
        }

        private static async Task<Stream> DecryptInternal(byte[] key, byte[] iv, Stream toDecryptStream)
        {
            if (key.Length < 32)
            {
                throw new ArgumentException($"{nameof(key)} must be at least 32 bytes");
            }

            if (iv.Length < 16)
            {
                throw new ArgumentException($"{nameof(iv)} must be at least 16 bytes");
            }

            key = key.Take(32).ToArray();
            iv = iv.Take(16).ToArray();


            var decryptedMemoryStream = new MemoryStream();
            using var cryptoStream = GetDecryptionStreamInternal(key, iv, toDecryptStream);
            await cryptoStream.CopyToAsync(decryptedMemoryStream).ConfigureAwait(false);
            if (!cryptoStream.HasFlushedFinalBlock)
            {
                cryptoStream.FlushFinalBlock();
            }

            decryptedMemoryStream.Position = 0;
            return decryptedMemoryStream;
        }

        private static CryptoStream GetEncryptionStreamInternal(byte[] key, byte[] iv, Stream encryptedStream)
        {
            using var aes = Aes.Create();
            aes.Padding = PaddingMode.PKCS7;
            var crypto = aes.CreateEncryptor(key, iv);
            var cryptoStream = new NotClosingCryptoStream(encryptedStream, crypto, CryptoStreamMode.Write);
            return cryptoStream;
        }

        private static CryptoStream GetDecryptionStreamInternal(byte[] key, byte[] iv, Stream encryptedStream)
        {
            using var aes = Aes.Create();
            aes.Padding = PaddingMode.PKCS7;
            var crypto = aes.CreateDecryptor(key, iv);
            var cryptoStream = new NotClosingCryptoStream(encryptedStream, crypto, CryptoStreamMode.Read);
            return cryptoStream;
        }
    }
}
