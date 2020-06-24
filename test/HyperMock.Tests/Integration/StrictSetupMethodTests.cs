using HyperMock.Exceptions;
using HyperMock.Tests.Support;
using Shouldly;
using Xunit;

namespace HyperMock.Tests.Integration
{
    public class StrictSetupMethodTests
    {
        [Fact]
        public void CallThrowsStrictMockViolationExceptionForUnsetupMockMethod()
        {
            var accountServiceMock = Mock.Create<IAccountService>(MockBehavior.Strict);
            var info = new AccountInfo { Number = "12345678", CreditAmount = 100 };
            var subject = new AccountController(accountServiceMock.Object);

            Should.Throw<StrictMockViolationException>(() => subject.Credit(info));
        }
    }
}
