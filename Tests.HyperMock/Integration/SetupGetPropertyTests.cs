using System;
#if WINDOWS_UWP
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif
using Tests.HyperMock.Support;

namespace Tests.HyperMock.Integration
{
    [TestClass]
    public class SetupGetPropertyTests : TestBase<AccountController>
    {
#if WINDOWS_UWP
        [TestMethod]
        public void SetupThrowsExceptionType()
        {
            MockFor<IAccountService>().SetupGet(s => s.HasAccounts).Throws<NotSupportedException>();

            Assert.ThrowsException<NotSupportedException>(() => Subject.HasAccounts());
        }

        [TestMethod]
        public void SetupThrowsExceptionInstance()
        {
            MockFor<IAccountService>().SetupGet(s => s.HasAccounts).Throws(new NotSupportedException());

            Assert.ThrowsException<NotSupportedException>(() => Subject.HasAccounts());
        }
#else
        [TestMethod, ExpectedException(typeof(NotSupportedException))]
        public void SetupThrowsExceptionType()
        {
            MockFor<IAccountService>().SetupGet(s => s.HasAccounts).Throws<NotSupportedException>();

            Subject.HasAccounts();
        }

        [TestMethod, ExpectedException(typeof(NotSupportedException))]
        public void SetupThrowsExceptionInstance()
        {
            MockFor<IAccountService>().SetupGet(s => s.HasAccounts).Throws(new NotSupportedException());

            Subject.HasAccounts();
        }
#endif
    }
}