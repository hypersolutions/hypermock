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
    }
}