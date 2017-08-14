using System;
using System.Threading.Tasks;
using Xunit;

namespace HyperMock.Universal.Examples
{
    public class AccountControllerTests
    {
        [Fact]
        public void CreditAddsToAccount()
        {
            var mockService = Mock.Create<IAccountService>();
            var controller = new AccountController(mockService.Object);
            var info = new AccountInfo { Number = "1234", CreditAmount = 100 };

            controller.Credit(info);

            mockService.Verify(s => s.Credit(info.Number, info.CreditAmount), Occurred.Once());
        }

        [Fact]
        public void CreditWithInvalidAmountThrowsException()
        {
            var mockService = Mock.Create<IAccountService>();
            var controller = new AccountController(mockService.Object);
            var info = new AccountInfo { Number = "1234", CreditAmount = -100 };
            mockService.Setup(s => s.Credit(info.Number, Param.Is<int>(p => p < 1))).Throws(new NotSupportedException());

            Assert.Throws<NotSupportedException>(() => controller.Credit(info));
        }

        [Fact]
        public void CreditFailsWithUnknownAmount()
        {
            var mockService = Mock.Create<IAccountService>();
            var controller = new AccountController(mockService.Object);
            var info = new AccountInfo { Number = "1234", CreditAmount = 100 };

            controller.Credit(info);

            mockService.Verify(s => s.Credit(info.Number, 200), Occurred.Never());
        }

        [Fact]
        public void CreditWithAnyAmount()
        {
            var mockService = Mock.Create<IAccountService>();
            var controller = new AccountController(mockService.Object);
            var info = new AccountInfo { Number = "1234", CreditAmount = 100 };

            controller.Credit(info);

            mockService.Verify(s => s.Credit(info.Number, Param.IsAny<int>()), Occurred.Once());
        }

        [Fact]
        public void CreditWithAmountAboveMin()
        {
            var mockService = Mock.Create<IAccountService>();
            var controller = new AccountController(mockService.Object);
            var info = new AccountInfo { Number = "1234", CreditAmount = 2 };

            controller.Credit(info);

            mockService.Verify(s => s.Credit(info.Number, Param.Is<int>(p => p > 1)), Occurred.Once());
        }

        [Fact]
        public void CreditFailsWithAmountBelowMin()
        {
            var mockService = Mock.Create<IAccountService>();
            var controller = new AccountController(mockService.Object);
            var info = new AccountInfo { Number = "1234", CreditAmount = 1 };

            controller.Credit(info);

            mockService.Verify(s => s.Credit(info.Number, Param.Is<int>(p => p > 1)), Occurred.Never());
        }

        [Fact]
        public void DebitCorrectAccount()
        {
            var mockService = Mock.Create<IAccountService>();
            var controller = new AccountController(mockService.Object);
            var info1 = new AccountInfo { Number = "1234", DebitAmount = 100 };
            var info2 = new AccountInfo { Number = "4321", DebitAmount = 50 };
            mockService.Setup(s => s.CanDebit(info1.Number, info1.DebitAmount)).Returns(false);
            mockService.Setup(s => s.CanDebit(info2.Number, info2.DebitAmount)).Returns(true);

            controller.Debit(info2);

            mockService.Verify(s => s.Debit(info2.Number, info2.DebitAmount), Occurred.Once());
            mockService.Verify(s => s.Debit(info1.Number, info1.DebitAmount), Occurred.Never());
        }

        [Fact]
        public void DebitCorrectAccountMatchingRegex()
        {
            var mockService = Mock.Create<IAccountService>();
            var controller = new AccountController(mockService.Object);
            var info1 = new AccountInfo { Number = "1234", DebitAmount = 100 };
            var info2 = new AccountInfo { Number = "12345678", DebitAmount = 50 };
            mockService.Setup(s => s.CanDebit(Param.IsRegex("^[0-9]{4}$"), info1.DebitAmount)).Returns(true);

            controller.Debit(info1);

            mockService.Verify(s => s.Debit(info1.Number, info1.DebitAmount), Occurred.Once());
            mockService.Verify(s => s.Debit(info2.Number, info2.DebitAmount), Occurred.Never());
        }

        [Fact]
        public void HasAccounts()
        {
            var mockService = Mock.Create<IAccountService>();
            var controller = new AccountController(mockService.Object);
            mockService.SetupGet(s => s.HasAccounts).Returns(true);

            var result = controller.HasAccounts();

            Assert.True(result);
        }

        [Fact]
        public void HasNoAccounts()
        {
            var mockService = Mock.Create<IAccountService>();
            var controller = new AccountController(mockService.Object);
            mockService.SetupGet(s => s.HasAccounts).Returns(false);

            var result = controller.HasAccounts();

            Assert.False(result);
        }

        [Fact]
        public void ManageDisablesAccount()
        {
            var mockService = Mock.Create<IAccountService>();
            var controller = new AccountController(mockService.Object);

            controller.Manage(false);

            mockService.VerifySet(s => s.HasAccounts, false, Occurred.Once());
        }

        [Fact]
        public void ManageDisablesAccountThrowsException()
        {
            var mockService = Mock.Create<IAccountService>();
            var controller = new AccountController(mockService.Object);
            mockService.SetupSet(s => s.HasAccounts).SetValue(false).Throws<NotSupportedException>();

            Assert.Throws<NotSupportedException>(() => controller.Manage(false));
        }

        [Fact]
        public async Task DownloadStatementsAsyncReturnsStatement()
        {
            var mockService = Mock.Create<IAccountService>();
            var controller = new AccountController(mockService.Object);
            var info = new AccountInfo { Number = "1234" };

            mockService.Setup(s => s.DownloadStatementsAsync("1234")).Returns(Task.Run(() => "Statement"));

            var statement = await controller.DownloadStatementsAsync(info);

            Assert.Equal("Statement", statement);
        }

        [Fact]
        public void ManageDisablesAccountUsingStrictSetup()
        {
            var mockService = Mock.Create<IAccountService>(MockBehavior.Strict);
            mockService.SetupSet(s => s.HasAccounts).SetValue(false);
            var controller = new AccountController(mockService.Object);

            controller.Manage(false);

            mockService.VerifySet(s => s.HasAccounts, false, Occurred.Once());
        }
    }
}
