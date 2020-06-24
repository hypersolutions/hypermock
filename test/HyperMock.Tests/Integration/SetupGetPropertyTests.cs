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
    }
}
