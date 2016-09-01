using System;
using HyperMock.Behaviors;
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
    }
}
