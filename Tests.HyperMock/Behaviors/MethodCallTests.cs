using System;
using HyperMock.Behaviors;
using HyperMock.Core;
using HyperMock.Exceptions;
using HyperMock.Setups;
#if WINDOWS_UWP
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace Tests.HyperMock.Behaviors
{
    [TestClass]
    public class MethodCallTests
    {
        private MethodCall _methodCall;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _methodCall = new MethodCall(new SetupInfo());
        }

        [TestMethod]
        public void ThrowsAttachesExceptionTypeToSetup()
        {
            _methodCall.Throws<NotSupportedException>();

            Assert.IsInstanceOfType(_methodCall.SetupInfo.Exception, typeof(NotSupportedException));
        }

        [TestMethod]
        public void ThrowsAttachesExceptionInstanceToSetup()
        {
            _methodCall.Throws(new NotSupportedException());

            Assert.IsInstanceOfType(_methodCall.SetupInfo.Exception, typeof(NotSupportedException));
        }

        [TestMethod]
        public void WithOutArgsReturnsSelf()
        {
            _methodCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.Out}
            };

            var self = _methodCall.WithOutArgs(10);

            Assert.AreEqual(_methodCall, self);
        }

        [TestMethod]
        public void WithOutArgsAttachesSingleValueToOutParameter()
        {
            _methodCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.Out}
            };

            _methodCall.WithOutArgs(10);

            Assert.AreEqual(10, _methodCall.SetupInfo.Parameters[0].Value);
        }

        [TestMethod]
        public void WithOutArgsAttachesSingleValueToCorrectOutParameter()
        {
            _methodCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Out}
            };

            _methodCall.WithOutArgs(10);

            Assert.AreEqual(10, _methodCall.SetupInfo.Parameters[1].Value);
        }

#if WINDOWS_UWP
        [TestMethod]
        public void WithOutArgsThrowsExceptionForNoParameterProvided()
        {
            _methodCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Out}
            };
            
            Assert.ThrowsException<MockException>(() => _methodCall.WithOutArgs());
        }

        [TestMethod]
        public void WithOutArgsThrowsExceptionForMoreParametersProvided()
        {
            _methodCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Out}
            };

            Assert.ThrowsException<MockException>(() => _methodCall.WithOutArgs(10, 20));
        }
#else
        [TestMethod, ExpectedException(typeof(MockException))]
        public void WithOutArgsThrowsExceptionForNoParameterProvided()
        {
            _methodCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Out}
            };

            _methodCall.WithOutArgs();
        }

        [TestMethod, ExpectedException(typeof(MockException))]
        public void WithOutArgsThrowsExceptionForMoreParametersProvided()
        {
            _methodCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Out}
            };

            _methodCall.WithOutArgs(10, 20);
        }
#endif

        [TestMethod]
        public void WithRefArgsReturnsSelf()
        {
            _methodCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.Ref}
            };

            var self = _methodCall.WithRefArgs(10);

            Assert.AreEqual(_methodCall, self);
        }

        [TestMethod]
        public void WithRefArgsAttachesSingleValueToRefParameter()
        {
            _methodCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.Ref}
            };

            _methodCall.WithRefArgs(10);

            Assert.AreEqual(10, _methodCall.SetupInfo.Parameters[0].Value);
        }

        [TestMethod]
        public void WithRefArgsAttachesSingleValueToCorrectRefParameter()
        {
            _methodCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Ref}
            };

            _methodCall.WithRefArgs(10);

            Assert.AreEqual(10, _methodCall.SetupInfo.Parameters[1].Value);
        }

#if WINDOWS_UWP
        [TestMethod]
        public void WithRefArgsThrowsExceptionForNoParameterProvided()
        {
            _methodCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Ref}
            };
            
            Assert.ThrowsException<MockException>(() => _methodCall.WithRefArgs());
        }

        [TestMethod]
        public void WithRefArgsThrowsExceptionForMoreParametersProvided()
        {
            _methodCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Ref}
            };

            Assert.ThrowsException<MockException>(() => _methodCall.WithRefArgs(10, 20));
        }
#else
        [TestMethod, ExpectedException(typeof(MockException))]
        public void WithRefArgsThrowsExceptionForNoParameterProvided()
        {
            _methodCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Ref}
            };

            _methodCall.WithRefArgs();
        }

        [TestMethod, ExpectedException(typeof(MockException))]
        public void WithRefArgsThrowsExceptionForMoreParametersProvided()
        {
            _methodCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Ref}
            };

            _methodCall.WithRefArgs(10, 20);
        }
#endif
    }
}