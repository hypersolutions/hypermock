using HyperMock.Exceptions;
using HyperMock.Tests.Support;
using Shouldly;
using Xunit;

namespace HyperMock.Tests.Integration
{
    public class StrictSetupFunctionTests
    {
        [Fact]
        public void CallThrowsStrictMockViolationExceptionForUnsetupMockFunction()
        {
            var accountServiceMock = Mock.Create<IAccountService>(MockBehavior.Strict);
            var info = new AccountInfo { Number = "12345678", DebitAmount = 100 };
            var subject = new AccountController(accountServiceMock.Object);

            Should.Throw<StrictMockViolationException>(() => subject.Debit(info));
        }
    }
}
