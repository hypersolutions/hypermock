using System;
using HyperMock.Behaviors;
using HyperMock.Core;
using HyperMock.Exceptions;
using HyperMock.Setups;
using Xunit;

namespace Tests.HyperMock.Behaviors
{
    public class MethodCallTests
    {
        private readonly MethodCall _methodCall;

        public MethodCallTests()
        {
            _methodCall = new MethodCall(new SetupInfo());
        }

        [Fact]
        public void ThrowsAttachesExceptionTypeToSetup()
        {
            _methodCall.Throws<NotSupportedException>();

            Assert.IsType<NotSupportedException>(_methodCall.SetupInfo.GetValue().Value);
        }

        [Fact]
        public void ThrowsAttachesExceptionInstanceToSetup()
        {
            var exception = new NotSupportedException();
            _methodCall.Throws(exception);

            Assert.Equal(exception, _methodCall.SetupInfo.GetValue().Value);
        }

        [Fact]
        public void WithOutArgsReturnsSelf()
        {
            _methodCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.Out}
            };

            var self = _methodCall.WithOutArgs(10);

            Assert.Equal(_methodCall, self);
        }

        [Fact]
        public void WithOutArgsAttachesSingleValueToOutParameter()
        {
            _methodCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.Out}
            };

            _methodCall.WithOutArgs(10);

            Assert.Equal(10, _methodCall.SetupInfo.Parameters[0].Value);
        }

        [Fact]
        public void WithOutArgsAttachesSingleValueToCorrectOutParameter()
        {
            _methodCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Out}
            };

            _methodCall.WithOutArgs(10);

            Assert.Equal(10, _methodCall.SetupInfo.Parameters[1].Value);
        }

        [Fact]
        public void WithOutArgsThrowsExceptionForNoParameterProvided()
        {
            _methodCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Out}
            };
            
            Assert.Throws<MockException>(() => _methodCall.WithOutArgs());
        }

        [Fact]
        public void WithOutArgsThrowsExceptionForMoreParametersProvided()
        {
            _methodCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Out}
            };

            Assert.Throws<MockException>(() => _methodCall.WithOutArgs(10, 20));
        }

        [Fact]
        public void WithRefArgsReturnsSelf()
        {
            _methodCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.Ref}
            };

            var self = _methodCall.WithRefArgs(10);

            Assert.Equal(_methodCall, self);
        }

        [Fact]
        public void WithRefArgsAttachesSingleValueToRefParameter()
        {
            _methodCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.Ref}
            };

            _methodCall.WithRefArgs(10);

            Assert.Equal(10, _methodCall.SetupInfo.Parameters[0].Value);
        }

        [Fact]
        public void WithRefArgsAttachesSingleValueToCorrectRefParameter()
        {
            _methodCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Ref}
            };

            _methodCall.WithRefArgs(10);

            Assert.Equal(10, _methodCall.SetupInfo.Parameters[1].Value);
        }

        [Fact]
        public void WithRefArgsThrowsExceptionForNoParameterProvided()
        {
            _methodCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Ref}
            };
            
            Assert.Throws<MockException>(() => _methodCall.WithRefArgs());
        }

        [Fact]
        public void WithRefArgsThrowsExceptionForMoreParametersProvided()
        {
            _methodCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Ref}
            };

            Assert.Throws<MockException>(() => _methodCall.WithRefArgs(10, 20));
        }
    }
}