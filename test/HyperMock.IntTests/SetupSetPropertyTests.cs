using System;
using HyperMock.IntTests.Support;
using Shouldly;
using Xunit;

namespace HyperMock.IntTests
{
    public class SetupSetPropertyTests : TestBase<AccountController>
    {
        [Fact]
        public void SetupSetThrowsExceptionType()
        {
            MockFor<IAccountService>().SetupSet(s => s.HasAccounts).SetValue(true).Throws<NotSupportedException>();

            Should.Throw<NotSupportedException>(() => Subject.Manage(true));
        }

        [Fact]
        public void SetupSetThrowsExceptionInstance()
        {
            MockFor<IAccountService>().SetupSet(s => s.HasAccounts).SetValue(true).Throws(new NotSupportedException());

            Should.Throw<NotSupportedException>(() => Subject.Manage(true));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SetupSetThrowsExceptionTypeForAny(bool value)
        {
            MockFor<IAccountService>().SetupSet(s => s.HasAccounts).AnySetValue().Throws<NotSupportedException>();

            Should.Throw<NotSupportedException>(() => Subject.Manage(value));
        }

        [Fact]
        public void SetupSetDoesNotThrowExceptionType()
        {
            MockFor<IAccountService>().SetupSet(s => s.HasAccounts).SetValue(false).Throws<NotSupportedException>();

            Should.NotThrow(() => Subject.Manage(true));
        }

        [Fact]
        public void SetupSetDoesNotThrowExceptionInstance()
        {
            MockFor<IAccountService>().SetupSet(s => s.HasAccounts).SetValue(false).Throws(new NotSupportedException());

            Should.NotThrow(() => Subject.Manage(true));
        }
    }
}
