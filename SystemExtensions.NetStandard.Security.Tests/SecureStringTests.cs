using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Encryption;

namespace SystemExtensions.NetStandard.Security.Tests
{
    [TestClass]
    public class SecureStringTests
    {
        [TestMethod]
        public void SecureString_NewSecureString_ReturnsValue()
        {
            var str = new SecureString("hello");

            str.Value.Should().Be("hello");
        }

        [TestMethod]
        public void SecureStringEmpty_AreEqual()
        {
            var str = SecureString.Empty;
            str.Should().Be(SecureString.Empty);
        }

        [TestMethod]
        public void SecureStringEmpty_Equals_StringEmpty()
        {
            var ss = SecureString.Empty;
            var s = string.Empty;

            ss.Should().Be(s);
        }

        [TestMethod]
        public void SecureString_Equals_OtherSecureString()
        {
            var ss1 = new SecureString("Hello");
            var ss2 = new SecureString("Hello");

            ss1.Equals(ss2).Should().BeTrue();
        }

        [TestMethod]
        [DataRow("hello", "hello", true)]
        [DataRow("hello", "henlo", false)]
        public void SecureString_EqualOperator_OtherSecureString(string str1, string str2, bool isEqual)
        {
            var ss1 = new SecureString(str1);
            var ss2 = new SecureString(str2);

            (ss1 == ss2).Should().Be(isEqual);
        }

        [TestMethod]
        [DataRow("hello", "hello", false)]
        [DataRow("hello", "henlo", true)]
        public void SecureString_DifferentOperator_OtherSecureString(string str1, string str2, bool isDifferent)
        {
            var ss1 = new SecureString(str1);
            var ss2 = new SecureString(str2);

            (ss1 != ss2).Should().Be(isDifferent);
        }

        [TestMethod]
        [DataRow("hello", "hello", true)]
        [DataRow("hello", "henlo", false)]
        public void SecureString_EqualOperator_String(string str1, string str2, bool isEqual)
        {
            var ss1 = new SecureString(str1);

            (ss1 == str2).Should().Be(isEqual);
        }

        [TestMethod]
        [DataRow("hello", "hello", false)]
        [DataRow("hello", "henlo", true)]
        public void SecureString_DifferentOperator_String(string str1, string str2, bool isDifferent)
        {
            var ss1 = new SecureString(str1);

            (ss1 != str2).Should().Be(isDifferent);
        }

        [TestMethod]
        public void SecureString_PlusOperator_SecureString()
        {
            var ss1 = new SecureString("Hello ");
            var ss2 = new SecureString("World");

            (ss1 + ss2).Should().Be("Hello World");
        }

        [TestMethod]
        public void SecureString_PlusOperator_String()
        {
            var ss1 = new SecureString("Hello ");
            var ss2 = "World";

            (ss1 + ss2).Should().Be("Hello World");
        }

        [TestMethod]
        public void SecureString_PlusOperator_Char()
        {
            var ss1 = new SecureString("Hello ");
            var c = 'W';

            (ss1 + c).Should().Be("Hello W");
        }

        [TestMethod]
        public void SecureString_WithOptionalEntropy_Matches()
        {
            SecureString.AddOptionalEntropy(new byte[] { 10, 20, 25, 34, 56, 12, 10, 81, 200, 155, 123, 144, 123, 192, 122, 1 });
            var ss1 = new SecureString("Hello");
            var ss2 = new SecureString("Hello");

            ss1.Should().Be(ss2);
        }

        [TestMethod]
        public void SecureString_ChangingEntropy_ThrowsOnPreviousValue()
        {
            var ss1 = new SecureString("Hello");
            SecureString.AddOptionalEntropy(new byte[] { 10, 20, 25, 34, 56, 12, 10, 81, 200, 155, 123, 144, 123, 192, 122, 1 });

            var action = new Func<string>(() => ss1.Value);

            action.Should().Throw<Exception>();
        }
    }
}
