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
        public void WithOutParamsReturnsSelf()
        {
            _methodCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.Out}
            };

            var self = _methodCall.WithOutParams(10);

            Assert.AreEqual(_methodCall, self);
        }

        [TestMethod]
        public void WithOutParamsAttachesSingleValueToOutParameter()
        {
            _methodCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.Out}
            };

            _methodCall.WithOutParams(10);

            Assert.AreEqual(10, _methodCall.SetupInfo.Parameters[0].Value);
        }

        [TestMethod]
        public void WithOutParamsAttachesSingleValueToCorrectOutParameter()
        {
            _methodCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Out}
            };

            _methodCall.WithOutParams(10);

            Assert.AreEqual(10, _methodCall.SetupInfo.Parameters[1].Value);
        }

#if WINDOWS_UWP
        [TestMethod]
        public void WithOutParamsThrowsExceptionForNoParameterProvided()
        {
            _methodCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Out}
            };
            
            Assert.ThrowsException<MockException>(() => _methodCall.WithOutParams());
        }

        [TestMethod]
        public void WithOutParamsThrowsExceptionForMoreParametersProvided()
        {
            _methodCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Out}
            };

            Assert.ThrowsException<MockException>(() => _methodCall.WithOutParams(10, 20));
        }
#else
        [TestMethod, ExpectedException(typeof(MockException))]
        public void WithOutParamsThrowsExceptionForNoParameterProvided()
        {
            _methodCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Out}
            };

            _methodCall.WithOutParams();
        }

        [TestMethod, ExpectedException(typeof(MockException))]
        public void WithOutParamsThrowsExceptionForMoreParametersProvided()
        {
            _methodCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Out}
            };

            _methodCall.WithOutParams(10, 20);
        }
#endif
    }
}