using System;
using HyperMock.Behaviors;
using HyperMock.Setups;
using Shouldly;
using Xunit;

namespace HyperMock.Tests.Behaviors
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

            _getPropertyCall.SetupInfo.Exception.ShouldBeOfType<NotSupportedException>();
        }

        [Fact]
        public void ThrowsAttachesExceptionInstanceToSetup()
        {
            var exception = new NotSupportedException();
            
            _getPropertyCall.Throws(exception);

            _getPropertyCall.SetupInfo.Exception.ShouldBe(exception);
        }

        [Fact]
        public void ReturnsAttachesValueToSetup()
        {
            var returnValue = 10;

            _getPropertyCall.Returns(returnValue);

            _getPropertyCall.SetupInfo.Value.ShouldBe(returnValue);
        }

        [Fact]
        public void ReturnsAttachesDeferredFuncToSetup()
        {
            var returnValue = 0;

            _getPropertyCall.Returns(() => returnValue);

            _getPropertyCall.SetupInfo.Value.ShouldBeOfType<Func<int>>();
        }
    }
}
