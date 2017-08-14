using System;
using System.Threading.Tasks;
using HyperMock;
using Tests.HyperMock.Support;
using Xunit;

namespace Tests.HyperMock.Integration
{
    public class SetupFunctionTests : TestBase<AccountController>
    {
        [Fact]
        public void SetupThrowsExceptionType()
        {
            var info = new AccountInfo { Number = "12345678", DebitAmount = 100 };
            MockFor<IAccountService>().Setup(
                s => s.CanDebit(info.Number, info.DebitAmount)).Throws<NotSupportedException>();

            Assert.Throws<NotSupportedException>(() => Subject.Debit(info));
        }

        [Fact]
        public void SetupThrowsExceptionInstance()
        {
            var info = new AccountInfo { Number = "12345678", DebitAmount = 100 };
            MockFor<IAccountService>().Setup(
                s => s.CanDebit(info.Number, info.DebitAmount)).Throws(new NotSupportedException());

            Assert.Throws<NotSupportedException>(() => Subject.Debit(info));
        }

        [Fact]
        public void SetupReturnsTrue()
        {
            var info = new AccountInfo { Number = "12345678", DebitAmount = 100 };
            MockFor<IAccountService>().Setup(s => s.CanDebit(info.Number, info.DebitAmount)).Returns(true);

            Subject.Debit(info);

            MockFor<IAccountService>().Verify(s => s.Debit(info.Number, info.DebitAmount), Occurred.Once());
        }

        [Fact]
        public void SetupReturnsFalse()
        {
            var info = new AccountInfo { Number = "12345678", DebitAmount = 100 };
            MockFor<IAccountService>().Setup(s => s.CanDebit(info.Number, info.DebitAmount)).Returns(false);

            Subject.Debit(info);

            MockFor<IAccountService>().Verify(s => s.Debit(info.Number, info.DebitAmount), Occurred.Never());
        }

        [Theory]
        [InlineData(2)]
        [InlineData(100)]
        [InlineData(999)]
        public void SetupReturnsTrueForPredicateIntMatch(int debitAmount)
        {
            var info = new AccountInfo {Number = "12345678", DebitAmount = debitAmount};
            MockFor<IAccountService>().Setup(
                s => s.CanDebit(info.Number, Param.Is<int>(p => p > 1 && p < 1000))).Returns(true);

            Subject.Debit(info);

            MockFor<IAccountService>().Verify(s => s.Debit(info.Number, info.DebitAmount), Occurred.Once());
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(1000)]
        [InlineData(1001)]
        public void SetupReturnsFalseForPredicateIntMismatch(int debitAmount)
        {
            var info = new AccountInfo {Number = "12345678", DebitAmount = debitAmount};
            MockFor<IAccountService>().Setup(
                s => s.CanDebit(info.Number, Param.Is<int>(p => p > 1 && p < 1000))).Returns(true);

            Subject.Debit(info);

            MockFor<IAccountService>().Verify(s => s.Debit(info.Number, info.DebitAmount), Occurred.Never());
        }

        [Theory]
        [InlineData("12345678")]
        [InlineData("87654321")]
        public void SetupReturnsTrueForPredicateStringMatch(string accountNumber)
        {
            var info = new AccountInfo {Number = accountNumber, DebitAmount = 100};
            MockFor<IAccountService>().Setup(
                s => s.CanDebit(Param.Is<string>(p => p == "12345678" || p == "87654321"), 100)).Returns(true);

            Subject.Debit(info);

            MockFor<IAccountService>().Verify(s => s.Debit(info.Number, info.DebitAmount), Occurred.Once());
        }

        [Theory]
        [InlineData("13572468")]
        [InlineData("75318642")]
        public void SetupReturnsTrueForPredicateStringMismatch(string accountNumber)
        {
            var info = new AccountInfo {Number = accountNumber, DebitAmount = 100};
            MockFor<IAccountService>().Setup(
                s => s.CanDebit(Param.Is<string>(p => p == "12345678" || p == "87654321"), 100)).Returns(true);

            Subject.Debit(info);

            MockFor<IAccountService>().Verify(s => s.Debit(info.Number, info.DebitAmount), Occurred.Never());
        }

        [Theory]
        [InlineData("12345678")]
        [InlineData("87654321")]
        public void SetupReturnsTrueForRegexStringMatch(string accountNumber)
        {
            var info = new AccountInfo { Number = accountNumber, DebitAmount = 100 };
            MockFor<IAccountService>().Setup(
                s => s.CanDebit(Param.IsRegex("^[0-9]{8}$"), 100)).Returns(true);

            Subject.Debit(info);

            MockFor<IAccountService>().Verify(s => s.Debit(info.Number, info.DebitAmount), Occurred.Once());
        }

        [Theory]
        [InlineData("1234567")]
        [InlineData("123456789")]
        [InlineData("ABCDEFGH")]
        public void SetupReturnsTrueForRegexStringMismatch(string accountNumber)
        {
            var info = new AccountInfo { Number = accountNumber, DebitAmount = 100 };
            MockFor<IAccountService>().Setup(
                s => s.CanDebit(Param.IsRegex("^[0-9]{8}$"), 100)).Returns(true);

            Subject.Debit(info);

            MockFor<IAccountService>().Verify(s => s.Debit(info.Number, info.DebitAmount), Occurred.Never());
        }

        [Fact]
        public async Task SetupAsyncGetsStatementAsync()
        {
            var info = new AccountInfo { Number = "12345678" };
            MockFor<IAccountService>().Setup(s => s.GetStatementAsync(info.Number))
                .Returns(Task.Run(() => new StatementInfo {Account = info.Number, Data = "Statement"}));

            var statement = await Subject.GetStatementAsync(info);

            Assert.Equal(info.Number, statement.Account);
            Assert.Equal("Statement", statement.Data);
        }

        [Fact]
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