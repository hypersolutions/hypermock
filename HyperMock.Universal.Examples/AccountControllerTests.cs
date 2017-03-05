using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace HyperMock.Universal.Examples
{
    [TestClass]
    public class AccountControllerTests
    {
        [TestMethod]
        public void CreditAddsToAccount()
        {
            var mockService = Mock.Create<IAccountService>();
            var controller = new AccountController(mockService.Object);
            var info = new AccountInfo { Number = "1234", CreditAmount = 100 };

            controller.Credit(info);

            mockService.Verify(s => s.Credit(info.Number, info.CreditAmount), Occurred.Once());
        }

        [TestMethod]
        public void CreditWithInvalidAmountThrowsException()
        {
            var mockService = Mock.Create<IAccountService>();
            var controller = new AccountController(mockService.Object);
            var info = new AccountInfo { Number = "1234", CreditAmount = -100 };
            mockService.Setup(s => s.Credit(info.Number, Param.Is<int>(p => p < 1))).Throws(new NotSupportedException());

            Assert.ThrowsException<NotSupportedException>(() => controller.Credit(info));
        }

        [TestMethod]
        public void CreditFailsWithUnknownAmount()
        {
            var mockService = Mock.Create<IAccountService>();
            var controller = new AccountController(mockService.Object);
            var info = new AccountInfo { Number = "1234", CreditAmount = 100 };

            controller.Credit(info);

            mockService.Verify(s => s.Credit(info.Number, 200), Occurred.Never());
        }

        [TestMethod]
        public void CreditWithAnyAmount()
        {
            var mockService = Mock.Create<IAccountService>();
            var controller = new AccountController(mockService.Object);
            var info = new AccountInfo { Number = "1234", CreditAmount = 100 };

            controller.Credit(info);

            mockService.Verify(s => s.Credit(info.Number, Param.IsAny<int>()), Occurred.Once());
        }

        [TestMethod]
        public void CreditWithAmountAboveMin()
        {
            var mockService = Mock.Create<IAccountService>();
            var controller = new AccountController(mockService.Object);
            var info = new AccountInfo { Number = "1234", CreditAmount = 2 };

            controller.Credit(info);

            mockService.Verify(s => s.Credit(info.Number, Param.Is<int>(p => p > 1)), Occurred.Once());
        }

        [TestMethod]
        public void CreditFailsWithAmountBelowMin()
        {
            var mockService = Mock.Create<IAccountService>();
            var controller = new AccountController(mockService.Object);
            var info = new AccountInfo { Number = "1234", CreditAmount = 1 };

            controller.Credit(info);

            mockService.Verify(s => s.Credit(info.Number, Param.Is<int>(p => p > 1)), Occurred.Never());
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        public void HasAccounts()
        {
            var mockService = Mock.Create<IAccountService>();
            var controller = new AccountController(mockService.Object);
            mockService.SetupGet(s => s.HasAccounts).Returns(true);

            var result = controller.HasAccounts();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HasNoAccounts()
        {
            var mockService = Mock.Create<IAccountService>();
            var controller = new AccountController(mockService.Object);
            mockService.SetupGet(s => s.HasAccounts).Returns(false);

            var result = controller.HasAccounts();

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ManageDisablesAccount()
        {
            var mockService = Mock.Create<IAccountService>();
            var controller = new AccountController(mockService.Object);

            controller.Manage(false);

            mockService.VerifySet(s => s.HasAccounts, false, Occurred.Once());
        }

        [TestMethod]
        public void ManageDisablesAccountThrowsException()
        {
            var mockService = Mock.Create<IAccountService>();
            var controller = new AccountController(mockService.Object);
            mockService.SetupSet(s => s.HasAccounts).SetValue(false).Throws<NotSupportedException>();

            Assert.ThrowsException<NotSupportedException>(() => controller.Manage(false));
        }

        [TestMethod]
        public async Task DownloadStatementsAsyncReturnsStatement()
        {
            var mockService = Mock.Create<IAccountService>();
            var controller = new AccountController(mockService.Object);
            var info = new AccountInfo { Number = "1234" };

            mockService.Setup(s => s.DownloadStatementsAsync("1234")).Returns(Task.Run(() => "Statement"));

            var statement = await controller.DownloadStatementsAsync(info);

            Assert.AreEqual("Statement", statement);
        }

        [TestMethod]
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
