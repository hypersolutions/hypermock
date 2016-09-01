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
    public class SetupMethodTests : TestBase<AccountController>
    {
#if WINDOWS_UWP
        [TestMethod]
        public void SetupThrowsExceptionType()
        {
            var info = new AccountInfo { Number = "12345678", CreditAmount = 100 };
            MockFor<IAccountService>().Setup(
                s => s.Credit(info.Number, info.CreditAmount)).Throws<NotSupportedException>();

            Assert.ThrowsException<NotSupportedException>(() => Subject.Credit(info));
        }

        [TestMethod]
        public void SetupThrowsExceptionInstance()
        {
            var info = new AccountInfo { Number = "12345678", CreditAmount = 100 };
            MockFor<IAccountService>().Setup(
                s => s.Credit(info.Number, info.CreditAmount)).Throws(new NotSupportedException());

            Assert.ThrowsException<NotSupportedException>(() => Subject.Credit(info));
        }
#else
        [TestMethod, ExpectedException(typeof(NotSupportedException))]
        public void SetupThrowsExceptionType()
        {
            var info = new AccountInfo { Number = "12345678", CreditAmount = 100 };
            MockFor<IAccountService>().Setup(
                s => s.Credit(info.Number, info.CreditAmount)).Throws<NotSupportedException>();

            Subject.Credit(info);
        }

        [TestMethod, ExpectedException(typeof(NotSupportedException))]
        public void SetupThrowsExceptionInstance()
        {
            var info = new AccountInfo { Number = "12345678", CreditAmount = 100 };
            MockFor<IAccountService>().Setup(
                s => s.Credit(info.Number, info.CreditAmount)).Throws(new NotSupportedException());

            Subject.Credit(info);
        }
#endif
    }
}
