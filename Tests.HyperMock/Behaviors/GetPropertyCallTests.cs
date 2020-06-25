using System;
using HyperMock.Behaviors;
using HyperMock.Setups;
using Xunit;

namespace Tests.HyperMock.Behaviors
{
    public class GetPropertyCallTests
    {
        private readonly GetPropertyCall<int> _getPropertyCall;

        public GetPropertyCallTests()
        {
            _getPropertyCall = new GetPropertyCall<int>(new SetupInfo());
        }

        [Fact]
        public void ThrowsAttachesExceptionTypeToSetup()
        {
            _getPropertyCall.Throws<NotSupportedException>();

            Assert.IsType<NotSupportedException>(_getPropertyCall.SetupInfo.GetValue().Value);
        }

        [Fact]
        public void ThrowsAttachesExceptionInstanceToSetup()
        {
            var exception = new NotSupportedException();
            _getPropertyCall.Throws(exception);

            Assert.Equal(exception, _getPropertyCall.SetupInfo.GetValue().Value);
        }

        [Fact]
        public void ReturnsAttachesValueToSetup()
        {
            var returnValue = 10;

            _getPropertyCall.Returns(returnValue);

            Assert.Equal(returnValue, _getPropertyCall.SetupInfo.GetValue().Value);
        }

        [Fact]
        public void ReturnsAttachesDeferredFuncToSetup()
        {
            var returnValue = 0;

            _getPropertyCall.Returns(() => returnValue);

            Assert.IsType<Func<int>>(_getPropertyCall.SetupInfo.GetValue().Value);
        }
    }
}