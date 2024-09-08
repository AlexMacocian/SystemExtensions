using System.Security.Cryptography;

namespace System.Security.Hashing;
public sealed class HMACSha256Service : BaseHMACService<HMACSHA256>, IHMACSha256Service
{
    protected override HMACSHA256 GetHashAlgorithm(byte[] key)
    {
        return new HMACSHA256(key);
    }
}
