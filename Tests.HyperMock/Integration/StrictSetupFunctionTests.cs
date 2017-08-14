using HyperMock;
using HyperMock.Exceptions;
using Tests.HyperMock.Support;
using Xunit;

namespace Tests.HyperMock.Integration
{
    public class StrictSetupFunctionTests
    {
        [Fact]
        public void CallThrowsStrictMockViolationExceptionForUnsetupMockFunction()
        {
            var accountServiceMock = Mock.Create<IAccountService>(MockBehavior.Strict);
            var info = new AccountInfo { Number = "12345678", DebitAmount = 100 };
            var subject = new AccountController(accountServiceMock.Object);

            Assert.Throws<StrictMockViolationException>(() => subject.Debit(info));
        }
    }
}