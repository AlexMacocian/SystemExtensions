using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Hashing;
using System.Threading.Tasks;

namespace SystemExtensions.NetStandard.Security.Tests
{
    [TestClass]
    public sealed class Rfc2898DeriveBytesPasswordHashingServiceTests
    {
        private const int TooShortHashLength = 31;
        private const int DesiredHashLength = 32;
        private const int Iterations = 10000;
        private const int TooLittleIterations = 1000;

        private readonly Rfc2898DeriveBytesPasswordHashingService rfc2898DeriveBytesPasswordHashingService = new();

        private byte[] tooShortSaltBytes;
        private string tooShortSaltString;
        private byte[] incorrectSaltBytes;
        private string incorrectSaltString;
        private byte[] saltBytes;
        private string saltString;
        private byte[] passwordBytes;
        private string passwordString;
        private byte[] incorrectPasswordBytes;
        private string incorrectPasswordString;

        [TestInitialize]
        public void TestInitialize()
        {
            this.tooShortSaltBytes = new byte[16];
            this.tooShortSaltString = Convert.ToBase64String(this.tooShortSaltBytes);
            this.saltBytes = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };
            this.incorrectSaltBytes = new byte[32];
            this.incorrectSaltString = Convert.ToBase64String(this.incorrectSaltBytes);
            this.saltString = Convert.ToBase64String(this.saltBytes);
            this.passwordBytes = new byte[] { 5, 1, 2, 34, 35, 123, 4, 23, 1, 235, 32, 234 };
            this.passwordString = Convert.ToBase64String(this.passwordBytes);
            this.incorrectPasswordBytes = new byte[] { 14, 123, 23, 4, 2, 1, 23, 25 };
            this.incorrectPasswordString = Convert.ToBase64String(this.incorrectPasswordBytes);
        }

        [TestMethod]
        public void PasswordNull_HashBytes_Throws_ArgumentNullException()
        {
            var action = new Func<Task<byte[]>>(() => this.rfc2898DeriveBytesPasswordHashingService.Hash(null, this.saltBytes, DesiredHashLength, Iterations));

            action.Should().Throw<ArgumentNullException>();
        }
        [TestMethod]
        public void PasswordNull_HashString_Throws_ArgumentNullException()
        {
            var action = new Func<Task<string>>(() => this.rfc2898DeriveBytesPasswordHashingService.Hash(null, this.saltString, DesiredHashLength, Iterations));

            action.Should().Throw<ArgumentNullException>();
        }
        [TestMethod]
        public void SaltNull_HashBytes_Throws_ArgumentNullException()
        {
            var action = new Func<Task<byte[]>>(() => this.rfc2898DeriveBytesPasswordHashingService.Hash(this.passwordBytes, null, DesiredHashLength, Iterations));

            action.Should().Throw<ArgumentNullException>();
        }
        [TestMethod]
        public void SaltNull_HashString_Throws_ArgumentNullException()
        {
            var action = new Func<Task<string>>(() => this.rfc2898DeriveBytesPasswordHashingService.Hash(this.passwordString, null, DesiredHashLength, Iterations));

            action.Should().Throw<ArgumentNullException>();
        }
        [TestMethod]
        public void HashLengthTooSmall_HashBytes_Throws_InvalidOperationException()
        {
            var action = new Func<Task<byte[]>>(() => this.rfc2898DeriveBytesPasswordHashingService.Hash(this.passwordBytes, this.saltBytes, TooShortHashLength, Iterations));

            action.Should().Throw<InvalidOperationException>();
        }
        [TestMethod]
        public void HashLengthTooSmall_HashString_Throws_InvalidOperationException()
        {
            var action = new Func<Task<string>>(() => this.rfc2898DeriveBytesPasswordHashingService.Hash(this.passwordString, this.saltString, TooShortHashLength, Iterations));

            action.Should().Throw<InvalidOperationException>();
        }
        [TestMethod]
        public void TooLittleIterations_HashBytes_Throws_InvalidOperationException()
        {
            var action = new Func<Task<byte[]>>(() => this.rfc2898DeriveBytesPasswordHashingService.Hash(this.passwordBytes, this.saltBytes, DesiredHashLength, TooLittleIterations));

            action.Should().Throw<InvalidOperationException>();
        }
        [TestMethod]
        public void TooLittleIterations_HashString_Throws_InvalidOperationException()
        {
            var action = new Func<Task<string>>(() => this.rfc2898DeriveBytesPasswordHashingService.Hash(this.passwordString, this.saltString, DesiredHashLength, TooLittleIterations));

            action.Should().Throw<InvalidOperationException>();
        }
        [TestMethod]
        public void TooShortSalt_HashBytes_Throws_InvalidOperationException()
        {
            var action = new Func<Task<byte[]>>(() => this.rfc2898DeriveBytesPasswordHashingService.Hash(this.passwordBytes, this.tooShortSaltBytes, DesiredHashLength, TooLittleIterations));

            action.Should().Throw<InvalidOperationException>();
        }
        [TestMethod]
        public void TooShortSalt_HashString_Throws_InvalidOperationException()
        {
            var action = new Func<Task<string>>(() => this.rfc2898DeriveBytesPasswordHashingService.Hash(this.passwordString, this.tooShortSaltString, DesiredHashLength, TooLittleIterations));

            action.Should().Throw<InvalidOperationException>();
        }
        [TestMethod]
        public void PasswordNull_VerifyBytes_Throws_ArgumentNullException()
        {
            var action = new Func<Task<bool>>(() => this.rfc2898DeriveBytesPasswordHashingService.VerifyPassword(null, this.incorrectPasswordBytes, this.saltBytes, Iterations));

            action.Should().Throw<ArgumentNullException>();
        }
        [TestMethod]
        public void PasswordNull_VerifyString_Throws_ArgumentNullException()
        {
            var action = new Func<Task<bool>>(() => this.rfc2898DeriveBytesPasswordHashingService.VerifyPassword(null, this.incorrectPasswordString, this.saltString, Iterations));

            action.Should().Throw<ArgumentNullException>();
        }
        [TestMethod]
        public void HashNull_VerifyBytes_Throws_ArgumentNullException()
        {
            var action = new Func<Task<bool>>(() => this.rfc2898DeriveBytesPasswordHashingService.VerifyPassword(this.passwordBytes, null, this.saltBytes, Iterations));

            action.Should().Throw<ArgumentNullException>();
        }
        [TestMethod]
        public void HashNull_VerifyString_Throws_ArgumentNullException()
        {
            var action = new Func<Task<bool>>(() => this.rfc2898DeriveBytesPasswordHashingService.VerifyPassword(this.passwordString, null, this.saltString, Iterations));

            action.Should().Throw<ArgumentNullException>();
        }
        [TestMethod]
        public void SaltNull_VerifyBytes_Throws_ArgumentNullException()
        {
            var action = new Func<Task<bool>>(() => this.rfc2898DeriveBytesPasswordHashingService.VerifyPassword(this.passwordBytes, this.incorrectPasswordBytes, null, Iterations));

            action.Should().Throw<ArgumentNullException>();
        }
        [TestMethod]
        public void SaltNull_VerifyString_Throws_ArgumentNullException()
        {
            var action = new Func<Task<bool>>(() => this.rfc2898DeriveBytesPasswordHashingService.VerifyPassword(this.passwordString, this.incorrectPasswordString, null, Iterations));

            action.Should().Throw<ArgumentNullException>();
        }
        [TestMethod]
        public void TooLittleIterations_VerifyBytes_Throws_InvalidOperationException()
        {
            var action = new Func<Task<bool>>(() => this.rfc2898DeriveBytesPasswordHashingService.VerifyPassword(this.passwordBytes, this.incorrectPasswordBytes, this.saltBytes, TooLittleIterations));

            action.Should().Throw<InvalidOperationException>();
        }
        [TestMethod]
        public void TooLittleIterations_VerifyString_Throws_InvalidOperationException()
        {
            var action = new Func<Task<bool>>(() => this.rfc2898DeriveBytesPasswordHashingService.VerifyPassword(this.passwordString, this.incorrectPasswordString, this.saltString, TooLittleIterations));

            action.Should().Throw<InvalidOperationException>();
        }
        [TestMethod]
        public async Task HashBytes_ReturnsHashedBytes()
        {
            var hashedBytes = await this.rfc2898DeriveBytesPasswordHashingService.Hash(this.passwordBytes, this.saltBytes, DesiredHashLength, Iterations);

            hashedBytes.Should().NotBeNull();
            hashedBytes.Should().HaveCount(DesiredHashLength);
        }
        [TestMethod]
        public async Task HashString_ReturnsHashedString()
        {
            var hashedString = await this.rfc2898DeriveBytesPasswordHashingService.Hash(this.passwordString, this.saltString, DesiredHashLength, Iterations);

            hashedString.Should().NotBeNull();
            var hashedBytes = Convert.FromBase64String(hashedString);
            hashedBytes.Should().HaveCount(DesiredHashLength);
        }
        [TestMethod]
        public async Task VerifyBytes_CorrectPassword_ReturnsTrue()
        {
            var hashedBytes = await this.rfc2898DeriveBytesPasswordHashingService.Hash(this.passwordBytes, this.saltBytes, DesiredHashLength, Iterations);

            var result = await this.rfc2898DeriveBytesPasswordHashingService.VerifyPassword(hashedBytes, this.passwordBytes, this.saltBytes, Iterations);

            result.Should().BeTrue();
        }
        [TestMethod]
        public async Task VerifyString_CorrectPassword_ReturnsTrue()
        {
            var hashedString = await this.rfc2898DeriveBytesPasswordHashingService.Hash(this.passwordString, this.saltString, DesiredHashLength, Iterations);

            var result = await this.rfc2898DeriveBytesPasswordHashingService.VerifyPassword(hashedString, this.passwordString, this.saltString, Iterations);

            result.Should().BeTrue();
        }
        [TestMethod]
        public async Task VerifyBytes_IncorrectPassword_ReturnsFalse()
        {
            var hashedBytes = await this.rfc2898DeriveBytesPasswordHashingService.Hash(this.passwordBytes, this.saltBytes, DesiredHashLength, Iterations);

            var result = await this.rfc2898DeriveBytesPasswordHashingService.VerifyPassword(hashedBytes, this.incorrectPasswordBytes, this.saltBytes, Iterations);

            result.Should().BeFalse();
        }
        [TestMethod]
        public async Task VerifyString_IncorrectPassword_ReturnsFalse()
        {
            var hashedString = await this.rfc2898DeriveBytesPasswordHashingService.Hash(this.passwordString, this.saltString, DesiredHashLength, Iterations);

            var result = await this.rfc2898DeriveBytesPasswordHashingService.VerifyPassword(hashedString, this.incorrectPasswordString, this.saltString, Iterations);

            result.Should().BeFalse();
        }
        [TestMethod]
        public async Task VerifyBytes_IncorrectSalt_ReturnsFalse()
        {
            var hashedBytes = await this.rfc2898DeriveBytesPasswordHashingService.Hash(this.passwordBytes, this.saltBytes, DesiredHashLength, Iterations);

            var result = await this.rfc2898DeriveBytesPasswordHashingService.VerifyPassword(hashedBytes, this.passwordBytes, this.incorrectSaltBytes, Iterations);

            result.Should().BeFalse();
        }
        [TestMethod]
        public async Task VerifyString_IncorrectSalt_ReturnsFalse()
        {
            var hashedString = await this.rfc2898DeriveBytesPasswordHashingService.Hash(this.passwordString, this.saltString, DesiredHashLength, Iterations);

            var result = await this.rfc2898DeriveBytesPasswordHashingService.VerifyPassword(hashedString, this.passwordString, this.incorrectSaltString, Iterations);

            result.Should().BeFalse();
        }
        [TestMethod]
        public async Task VerifyBytes_IncorrectIterations_ReturnsFalse()
        {
            var hashedBytes = await this.rfc2898DeriveBytesPasswordHashingService.Hash(this.passwordBytes, this.saltBytes, DesiredHashLength, Iterations);

            var result = await this.rfc2898DeriveBytesPasswordHashingService.VerifyPassword(hashedBytes, this.passwordBytes, this.saltBytes, Iterations + 1);

            result.Should().BeFalse();
        }
        [TestMethod]
        public async Task VerifyString_IncorrectIterations_ReturnsFalse()
        {
            var hashedString = await this.rfc2898DeriveBytesPasswordHashingService.Hash(this.passwordString, this.saltString, DesiredHashLength, Iterations);

            var result = await this.rfc2898DeriveBytesPasswordHashingService.VerifyPassword(hashedString, this.passwordString, this.saltString, Iterations + 1);

            result.Should().BeFalse();
        }
    }
}
