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
    public class GetPropertyCallTests
    {
        private GetPropertyCall<int> _getPropertyCall;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _getPropertyCall = new GetPropertyCall<int>(new SetupInfo());
        }

        [TestMethod]
        public void ThrowsAttachesExceptionTypeToSetup()
        {
            _getPropertyCall.Throws<NotSupportedException>();

            Assert.IsInstanceOfType(_getPropertyCall.SetupInfo.Exception, typeof(NotSupportedException));
        }

        [TestMethod]
        public void ThrowsAttachesExceptionInstanceToSetup()
        {
            _getPropertyCall.Throws(new NotSupportedException());

            Assert.IsInstanceOfType(_getPropertyCall.SetupInfo.Exception, typeof(NotSupportedException));
        }

        [TestMethod]
        public void ReturnsAttachesValueToSetup()
        {
            var returnValue = 10;

            _getPropertyCall.Returns(returnValue);

            Assert.AreEqual(returnValue, _getPropertyCall.SetupInfo.Value);
        }

        [TestMethod]
        public void ReturnsAttachesDeferredFuncToSetup()
        {
            var returnValue = 0;

            _getPropertyCall.Returns(() => returnValue);

            Assert.IsInstanceOfType(_getPropertyCall.SetupInfo.Value, typeof(Func<int>));
        }
    }
}