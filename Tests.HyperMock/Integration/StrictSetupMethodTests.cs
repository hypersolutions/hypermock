using System;
using HyperMock;
using HyperMock.Behaviors;
using HyperMock.Exceptions;
#if WINDOWS_UWP
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif
using Tests.HyperMock.Support;

namespace Tests.HyperMock.Integration
{
    [TestClass]
    public class StrictSetupMethodTests
    {
#if WINDOWS_UWP
        [TestMethod]
        public void CallThrowsStrictMockViolationExceptionForUnsetupMockMethod()
        {
            var accountServiceMock = Mock.Create<IAccountService>(MockBehavior.Strict);
            var info = new AccountInfo { Number = "12345678", CreditAmount = 100 };
            var subject = new AccountController(accountServiceMock.Object);

            Assert.ThrowsException<StrictMockViolationException>(() => subject.Credit(info));
        }
#else
        [TestMethod, ExpectedException(typeof(StrictMockViolationException))]
        public void CallThrowsStrictMockViolationExceptionForUnsetupMockMethod()
        {
            var accountServiceMock = Mock.Create<IAccountService>(MockBehavior.Strict);
            var info = new AccountInfo { Number = "12345678", CreditAmount = 100 };
            var subject = new AccountController(accountServiceMock.Object);

            subject.Credit(info);
        }
#endif
    }
}
