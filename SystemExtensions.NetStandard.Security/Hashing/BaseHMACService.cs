using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace System.Security.Hashing;
public abstract class BaseHMACService<T> : IHMACService
    where T : HMAC
{
    protected abstract T GetHashAlgorithm(byte[] key);

    public async Task<string> Hash(string key, string raw)
    {
        using var rawStream = BytesToStream(StringToBytes(raw));
        var keyBytes = StringToBytes(key);
        using var hashedStream = await this.HashInternal(keyBytes, rawStream).ConfigureAwait(false);
        return StreamToString(hashedStream);
    }
    public async Task<byte[]> Hash(byte[] key, byte[] raw)
    {
        using var rawStream = BytesToStream(raw);
        using var hashedStream = await this.HashInternal(key, rawStream).ConfigureAwait(false);
        return StreamToBytes(hashedStream);
    }
    public async Task<Stream> Hash(byte[] key, Stream raw)
    {
        return await this.HashInternal(key, raw);
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
    private async Task<Stream> HashInternal(byte[] key, Stream raw)
    {
        if (raw is null)
        {
            throw new ArgumentNullException(nameof(raw));
        }

        using var algo = this.GetHashAlgorithm(key);
        return await Task.Run(() => new MemoryStream(algo.ComputeHash(raw)));
    }
}
