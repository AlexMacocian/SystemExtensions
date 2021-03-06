﻿using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Configuration;
using System.Extensions.Configuration;

namespace SystemExtensions.DependencyInjection.Tests.Configuration
{
    [TestClass]
    public class UpdateableOptionsWrapperTests
    {
        private const string Value = "hello";

        private UpdateableOptionsWrapper<string> optionsWrapper;
        private readonly Mock<IOptionsManager> optionsManagerMock = new();

        [TestInitialize]
        public void TestInitialize()
        {
            this.optionsWrapper = new UpdateableOptionsWrapper<string>(optionsManagerMock.Object, Value);
        }

        [TestMethod]
        public void GetValue_ReturnsValue()
        {
            this.optionsManagerMock
                .Setup(u => u.GetOptions<string>())
                .Throws<Exception>();

            var value = this.optionsWrapper.Value;

            value.Should().Be(Value);
        }

        [TestMethod]
        public void UpdateOption_CallsOptionsManager()
        {
            this.optionsManagerMock
                .Setup(u => u.UpdateOptions<string>(It.IsAny<string>()))
                .Verifiable();

            this.optionsWrapper.UpdateOption();

            this.optionsManagerMock.Verify();
        }
    }
}
