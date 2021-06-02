using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Extensions.Tests
{
    [TestClass]
    public class ResultTests
    {
        [TestMethod]
        public void SuccessResultShouldCreateFromSuccess()
        {
            var success = string.Empty;
            var result = Result<string, object>.Success(success);

            result.DoAny(
                onFailure: () => throw new InvalidOperationException());
        }

        [TestMethod]
        public void FailureResultShouldCreateFromFailure()
        {
            var failure = new object();
            var result = Result<string, object>.Failure(failure);

            result.DoAny(
                onSuccess: () => throw new InvalidOperationException());
        }

        [TestMethod]
        public void DoShouldExecute()
        {
            var success = string.Empty;
            var successResult = Result<string, object>.Success(success);

            var failure = new object();
            var failedResult = Result<string, object>.Failure(failure);

            successResult.Do(
                onSuccess: _ => { },
                onFailure: _ => throw new InvalidOperationException());

            failedResult.Do(
                onSuccess: _ => throw new InvalidOperationException(),
                onFailure: _ => { });
        }

        [TestMethod]
        public void DoAnyShouldExecute()
        {
            var success = string.Empty;
            var successResult = Result<string, object>.Success(success);

            var failure = new object();
            var failedResult = Result<string, object>.Failure(failure);

            successResult.DoAny(
                onSuccess: _ => { });

            failedResult.DoAny(
                onFailure: _ => { });
        }

        [TestMethod]
        public void SwitchShouldExecute()
        {
            var success = string.Empty;
            var successResult = Result<string, object>.Success(success);

            var failure = new object();
            var failedResult = Result<string, object>.Failure(failure);

            var successSwitch = successResult.Switch(
                onSuccess: _ => string.Empty,
                onFailure: _ => throw new InvalidOperationException());

            var failedSwitch = failedResult.Switch(
                onSuccess: _ => throw new InvalidOperationException(),
                onFailure: _ => string.Empty);

            success.Should().Be(failedSwitch);
        }

        [TestMethod]
        public void SwitchAnyShouldExecute()
        {
            var success = string.Empty;
            var successResult = Result<string, object>.Success(success);

            var failure = new object();
            var failedResult = Result<string, object>.Failure(failure);

            var successSwitch = successResult.SwitchAny(
                onSuccess: _ => string.Empty);

            var failedSwitch = failedResult.SwitchAny(
                onFailure: _ => string.Empty);

            success.Should().Be(failedSwitch);
        }

        [TestMethod]
        public void ToOptionalShouldConvert()
        {
            var success = string.Empty;
            var successResult = Result<string, object>.Success(success);

            var failure = new object();
            var failedResult = Result<string, object>.Failure(failure);

            var successOptional = successResult.ToOptional();
            var failureOptional = failedResult.ToOptional();

            successOptional.Should().NotBeNull();
            failureOptional.Should().NotBeNull();
        }
    }
}
