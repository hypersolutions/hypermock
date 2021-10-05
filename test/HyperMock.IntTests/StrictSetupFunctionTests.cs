using HyperMock.Exceptions;
using HyperMock.IntTests.Support;
using Shouldly;
using Xunit;

namespace HyperMock.IntTests
{
    public class StrictSetupFunctionTests
    {
        [Fact]
        public void CallThrowsStrictMockViolationExceptionForUnSetupMockFunction()
        {
            var accountServiceMock = Mock.Create<IAccountService>(MockBehavior.Strict);
            var info = new AccountInfo { Number = "12345678", DebitAmount = 100 };
            var subject = new AccountController(accountServiceMock.Object);

            Should.Throw<StrictMockViolationException>(() => subject.Debit(info));
        }
    }
}
