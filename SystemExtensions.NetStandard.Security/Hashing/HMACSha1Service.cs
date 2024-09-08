using System.Security.Cryptography;

namespace System.Security.Hashing;
public sealed class HMACSha1Service : BaseHMACService<HMACSHA1>, IHMACSha1Service
{
    protected override HMACSHA1 GetHashAlgorithm(byte[] key)
    {
        return new HMACSHA1(key);
    }
}
