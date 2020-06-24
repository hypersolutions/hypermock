using System;
using HyperMock.Behaviors;
using HyperMock.Matchers;
using HyperMock.Setups;
using Shouldly;
using Xunit;

namespace HyperMock.Tests.Behaviors
{
    public class SetPropertyCallTests
    {
        private readonly SetPropertyCall<int> _setPropertyCall;

        public SetPropertyCallTests()
        {
            _setPropertyCall = new SetPropertyCall<int>(new SetupInfo());
        }

        [Fact]
        public void ThrowsAttachesExceptionTypeToSetup()
        {
            _setPropertyCall.Throws<NotSupportedException>();

            _setPropertyCall.SetupInfo.Exception.ShouldBeOfType<NotSupportedException>();
        }

        [Fact]
        public void ThrowsAttachesExceptionInstanceToSetup()
        {
            var exception = new NotSupportedException();
            
            _setPropertyCall.Throws(exception);

            _setPropertyCall.SetupInfo.Exception.ShouldBe(exception);
        }

        [Fact]
        public void SetValueReturnsSelf()
        {
            var result = _setPropertyCall.SetValue(10);

            _setPropertyCall.ShouldBe(result);
        }

        [Fact]
        public void SetValueAttachesExactMatchArgToSetup()
        {
            var value = 10;

            _setPropertyCall.SetValue(value);

            _setPropertyCall.SetupInfo.Parameters[0].Matcher.ShouldBeOfType<ExactParameterMatcher>();
            _setPropertyCall.SetupInfo.Parameters[0].Value.ShouldBe(value);
        }

        [Fact]
        public void AnySetValueReturnsSelf()
        {
            var continueSetPropertyCall = _setPropertyCall.AnySetValue();

            _setPropertyCall.ShouldBe(continueSetPropertyCall);
        }

        [Fact]
        public void AnySetValueAttachesAnyMatchArgToSetup()
        {
            _setPropertyCall.AnySetValue();

            _setPropertyCall.SetupInfo.Parameters[0].Matcher.ShouldBeOfType<AnyParameterMatcher>();
            _setPropertyCall.SetupInfo.Parameters[0].Value.ShouldBeNull();
        }
    }
}
