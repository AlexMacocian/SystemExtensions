using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace System.Security.Hashing;
public abstract class BaseHashingService<T>
    where T : HashAlgorithm
{
    protected abstract T GetHashAlgorithm();

    public async Task<string> Hash(string raw)
    {
        using var rawStream = BytesToStream(StringToBytes(raw));
        using var hashedStream = await this.HashInternal(rawStream).ConfigureAwait(false);
        return StreamToString(hashedStream);
    }
    public async Task<byte[]> Hash(byte[] raw)
    {
        using var rawStream = BytesToStream(raw);
        using var hashedStream = await this.HashInternal(rawStream).ConfigureAwait(false);
        return StreamToBytes(hashedStream);
    }
    public async Task<Stream> Hash(Stream raw)
    {
        return await this.HashInternal(raw);
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
    private async Task<Stream> HashInternal(Stream raw)
    {
        if (raw is null)
        {
            throw new ArgumentNullException(nameof(raw));
        }

        using var algo = this.GetHashAlgorithm();
        return await Task.Run(() => new MemoryStream(algo.ComputeHash(raw)));
    }
}
