using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Rng;

namespace SystemExtensions.NetStandard.Security.Tests
{
    [TestClass]
    public class CryptoRngProviderTests
    {
        private CryptoRngProvider cryptoRngProvider;

        [TestInitialize]
        public void TestInitialize()
        {
            this.cryptoRngProvider = new CryptoRngProvider();
        }

        [TestMethod]
        public void GetBytes_ShouldSetValues()
        {
            var bytes = new byte[100];

            this.cryptoRngProvider.GetBytes(bytes);

            bytes.All(b => b == 0).Should().BeFalse();
        }

        [TestMethod]
        public void GetNonZeroBytes_ShouldSetNonZeroValues()
        {
            var bytes = new byte[100];

            this.cryptoRngProvider.GetNonZeroBytes(bytes);

            bytes.All(b => b != 0).Should().BeTrue();
        }
    }
}