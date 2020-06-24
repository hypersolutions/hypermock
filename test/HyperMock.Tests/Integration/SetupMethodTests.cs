using System;
using HyperMock.Tests.Support;
using Shouldly;
using Xunit;

namespace HyperMock.Tests.Integration
{
    public class SetupMethodTests : TestBase<AccountController>
    {
        [Fact]
        public void SetupThrowsExceptionType()
        {
            var info = new AccountInfo { Number = "12345678", CreditAmount = 100 };
            MockFor<IAccountService>().Setup(
                s => s.Credit(info.Number, info.CreditAmount)).Throws<NotSupportedException>();

            Should.Throw<NotSupportedException>(() => Subject.Credit(info));
        }

        [Fact]
        public void SetupThrowsExceptionInstance()
        {
            var info = new AccountInfo { Number = "12345678", CreditAmount = 100 };
            MockFor<IAccountService>().Setup(
                s => s.Credit(info.Number, info.CreditAmount)).Throws(new NotSupportedException());

            Should.Throw<NotSupportedException>(() => Subject.Credit(info));
        }
    }
}
