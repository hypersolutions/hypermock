using System;
using HyperMock.IntTests.Support;
using Shouldly;
using Xunit;

namespace HyperMock.IntTests
{
    public class SetupIndexerTests : TestBase<AccountController>
    {
        [Fact]
        public void SetupGetIndexerReturnsMatchingAccount()
        {
            MockFor<IAccountService>().SetupGet(
                s => s["12345678"]).Returns(new Account { Name = "Acc1", Number = "12345678" });
            MockFor<IAccountService>().SetupGet(
                s => s["87654321"]).Returns(new Account { Name = "Acc2", Number = "87654321" });

            var accountInfo = Subject.GetAccount("12345678");

            accountInfo.Name.ShouldBe("Acc1");
        }

        [Fact]
        public void SetupSetIndexerUpdatesIndexerValue()
        {
            var account = new Account {Number = "12345678"};
            MockFor<IAccountService>().SetupGet(s => s["12345678"]).Returns(account);

            Subject.UpdateAccount("12345678", "Acc1");

            account.Name.ShouldBe("Acc1");
        }

        [Fact]
        public void SetupSetIndexerThrowsException()
        {
            var account = new Account {Number = "12345678"};
            MockFor<IAccountService>().SetupGet(s => s["12345678"]).Returns(account);
            MockFor<IAccountService>().SetupSet(s => s["12345678"]).SetValue(account).Throws<InvalidOperationException>();

            Should.Throw<InvalidOperationException>(() => Subject.UpdateAccount("12345678", "Acc1"));
        }
    }
}
