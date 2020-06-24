using HyperMock.Exceptions;
using HyperMock.Tests.Support;
using Shouldly;
using Xunit;

namespace HyperMock.Tests.Integration
{
    public class StrictSetupSetPropertyTests
    {
        [Fact]
        public void CallThrowsStrictMockViolationExceptionForUnsetupMockSetProperty()
        {
            var accountServiceMock = Mock.Create<IAccountService>(MockBehavior.Strict);
            var subject = new AccountController(accountServiceMock.Object);

            Should.Throw<StrictMockViolationException>(() => subject.Manage(true));
        }
    }
}
