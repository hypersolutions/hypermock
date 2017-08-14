using System;
using Tests.HyperMock.Support;
using Xunit;

namespace Tests.HyperMock.Integration
{
    public class SetupMethodTests : TestBase<AccountController>
    {
        [Fact]
        public void SetupThrowsExceptionType()
        {
            var info = new AccountInfo { Number = "12345678", CreditAmount = 100 };
            MockFor<IAccountService>().Setup(
                s => s.Credit(info.Number, info.CreditAmount)).Throws<NotSupportedException>();

            Assert.Throws<NotSupportedException>(() => Subject.Credit(info));
        }

        [Fact]
        public void SetupThrowsExceptionInstance()
        {
            var info = new AccountInfo { Number = "12345678", CreditAmount = 100 };
            MockFor<IAccountService>().Setup(
                s => s.Credit(info.Number, info.CreditAmount)).Throws(new NotSupportedException());

            Assert.Throws<NotSupportedException>(() => Subject.Credit(info));
        }
    }
}
