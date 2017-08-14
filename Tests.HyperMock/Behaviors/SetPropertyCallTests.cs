using System;
using HyperMock.Behaviors;
using HyperMock.Matchers;
using HyperMock.Setups;
using Xunit;

namespace Tests.HyperMock.Behaviors
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

            Assert.IsType<NotSupportedException>(_setPropertyCall.SetupInfo.Exception);
        }

        [Fact]
        public void ThrowsAttachesExceptionInstanceToSetup()
        {
            var exception = new NotSupportedException();
            _setPropertyCall.Throws(exception);

            Assert.Equal(exception, _setPropertyCall.SetupInfo.Exception);
        }

        [Fact]
        public void SetValueReturnsSelf()
        {
            var result = _setPropertyCall.SetValue(10);

            Assert.Equal(result, _setPropertyCall);
        }

        [Fact]
        public void SetValueAttachesExactMatchArgToSetup()
        {
            var value = 10;

            _setPropertyCall.SetValue(value);

            Assert.IsType<ExactParameterMatcher>(_setPropertyCall.SetupInfo.Parameters[0].Matcher);
            Assert.Equal(value, _setPropertyCall.SetupInfo.Parameters[0].Value);
        }

        [Fact]
        public void AnySetValueReturnsSelf()
        {
            var continueSetPropertyCall = _setPropertyCall.AnySetValue();

            Assert.Equal(continueSetPropertyCall, _setPropertyCall);
        }

        [Fact]
        public void AnySetValueAttachesAnyMatchArgToSetup()
        {
            _setPropertyCall.AnySetValue();

            Assert.IsType<AnyParameterMatcher>(_setPropertyCall.SetupInfo.Parameters[0].Matcher);
            Assert.Null(_setPropertyCall.SetupInfo.Parameters[0].Value);
        }
    }
}