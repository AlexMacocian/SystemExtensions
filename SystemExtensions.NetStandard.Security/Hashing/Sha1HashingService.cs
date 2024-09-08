using System.Security.Cryptography;

namespace System.Security.Hashing;
public sealed class Sha1HashingService : BaseHashingService<SHA1>, ISha1HashingService
{
    protected override SHA1 GetHashAlgorithm() => SHA1.Create();
}
