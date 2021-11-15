using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Encryption;
using System.Text;
using System.Threading.Tasks;

namespace SystemExtensions.NetStandard.Security.Tests
{
    [TestClass]
    public class AesEncrypterTests
    {
        private ISymmetricEncrypter symmetricEncrypter;
        private string keyString;
        private byte[] keyBytes;
        private byte[] ivBytes;
        private string ivString;
        private string toEncryptString;
        private byte[] toEncryptBytes;

        [TestInitialize]
        public void TestInitialize()
        {
            this.symmetricEncrypter = new AesEncrypter();
            this.keyBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2 };
            this.keyString = Convert.ToBase64String(this.keyBytes);
            this.ivBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6 };
            this.ivString = Convert.ToBase64String(this.ivBytes);
            this.toEncryptBytes = Encoding.UTF8.GetBytes("toEncrypt");
            this.toEncryptString = Convert.ToBase64String(this.toEncryptBytes);
        }

        [TestMethod]
        public async Task EncryptDecryptStringWithStringKeyStringIv()
        {
            var encrypted = await this.symmetricEncrypter.Encrypt(this.keyString, this.ivString, this.toEncryptString).ConfigureAwait(false);
            var decrypted = await this.symmetricEncrypter.Decrypt(this.keyString, this.ivString, encrypted).ConfigureAwait(false);

            this.toEncryptString.Should().Be(decrypted);
        }

        [TestMethod]
        public async Task EncryptDecryptStringWithByteKeyByteIv()
        {
            var encrypted = await this.symmetricEncrypter.Encrypt(this.keyBytes, this.ivBytes, this.toEncryptString).ConfigureAwait(false);
            var decrypted = await this.symmetricEncrypter.Decrypt(this.keyBytes, this.ivBytes, encrypted).ConfigureAwait(false);

            this.toEncryptString.Should().Be(decrypted);
        }

        [TestMethod]
        public async Task EncryptDecryptByteWithStringKeyStringIv()
        {
            var encrypted = await this.symmetricEncrypter.Encrypt(this.keyString, this.ivString, this.toEncryptBytes).ConfigureAwait(false);
            var decrypted = await this.symmetricEncrypter.Decrypt(this.keyString, this.ivString, encrypted).ConfigureAwait(false);

            this.toEncryptBytes.Should().BeEquivalentTo(decrypted);
        }

        [TestMethod]
        public async Task EncryptDecryptByteWithByteKeyByteIv()
        {
            var encrypted = await this.symmetricEncrypter.Encrypt(this.keyBytes, this.ivBytes, this.toEncryptBytes).ConfigureAwait(false);
            var decrypted = await this.symmetricEncrypter.Decrypt(this.keyBytes, this.ivBytes, encrypted).ConfigureAwait(false);

            this.toEncryptBytes.Should().BeEquivalentTo(decrypted);
        }

        [TestMethod]
        public async Task EncryptDecryptStreamWithStringKeyStringIv()
        {
            var encrypted = await this.symmetricEncrypter.Encrypt(this.keyString, this.ivString, new MemoryStream(this.toEncryptBytes)).ConfigureAwait(false);
            encrypted.Position = 0;
            var decrypted = await this.symmetricEncrypter.Decrypt(this.keyString, this.ivString, encrypted).ConfigureAwait(false);

            this.toEncryptBytes.Should().BeEquivalentTo(((MemoryStream)decrypted).ToArray());
        }

        [TestMethod]
        public async Task EncryptDecryptStreamWithByteKeyByteIv()
        {
            var encrypted = await this.symmetricEncrypter.Encrypt(this.keyBytes, this.ivBytes, new MemoryStream(this.toEncryptBytes)).ConfigureAwait(false);
            encrypted.Position = 0;
            var decrypted = await this.symmetricEncrypter.Decrypt(this.keyBytes, this.ivBytes, encrypted).ConfigureAwait(false);

            this.toEncryptBytes.Should().BeEquivalentTo(((MemoryStream)decrypted).ToArray());
        }

        [TestMethod]
        public void GetEncryptionStreamReturnsCryptoStream()
        {
            var encryptionStream = this.symmetricEncrypter.GetEncryptionStream(this.keyBytes, this.ivBytes, new MemoryStream());
            encryptionStream.Should().BeAssignableTo<CryptoStream>();
        }

        [TestMethod]
        public void GetDecryptionStreamReturnsCryptoStream()
        {
            var encryptionStream = this.symmetricEncrypter.GetDecryptionStream(this.keyBytes, this.ivBytes, new MemoryStream());
            encryptionStream.Should().BeAssignableTo<CryptoStream>();
        }
    }
}
