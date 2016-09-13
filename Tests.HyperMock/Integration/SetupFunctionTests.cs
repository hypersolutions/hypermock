using System;
using System.Threading.Tasks;
using HyperMock;
#if WINDOWS_UWP
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif
using Tests.HyperMock.Support;

namespace Tests.HyperMock.Integration
{
    [TestClass]
    public class SetupFunctionTests : TestBase<AccountController>
    {
#if WINDOWS_UWP
        [TestMethod]
        public void SetupThrowsExceptionType()
        {
            var info = new AccountInfo { Number = "12345678", DebitAmount = 100 };
            MockFor<IAccountService>().Setup(
                s => s.CanDebit(info.Number, info.DebitAmount)).Throws<NotSupportedException>();

            Assert.ThrowsException<NotSupportedException>(() => Subject.Debit(info));
        }

        [TestMethod]
        public void SetupThrowsExceptionInstance()
        {
            var info = new AccountInfo { Number = "12345678", DebitAmount = 100 };
            MockFor<IAccountService>().Setup(
                s => s.CanDebit(info.Number, info.DebitAmount)).Throws(new NotSupportedException());

            Assert.ThrowsException<NotSupportedException>(() => Subject.Debit(info));
        }
#else
        [TestMethod, ExpectedException(typeof(NotSupportedException))]
        public void SetupThrowsExceptionType()
        {
            var info = new AccountInfo { Number = "12345678", DebitAmount = 100 };
            MockFor<IAccountService>().Setup(
                s => s.CanDebit(info.Number, info.DebitAmount)).Throws<NotSupportedException>();

            Subject.Debit(info);
        }

        [TestMethod, ExpectedException(typeof(NotSupportedException))]
        public void SetupThrowsExceptionInstance()
        {
            var info = new AccountInfo { Number = "12345678", DebitAmount = 100 };
            MockFor<IAccountService>().Setup(
                s => s.CanDebit(info.Number, info.DebitAmount)).Throws(new NotSupportedException());

            Subject.Debit(info);
        }
#endif

        [TestMethod]
        public void SetupReturnsTrue()
        {
            var info = new AccountInfo { Number = "12345678", DebitAmount = 100 };
            MockFor<IAccountService>().Setup(s => s.CanDebit(info.Number, info.DebitAmount)).Returns(true);

            Subject.Debit(info);

            MockFor<IAccountService>().Verify(s => s.Debit(info.Number, info.DebitAmount), Occurred.Once());
        }

        [TestMethod]
        public void SetupReturnsFalse()
        {
            var info = new AccountInfo { Number = "12345678", DebitAmount = 100 };
            MockFor<IAccountService>().Setup(s => s.CanDebit(info.Number, info.DebitAmount)).Returns(false);

            Subject.Debit(info);

            MockFor<IAccountService>().Verify(s => s.Debit(info.Number, info.DebitAmount), Occurred.Never());
        }

        // Diff between windows and uwp MSTest. Windows one supports DataSource and UWP supports DataRows! 
        [TestMethod]
        public void SetupReturnsTrueForPredicateIntMatch()
        {
            var data = new[] {2, 100, 999};

            foreach (var debitAmount in data)
            {
                var info = new AccountInfo {Number = "12345678", DebitAmount = debitAmount};
                MockFor<IAccountService>().Setup(
                    s => s.CanDebit(info.Number, Param.Is<int>(p => p > 1 && p < 1000))).Returns(true);

                Subject.Debit(info);

                MockFor<IAccountService>().Verify(s => s.Debit(info.Number, info.DebitAmount), Occurred.Once());
            }
        }

        // Diff between windows and uwp MSTest. Windows one supports DataSource and UWP supports DataRows! 
        [TestMethod]
        public void SetupReturnsFalseForPredicateIntMismatch()
        {
            var data = new[] {-1, 0, 1, 1000, 1001};

            foreach (var debitAmount in data)
            {
                var info = new AccountInfo {Number = "12345678", DebitAmount = debitAmount};
                MockFor<IAccountService>().Setup(
                    s => s.CanDebit(info.Number, Param.Is<int>(p => p > 1 && p < 1000))).Returns(true);

                Subject.Debit(info);

                MockFor<IAccountService>().Verify(s => s.Debit(info.Number, info.DebitAmount), Occurred.Never());
            }
        }

        // Diff between windows and uwp MSTest. Windows one supports DataSource and UWP supports DataRows! 
        [TestMethod]
        public void SetupReturnsTrueForPredicateStringMatch()
        {
            var data = new[] { "12345678", "87654321" };

            foreach (var accountNumber in data)
            {
                var info = new AccountInfo {Number = accountNumber, DebitAmount = 100};
                MockFor<IAccountService>().Setup(
                    s => s.CanDebit(Param.Is<string>(p => p == "12345678" || p == "87654321"), 100)).Returns(true);

                Subject.Debit(info);

                MockFor<IAccountService>().Verify(s => s.Debit(info.Number, info.DebitAmount), Occurred.Once());
            }
        }

        // Diff between windows and uwp MSTest. Windows one supports DataSource and UWP supports DataRows! 
        [TestMethod]
        public void SetupReturnsTrueForPredicateStringMismatch()
        {
            var data = new[] { "13572468", "75318642" };

            foreach (var accountNumber in data)
            {
                var info = new AccountInfo {Number = accountNumber, DebitAmount = 100};
                MockFor<IAccountService>().Setup(
                    s => s.CanDebit(Param.Is<string>(p => p == "12345678" || p == "87654321"), 100)).Returns(true);

                Subject.Debit(info);

                MockFor<IAccountService>().Verify(s => s.Debit(info.Number, info.DebitAmount), Occurred.Never());
            }
        }

        // Diff between windows and uwp MSTest. Windows one supports DataSource and UWP supports DataRows! 
        [TestMethod]
        public void SetupReturnsTrueForRegexStringMatch()
        {
            var data = new[] { "12345678", "87654321" };

            foreach (var accountNumber in data)
            {
                var info = new AccountInfo { Number = accountNumber, DebitAmount = 100 };
                MockFor<IAccountService>().Setup(
                    s => s.CanDebit(Param.IsRegex("^[0-9]{8}$"), 100)).Returns(true);

                Subject.Debit(info);

                MockFor<IAccountService>().Verify(s => s.Debit(info.Number, info.DebitAmount), Occurred.Once());
            }
        }

        // Diff between windows and uwp MSTest. Windows one supports DataSource and UWP supports DataRows! 
        [TestMethod]
        public void SetupReturnsTrueForRegexStringMismatch()
        {
            var data = new[] {"1234567", "123456789", "ABCDEFGH"};

            foreach (var accountNumber in data)
            {
                var info = new AccountInfo { Number = accountNumber, DebitAmount = 100 };
                MockFor<IAccountService>().Setup(
                    s => s.CanDebit(Param.IsRegex("^[0-9]{8}$"), 100)).Returns(true);

                Subject.Debit(info);

                MockFor<IAccountService>().Verify(s => s.Debit(info.Number, info.DebitAmount), Occurred.Never());
            }
        }

        [TestMethod]
        public async Task SetupAsyncGetsStatementAsync()
        {
            var info = new AccountInfo { Number = "12345678" };
            MockFor<IAccountService>().Setup(s => s.GetStatementAsync(info.Number))
                .Returns(Task.Run(() => new StatementInfo {Account = info.Number, Data = "Statement"}));

            var statement = await Subject.GetStatementAsync(info);

            Assert.AreEqual(info.Number, statement.Account);
            Assert.AreEqual("Statement", statement.Data);
        }

        [TestMethod]
        public void SetupReturnsDeferredValue()
        {
            var canDebit = false;
            var info = new AccountInfo { Number = "12345678", DebitAmount = 100 };
            // ReSharper disable once AccessToModifiedClosure
            MockFor<IAccountService>().Setup(s => s.CanDebit(info.Number, info.DebitAmount)).Returns(() => canDebit);

            canDebit = true;
            Subject.Debit(info);

            MockFor<IAccountService>().Verify(s => s.Debit(info.Number, info.DebitAmount), Occurred.Once());
        }
    }
}