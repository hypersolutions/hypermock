using System;
using HyperMock.Behaviors;
using HyperMock.Core;
using HyperMock.Exceptions;
using HyperMock.Setups;
using Shouldly;
using Xunit;

namespace HyperMock.Tests.Behaviors
{
    public class FunctionCallTests
    {
        private readonly FunctionCall<int> _functionCall;

        public FunctionCallTests()
        {
            _functionCall = new FunctionCall<int>(new SetupInfo());
        }

        [Fact]
        public void ThrowsAttachesExceptionTypeToSetup()
        {
            _functionCall.Throws<NotSupportedException>();

            _functionCall.SetupInfo.GetValue().Value.ShouldBeOfType<NotSupportedException>();
        }

        [Fact]
        public void ThrowsAttachesExceptionInstanceToSetup()
        {
            var exception = new NotSupportedException();

            _functionCall.Throws(exception);

            _functionCall.SetupInfo.GetValue().Value.ShouldBe(exception);
        }

        [Fact]
        public void ReturnsAttachesValueToSetup()
        {
            var returnValue = 10;

            _functionCall.Returns(returnValue);

            _functionCall.SetupInfo.GetValue().Value.ShouldBe(returnValue);
        }

        [Fact]
        public void ReturnsAttachesDeferredFuncToSetup()
        {
            var returnValue = 0;

            _functionCall.Returns(() => returnValue);

            _functionCall.SetupInfo.GetValue().Value.ShouldBeOfType<Func<int>>();
        }

        [Fact]
        public void WithOutArgsReturnsSelf()
        {
            _functionCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.Out}
            };

            var self = _functionCall.WithOutArgs(10);

            _functionCall.ShouldBe(self);
        }

        [Fact]
        public void WithOutArgsAttachesSingleValueToOutParameter()
        {
            _functionCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.Out}
            };

            _functionCall.WithOutArgs(10);

            _functionCall.SetupInfo.Parameters[0].Value.ShouldBe(10);
        }

        [Fact]
        public void WithOutArgsAttachesSingleValueToCorrectOutParameter()
        {
            _functionCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Out}
            };

            _functionCall.WithOutArgs(10);

            _functionCall.SetupInfo.Parameters[1].Value.ShouldBe(10);
        }

        [Fact]
        public void WithOutArgsThrowsExceptionForNoParameterProvided()
        {
            _functionCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Out}
            };

            Should.Throw<MockException>(() => _functionCall.WithOutArgs());
        }

        [Fact]
        public void WithOutArgsThrowsExceptionForMoreParametersProvided()
        {
            _functionCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Out}
            };

            Should.Throw<MockException>(() => _functionCall.WithOutArgs(10, 20));
        }

        [Fact]
        public void WithRefArgsReturnsSelf()
        {
            _functionCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.Ref}
            };

            var self = _functionCall.WithRefArgs(10);

            _functionCall.ShouldBe(self);
        }

        [Fact]
        public void WithRefArgsAttachesSingleValueToRefParameter()
        {
            _functionCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.Ref}
            };

            _functionCall.WithRefArgs(10);

            _functionCall.SetupInfo.Parameters[0].Value.ShouldBe(10);
        }

        [Fact]
        public void WithRefArgsAttachesSingleValueToCorrectRefParameter()
        {
            _functionCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Ref}
            };

            _functionCall.WithRefArgs(10);

            _functionCall.SetupInfo.Parameters[1].Value.ShouldBe(10);
        }

        [Fact]
        public void WithRefArgsThrowsExceptionForNoParameterProvided()
        {
            _functionCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Ref}
            };
            
            Should.Throw<MockException>(() => _functionCall.WithRefArgs());
        }

        [Fact]
        public void WithRefArgsThrowsExceptionForMoreParametersProvided()
        {
            _functionCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Ref}
            };

            Should.Throw<MockException>(() => _functionCall.WithRefArgs(10, 20));
        }
    }
}
