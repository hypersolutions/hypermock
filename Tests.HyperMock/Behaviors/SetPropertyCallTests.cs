using System;
using HyperMock.Behaviors;
using HyperMock.Matchers;
using HyperMock.Setups;
#if WINDOWS_UWP
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace Tests.HyperMock.Behaviors
{
    [TestClass]
    public class SetPropertyCallTests
    {
        private SetPropertyCall<int> _setPropertyCall;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _setPropertyCall = new SetPropertyCall<int>(new SetupInfo());
        }

        [TestMethod]
        public void ThrowsAttachesExceptionTypeToSetup()
        {
            _setPropertyCall.Throws<NotSupportedException>();

            Assert.IsInstanceOfType(_setPropertyCall.SetupInfo.Exception, typeof(NotSupportedException));
        }

        [TestMethod]
        public void ThrowsAttachesExceptionInstanceToSetup()
        {
            _setPropertyCall.Throws(new NotSupportedException());

            Assert.IsInstanceOfType(_setPropertyCall.SetupInfo.Exception, typeof(NotSupportedException));
        }

        [TestMethod]
        public void SetValueReturnsSelf()
        {
            var result = _setPropertyCall.SetValue(10);

            Assert.AreEqual(result, _setPropertyCall);
        }

        [TestMethod]
        public void SetValueAttachesExactMatchArgToSetup()
        {
            var value = 10;

            _setPropertyCall.SetValue(value);

            Assert.IsInstanceOfType(_setPropertyCall.SetupInfo.Parameters[0].Matcher, typeof(ExactParameterMatcher));
            Assert.AreEqual(value, _setPropertyCall.SetupInfo.Parameters[0].Value);
        }

        [TestMethod]
        public void AnySetValueReturnsSelf()
        {
            var continueSetPropertyCall = _setPropertyCall.AnySetValue();

            Assert.AreEqual(continueSetPropertyCall, _setPropertyCall);
        }

        [TestMethod]
        public void AnySetValueAttachesAnyMatchArgToSetup()
        {
            _setPropertyCall.AnySetValue();

            Assert.IsInstanceOfType(_setPropertyCall.SetupInfo.Parameters[0].Matcher, typeof(AnyParameterMatcher));
            Assert.IsNull(_setPropertyCall.SetupInfo.Parameters[0].Value);
        }
    }
}