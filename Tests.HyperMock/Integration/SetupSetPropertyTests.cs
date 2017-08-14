using System;
using Tests.HyperMock.Support;
using Xunit;

namespace Tests.HyperMock.Integration
{
    public class SetupSetPropertyTests : TestBase<AccountController>
    {
        [Fact]
        public void SetupSetThrowsExceptionType()
        {
            MockFor<IAccountService>().SetupSet(s => s.HasAccounts).SetValue(true).Throws<NotSupportedException>();

            Assert.Throws<NotSupportedException>(() => Subject.Manage(true));
        }

        [Fact]
        public void SetupSetThrowsExceptionInstance()
        {
            MockFor<IAccountService>().SetupSet(s => s.HasAccounts).SetValue(true).Throws(new NotSupportedException());

            Assert.Throws<NotSupportedException>(() => Subject.Manage(true));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SetupSetThrowsExceptionTypeForAny(bool value)
        {
            MockFor<IAccountService>().SetupSet(s => s.HasAccounts).AnySetValue().Throws<NotSupportedException>();

            Assert.Throws<NotSupportedException>(() => Subject.Manage(value));
        }

        [Fact]
        public void SetupSetDoesNotThrowExceptionType()
        {
            MockFor<IAccountService>().SetupSet(s => s.HasAccounts).SetValue(false).Throws<NotSupportedException>();

            Subject.Manage(true);
        }

        [Fact]
        public void SetupSetDoesNotThrowExceptionInstance()
        {
            MockFor<IAccountService>().SetupSet(s => s.HasAccounts).SetValue(false).Throws(new NotSupportedException());

            Subject.Manage(true);
        }
    }
}