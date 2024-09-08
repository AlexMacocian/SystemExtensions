using System.Security.Cryptography;

namespace System.Security.Hashing;
public sealed class MD5HashingService : BaseHashingService<MD5>, IMD5HashingService
{
    protected override MD5 GetHashAlgorithm()
    {
        return MD5.Create();
    }
}
