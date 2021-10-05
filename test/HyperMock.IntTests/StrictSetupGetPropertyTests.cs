using HyperMock.Exceptions;
using HyperMock.IntTests.Support;
using Shouldly;
using Xunit;

namespace HyperMock.IntTests
{
    public class StrictSetupGetPropertyTests
    {
        [Fact]
        public void CallThrowsStrictMockViolationExceptionForUnSetupMockGetProperty()
        {
            var accountServiceMock = Mock.Create<IAccountService>(MockBehavior.Strict);
            var subject = new AccountController(accountServiceMock.Object);

            Should.Throw<StrictMockViolationException>(() => subject.HasAccounts());
        }
    }
}
