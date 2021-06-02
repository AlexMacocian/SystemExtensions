using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Extensions.Tests
{
    [TestClass]
    public class OptionalTests
    {
        [TestMethod]
        public void SomeValueShouldEqualOptional()
        {
            var value = string.Empty;
            var optional = value.ToOptional();

            Assert.IsTrue(optional.Equals(value));
        }

        [TestMethod]
        public void OptionalFromNullShouldNotEqual()
        {
            object value = null;
            var optional = value.ToOptional();

            Assert.IsFalse(optional.Equals(value));
        }

        [TestMethod]
        public void OptionalFromValueShouldBeSome()
        {
            var optional = Optional.FromValue<object>(new object());

            optional.DoAny(
                onNone: () => throw new InvalidOperationException());
        }

        [TestMethod]
        public void OptionalFromNullShouldBeNone()
        {
            var optional = Optional.FromValue<object>(null);

            optional.DoAny(
                onSome: _ => throw new InvalidOperationException());
        }

        [TestMethod]
        public void DoShouldExecute()
        {
            var optional = Optional.FromValue(string.Empty);
            var nullOptional = Optional.FromValue<string>(null);

            optional.Do(
                onSome: _ => { },
                onNone: () => throw new InvalidOperationException());

            nullOptional.Do(
                onSome: _ => throw new InvalidOperationException(),
                onNone: () => { });
        }

        [TestMethod]
        public void DoAnyShouldExecute()
        {
            var optional = Optional.FromValue(string.Empty);
            var nullOptional = Optional.FromValue<string>(null);

            optional.DoAny(
                onNone: () => throw new InvalidOperationException());

            nullOptional.DoAny(
                onSome: _ => throw new InvalidOperationException());
        }

        [TestMethod]
        public void SwitchShouldExecute()
        {
            var optional = Optional.FromValue(string.Empty);
            var nullOptional = Optional.FromValue<string>(null);

            var newValue = optional.Switch(
                onSome: _ => string.Empty,
                onNone: () => throw new InvalidOperationException())
                .ExtractValue();

            var newNullValue = nullOptional.Switch<string>(
                onSome: _ => throw new InvalidOperationException(),
                onNone: () => null)
                .ExtractValue();

            newValue.Should().Be(string.Empty);
            newNullValue.Should().Be(null);
        }

        [TestMethod]
        public void SwitchAnyShouldExecute()
        {
            var optional = Optional.FromValue(string.Empty);
            var nullOptional = Optional.FromValue<string>(null);

            var newValue = optional.SwitchAny(
                onSome: _ => string.Empty)
                .ExtractValue();

            var newNullValue = nullOptional.SwitchAny<string>(
                onNone: () => null)
                .ExtractValue();

            newValue.Should().Be(string.Empty);
            newNullValue.Should().Be(null);
        }
    }
}
