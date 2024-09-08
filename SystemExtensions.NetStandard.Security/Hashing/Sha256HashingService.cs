using System.Hashing;
using System.Security.Cryptography;

namespace System.Security.Hashing;

public sealed class Sha256HashingService : BaseHashingService<SHA256>, ISha256HashingService
{
    protected override SHA256 GetHashAlgorithm() => SHA256.Create();
}
