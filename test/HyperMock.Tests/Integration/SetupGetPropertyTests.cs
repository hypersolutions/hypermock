using System;
using HyperMock.Tests.Support;
using Shouldly;
using Xunit;

namespace HyperMock.Tests.Integration
{
    public class SetupGetPropertyTests : TestBase<AccountController>
    {
        [Fact]
        public void SetupThrowsExceptionType()
        {
            MockFor<IAccountService>().SetupGet(s => s.HasAccounts).Throws<NotSupportedException>();

            Should.Throw<NotSupportedException>(() => Subject.HasAccounts());
        }

        [Fact]
        public void SetupThrowsExceptionInstance()
        {
            MockFor<IAccountService>().SetupGet(s => s.HasAccounts).Throws(new NotSupportedException());

            Should.Throw<NotSupportedException>(() => Subject.HasAccounts());
        }
        
        [Fact]
        public void SetupReturnsTrueThenFalseOnlySubsequentCalls()
        {
            MockFor<IAccountService>().SetupGet(s => s.HasAccounts).Returns(true).Returns(false);

            Subject.HasAccounts().ShouldBeTrue();
            Subject.HasAccounts().ShouldBeFalse();
        }
        
        [Fact]
        public void SetupThrowsExceptionOnSecondCall()
        {
            MockFor<IAccountService>().SetupGet(s => s.HasAccounts).Returns(true).Throws(new Exception());

            Subject.HasAccounts().ShouldBeTrue();
            Should.Throw<Exception>(() => Subject.HasAccounts());
        }
    }
}
