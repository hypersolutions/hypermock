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
    public class FunctionCallTests
    {
        private FunctionCall<int> _functionCall;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _functionCall = new FunctionCall<int>(new SetupInfo());
        }

        [TestMethod]
        public void ThrowsAttachesExceptionTypeToSetup()
        {
            _functionCall.Throws<NotSupportedException>();

            Assert.IsInstanceOfType(_functionCall.SetupInfo.Exception, typeof(NotSupportedException));
        }

        [TestMethod]
        public void ThrowsAttachesExceptionInstanceToSetup()
        {
            _functionCall.Throws(new NotSupportedException());

            Assert.IsInstanceOfType(_functionCall.SetupInfo.Exception, typeof(NotSupportedException));
        }

        [TestMethod]
        public void ReturnsAttachesValueToSetup()
        {
            var returnValue = 10;

            _functionCall.Returns(returnValue);

            Assert.AreEqual(returnValue, _functionCall.SetupInfo.Value);
        }

        [TestMethod]
        public void WithOutArgsReturnsSelf()
        {
            _functionCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.Out}
            };

            var self = _functionCall.WithOutArgs(10);

            Assert.AreEqual(_functionCall, self);
        }

        [TestMethod]
        public void WithOutArgsAttachesSingleValueToOutParameter()
        {
            _functionCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.Out}
            };

            _functionCall.WithOutArgs(10);

            Assert.AreEqual(10, _functionCall.SetupInfo.Parameters[0].Value);
        }

        [TestMethod]
        public void WithOutArgsAttachesSingleValueToCorrectOutParameter()
        {
            _functionCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Out}
            };

            _functionCall.WithOutArgs(10);

            Assert.AreEqual(10, _functionCall.SetupInfo.Parameters[1].Value);
        }

#if WINDOWS_UWP
        [TestMethod]
        public void WithOutArgsThrowsExceptionForNoParameterProvided()
        {
            _functionCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Out}
            };
            
            Assert.ThrowsException<MockException>(() => _functionCall.WithOutArgs());
        }

        [TestMethod]
        public void WithOutArgsThrowsExceptionForMoreParametersProvided()
        {
            _functionCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Out}
            };

            Assert.ThrowsException<MockException>(() => _functionCall.WithOutArgs(10, 20));
        }
#else
        [TestMethod, ExpectedException(typeof(MockException))]
        public void WithOutArgsThrowsExceptionForNoParameterProvided()
        {
            _functionCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Out}
            };

            _functionCall.WithOutArgs();
        }

        [TestMethod, ExpectedException(typeof(MockException))]
        public void WithOutArgsThrowsExceptionForMoreParametersProvided()
        {
            _functionCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Out}
            };

            _functionCall.WithOutArgs(10, 20);
        }
#endif

        [TestMethod]
        public void WithRefArgsReturnsSelf()
        {
            _functionCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.Ref}
            };

            var self = _functionCall.WithRefArgs(10);

            Assert.AreEqual(_functionCall, self);
        }

        [TestMethod]
        public void WithRefArgsAttachesSingleValueToRefParameter()
        {
            _functionCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.Ref}
            };

            _functionCall.WithRefArgs(10);

            Assert.AreEqual(10, _functionCall.SetupInfo.Parameters[0].Value);
        }

        [TestMethod]
        public void WithRefArgsAttachesSingleValueToCorrectRefParameter()
        {
            _functionCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Ref}
            };

            _functionCall.WithRefArgs(10);

            Assert.AreEqual(10, _functionCall.SetupInfo.Parameters[1].Value);
        }

#if WINDOWS_UWP
        [TestMethod]
        public void WithRefArgsThrowsExceptionForNoParameterProvided()
        {
            _functionCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Ref}
            };
            
            Assert.ThrowsException<MockException>(() => _functionCall.WithRefArgs());
        }

        [TestMethod]
        public void WithRefArgsThrowsExceptionForMoreParametersProvided()
        {
            _functionCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Ref}
            };

            Assert.ThrowsException<MockException>(() => _functionCall.WithRefArgs(10, 20));
        }
#else
        [TestMethod, ExpectedException(typeof(MockException))]
        public void WithRefArgsThrowsExceptionForNoParameterProvided()
        {
            _functionCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Ref}
            };

            _functionCall.WithRefArgs();
        }

        [TestMethod, ExpectedException(typeof(MockException))]
        public void WithRefArgsThrowsExceptionForMoreParametersProvided()
        {
            _functionCall.SetupInfo.Parameters = new[]
            {
                new Parameter {Type = ParameterType.In},
                new Parameter {Type = ParameterType.Ref}
            };

            _functionCall.WithRefArgs(10, 20);
        }
#endif
    }
}
