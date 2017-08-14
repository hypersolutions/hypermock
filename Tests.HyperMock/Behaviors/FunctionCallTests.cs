using System;
using HyperMock.Behaviors;
using HyperMock.Core;
using HyperMock.Exceptions;
using HyperMock.Setups;
using Xunit;

namespace Tests.HyperMock.Behaviors
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

            Assert.IsType<NotSupportedException>(_functionCall.SetupInfo.Exception);
        }

        [Fact]
        public void ThrowsAttachesExceptionInstanceToSetup()
        {
            var exception = new NotSupportedException();
            _functionCall.Throws(exception);

            Assert.Equal(exception, _functionCall.SetupInfo.Exception);
        }

        [Fact]
        public void ReturnsAttachesValueToSetup()
        {
            var returnValue = 10;

            _functionCall.Returns(returnValue);

            Assert.Equal(returnValue, _functionCall.SetupInfo.Value);
        }

        [Fact]
        public void ReturnsAttachesDeferredFuncToSetup()
        {
            var returnValue = 0;

            _functionCall.Returns(() => returnValue);

            Assert.IsType<Func<int>>(_functionCall.SetupInfo.Value);
        }

        [Fact]
        public void WithOutArgsReturnsSelf()
        {
            _functionCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.Out}
            };

            var self = _functionCall.WithOutArgs(10);

            Assert.Equal(_functionCall, self);
        }

        [Fact]
        public void WithOutArgsAttachesSingleValueToOutParameter()
        {
            _functionCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.Out}
            };

            _functionCall.WithOutArgs(10);

            Assert.Equal(10, _functionCall.SetupInfo.Parameters[0].Value);
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

            Assert.Equal(10, _functionCall.SetupInfo.Parameters[1].Value);
        }

        [Fact]
        public void WithOutArgsThrowsExceptionForNoParameterProvided()
        {
            _functionCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Out}
            };
            
            Assert.Throws<MockException>(() => _functionCall.WithOutArgs());
        }

        [Fact]
        public void WithOutArgsThrowsExceptionForMoreParametersProvided()
        {
            _functionCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Out}
            };

            Assert.Throws<MockException>(() => _functionCall.WithOutArgs(10, 20));
        }

        [Fact]
        public void WithRefArgsReturnsSelf()
        {
            _functionCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.Ref}
            };

            var self = _functionCall.WithRefArgs(10);

            Assert.Equal(_functionCall, self);
        }

        [Fact]
        public void WithRefArgsAttachesSingleValueToRefParameter()
        {
            _functionCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.Ref}
            };

            _functionCall.WithRefArgs(10);

            Assert.Equal(10, _functionCall.SetupInfo.Parameters[0].Value);
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

            Assert.Equal(10, _functionCall.SetupInfo.Parameters[1].Value);
        }

        [Fact]
        public void WithRefArgsThrowsExceptionForNoParameterProvided()
        {
            _functionCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Ref}
            };
            
            Assert.Throws<MockException>(() => _functionCall.WithRefArgs());
        }

        [Fact]
        public void WithRefArgsThrowsExceptionForMoreParametersProvided()
        {
            _functionCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Ref}
            };

            Assert.Throws<MockException>(() => _functionCall.WithRefArgs(10, 20));
        }
    }
}
