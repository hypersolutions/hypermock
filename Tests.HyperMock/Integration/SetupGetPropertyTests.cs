using System;
using Tests.HyperMock.Support;
using Xunit;

namespace Tests.HyperMock.Integration
{
    public class SetupGetPropertyTests : TestBase<AccountController>
    {
        [Fact]
        public void SetupThrowsExceptionType()
        {
            MockFor<IAccountService>().SetupGet(s => s.HasAccounts).Throws<NotSupportedException>();

            Assert.Throws<NotSupportedException>(() => Subject.HasAccounts());
        }

        [Fact]
        public void SetupThrowsExceptionInstance()
        {
            MockFor<IAccountService>().SetupGet(s => s.HasAccounts).Throws(new NotSupportedException());

            Assert.Throws<NotSupportedException>(() => Subject.HasAccounts());
        }

        [Fact]
        public void SetupReturnsTrueThenFalseOnlySubsequentCalls()
        {
            MockFor<IAccountService>().SetupGet(s => s.HasAccounts).Returns(true).Returns(false);

            Assert.True(Subject.HasAccounts());
            Assert.False(Subject.HasAccounts());
        }

        [Fact]
        public void SetupThrowsExceptionOnSecondCall()
        {
            MockFor<IAccountService>().SetupGet(s => s.HasAccounts).Returns(true).Throws(new Exception());

            Assert.True(Subject.HasAccounts());
            Assert.Throws<Exception>(() => Subject.HasAccounts());
        }
    }
}