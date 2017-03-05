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
    public class StrictSetupGetPropertyTests
    {
#if WINDOWS_UWP
        [TestMethod]
        public void CallThrowsStrictMockViolationExceptionForUnsetupMockGetProperty()
        {
            var accountServiceMock = Mock.Create<IAccountService>(MockBehavior.Strict);
            var subject = new AccountController(accountServiceMock.Object);

            Assert.ThrowsException<StrictMockViolationException>(() => subject.HasAccounts());
        }
#else
        [TestMethod, ExpectedException(typeof(StrictMockViolationException))]
        public void CallThrowsStrictMockViolationExceptionForUnsetupMockGetProperty()
        {
            var accountServiceMock = Mock.Create<IAccountService>(MockBehavior.Strict);
            var subject = new AccountController(accountServiceMock.Object);

            subject.HasAccounts();
        }
#endif
    }
}