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
    }
}