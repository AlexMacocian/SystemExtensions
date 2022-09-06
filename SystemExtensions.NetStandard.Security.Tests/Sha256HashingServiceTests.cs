using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Security.Hashing;
using System.Text;
using System.Threading.Tasks;

namespace SystemExtensions.NetStandard.Security.Tests;

[TestClass]
public sealed class Sha256HashingServiceTests
{
    private readonly IHashingService hashingService = new Sha256HashingService();
    private string toHashtString;
    private byte[] toHashtBytes;
    private Stream toHashStream;

    [TestInitialize]
    public void TestInitialize()
    {
        this.toHashtBytes = Encoding.UTF8.GetBytes("toEncrypt");
        this.toHashtString = Convert.ToBase64String(this.toHashtBytes);
        this.toHashStream = new MemoryStream(this.toHashtBytes);
    }

    [TestMethod]
    public async Task HashString()
    {
        var hash = await this.hashingService.Hash(this.toHashtString).ConfigureAwait(false);
        hash.Should().BeOfType<string>();
    }

    [TestMethod]
    public async Task HashBytes()
    {
        var hash = await this.hashingService.Hash(this.toHashtBytes).ConfigureAwait(false);
        hash.Should().BeOfType<byte[]>();
    }

    [TestMethod]
    public async Task HashStream()
    {
        var hash = await this.hashingService.Hash(this.toHashStream).ConfigureAwait(false);
        hash.Should().BeAssignableTo<Stream>();
    }
}
