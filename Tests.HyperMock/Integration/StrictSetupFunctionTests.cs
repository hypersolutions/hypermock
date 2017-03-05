using HyperMock;
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
    public class StrictSetupFunctionTests
    {
#if WINDOWS_UWP
        [TestMethod]
        public void CallThrowsStrictMockViolationExceptionForUnsetupMockFunction()
        {
            var accountServiceMock = Mock.Create<IAccountService>(MockBehavior.Strict);
            var info = new AccountInfo { Number = "12345678", DebitAmount = 100 };
            var subject = new AccountController(accountServiceMock.Object);

            Assert.ThrowsException<StrictMockViolationException>(() => subject.Debit(info));
        }
#else
        [TestMethod, ExpectedException(typeof(StrictMockViolationException))]
        public void CallThrowsStrictMockViolationExceptionForUnsetupMockFunction()
        {
            var accountServiceMock = Mock.Create<IAccountService>(MockBehavior.Strict);
            var info = new AccountInfo { Number = "12345678", DebitAmount = 100 };
            var subject = new AccountController(accountServiceMock.Object);

            subject.Debit(info);
        }
#endif
    }
}