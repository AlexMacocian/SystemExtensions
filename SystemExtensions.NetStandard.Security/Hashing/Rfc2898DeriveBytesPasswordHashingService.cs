using System.Security.Cryptography;
using System.Threading.Tasks;

namespace System.Hashing
{
    public sealed class Rfc2898DeriveBytesPasswordHashingService : IPasswordHashingService
    {
        private const int MinimumIterations = 10000;
        private const int MinimumHashLength = 32;

        public Rfc2898DeriveBytesPasswordHashingService()
        {
        }

        public async Task<string> Hash(string raw, string salt, int length, int iterations)
        {
            this.ValidateHashArguments(raw, salt, length, iterations);

            var rawBytes = Convert.FromBase64String(raw);
            var saltBytes = Convert.FromBase64String(salt);
            var hashBytes = await this.HashInternal(rawBytes, saltBytes, length, iterations);
            return Convert.ToBase64String(hashBytes);
        }
        public Task<byte[]> Hash(byte[] raw, byte[] salt, int length, int iterations)
        {
            this.ValidateHashArguments(raw, salt, length, iterations);

            return this.HashInternal(raw, salt, length, iterations);
        }
        public Task<bool> VerifyPassword(string hash, string password, string salt, int iterations)
        {
            this.ValidateVerifyArguments(hash, password, salt, iterations);
            var hashBytes = Convert.FromBase64String(hash);
            var saltBytes = Convert.FromBase64String(salt);
            var passwordBytes = Convert.FromBase64String(password);
            return this.VerifyPasswordInternal(hashBytes, passwordBytes, saltBytes, iterations);
        }
        public Task<bool> VerifyPassword(byte[] hash, byte[] password, byte[] salt, int iterations)
        {
            this.ValidateVerifyArguments(hash, password, salt, iterations);
            return this.VerifyPasswordInternal(hash, password, salt, iterations);
        }

        private void ValidateHashArguments(object raw, object salt, int length, int iterations)
        {
            _ = raw ?? throw new ArgumentNullException(nameof(raw));
            _ = salt ?? throw new ArgumentNullException(nameof(salt));

            if (iterations < MinimumIterations)
            {
                throw new InvalidOperationException($"Unable to perform secure hash. Iteration count must be over {MinimumIterations}");
            }

            if (length < MinimumHashLength)
            {
                throw new InvalidOperationException($"Unable to perform secure hash. Hash length must be over {MinimumHashLength}");
            }
        }
        private void ValidateVerifyArguments(object hash, object password, object salt, int iterations)
        {
            _ = hash ?? throw new ArgumentNullException(nameof(hash));
            _ = salt ?? throw new ArgumentNullException(nameof(salt));
            _ = password ?? throw new ArgumentNullException(nameof(password));

            if (iterations < MinimumIterations)
            {
                throw new InvalidOperationException($"Unable to verify hash. Iteration count must be over {MinimumIterations}");
            }
        }
        private Task<byte[]> HashInternal(byte[] raw, byte[] salt, int length, int iterations)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(raw, salt, iterations);
            var hash = pbkdf2.GetBytes(length);
            return Task.FromResult(hash);
        }
        private Task<bool> VerifyPasswordInternal(byte[] hash, byte[] password, byte[] salt, int iterations)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            var hashToVerify = pbkdf2.GetBytes(hash.Length);
            var lengthToVerify = Math.Max(hash.Length, hashToVerify.Length);
            var match = true;
            if (lengthToVerify <= 0)
            {
                return Task.FromResult(false);
            }

            for(var i = 0; i < lengthToVerify; i++)
            {
                if (i < hash.Length && i < hashToVerify.Length)
                {
                    if (hashToVerify[i].Equals(hash[i]) is false)
                    {
                        match = false;
                    }
                }
            }

            return Task.FromResult(match && hash.Length.Equals(hashToVerify.Length));
        }
    }
}
