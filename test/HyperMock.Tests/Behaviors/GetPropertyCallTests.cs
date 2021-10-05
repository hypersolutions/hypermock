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

            _getPropertyCall.SetupInfo.GetValue().Value.ShouldBeOfType<NotSupportedException>();
        }

        [Fact]
        public void ThrowsAttachesExceptionInstanceToSetup()
        {
            var exception = new NotSupportedException();
            
            _getPropertyCall.Throws(exception);

            _getPropertyCall.SetupInfo.GetValue().Value.ShouldBe(exception);
        }

        [Fact]
        public void ReturnsAttachesValueToSetup()
        {
            const int returnValue = 10;

            _getPropertyCall.Returns(returnValue);

            _getPropertyCall.SetupInfo.GetValue().Value.ShouldBe(returnValue);
        }

        [Fact]
        public void ReturnsAttachesDeferredFuncToSetup()
        {
            const int returnValue = 0;

            _getPropertyCall.Returns(() => returnValue);

            _getPropertyCall.SetupInfo.GetValue().Value.ShouldBeOfType<Func<int>>();
        }
    }
}
