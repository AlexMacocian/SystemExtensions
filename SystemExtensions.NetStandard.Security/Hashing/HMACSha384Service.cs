using System.Security.Cryptography;

namespace System.Security.Hashing;
public sealed class HMACSha384Service : BaseHMACService<HMACSHA384>, IHMACSha384Service
{
    protected override HMACSHA384 GetHashAlgorithm(byte[] key)
    {
        return new HMACSHA384(key);
    }
}
