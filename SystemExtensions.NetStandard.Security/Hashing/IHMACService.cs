using System.IO;
using System.Threading.Tasks;

namespace System.Security.Hashing;
public interface IHMACService
{
    Task<string> Hash(string key, string raw);
    Task<byte[]> Hash(byte[] key, byte[] raw);
    Task<Stream> Hash(byte[] key, Stream raw);
}
