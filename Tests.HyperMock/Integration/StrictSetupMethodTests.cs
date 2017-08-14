using HyperMock;
using HyperMock.Exceptions;
using Tests.HyperMock.Support;
using Xunit;

namespace Tests.HyperMock.Integration
{
    public class StrictSetupMethodTests
    {
        [Fact]
        public void CallThrowsStrictMockViolationExceptionForUnsetupMockMethod()
        {
            var accountServiceMock = Mock.Create<IAccountService>(MockBehavior.Strict);
            var info = new AccountInfo { Number = "12345678", CreditAmount = 100 };
            var subject = new AccountController(accountServiceMock.Object);

            Assert.Throws<StrictMockViolationException>(() => subject.Credit(info));
        }
    }
}
