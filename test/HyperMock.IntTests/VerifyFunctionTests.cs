using HyperMock.Exceptions;
using HyperMock.IntTests.Support;
using Shouldly;
using Xunit;

namespace HyperMock.IntTests
{
    public class VerifyFunctionTests : TestBase<AccountController>
    {
        [Fact]
        public void VerifyNever()
        {
            var info = new AccountInfo { Number = "12345678", DebitAmount = 100 };

            MockFor<IAccountService>().Verify(s => s.CanDebit(info.Number, info.DebitAmount), Occurred.Never());
        }

        [Fact]
        public void VerifyExactlyOnce()
        {
            var info = new AccountInfo {Number = "12345678", DebitAmount = 100};

            Subject.Debit(info);

            MockFor<IAccountService>().Verify(s => s.CanDebit(info.Number, info.DebitAmount), Occurred.Once());
        }

        [Fact]
        public void VerifyExactMatch()
        {
            var info = new AccountInfo { Number = "12345678", DebitAmount = 100 };

            Subject.Debit(info);
            Subject.Debit(info);
            Subject.Debit(info);

            MockFor<IAccountService>().Verify(s => s.CanDebit(info.Number, info.DebitAmount), Occurred.Exactly(3));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void VerifyAtLeast(int atLeastCount)
        {
            var info = new AccountInfo {Number = "12345678", DebitAmount = 100};

            Subject.Debit(info);
            Subject.Debit(info);
            Subject.Debit(info);

            MockFor<IAccountService>().Verify(
                s => s.CanDebit(info.Number, info.DebitAmount), Occurred.AtLeast(atLeastCount));
        }

        [Fact]
        public void VerifyExactlyTwiceWithAnyParam()
        {
            var info1 = new AccountInfo { Number = "12345678", DebitAmount = 100 };
            var info2 = new AccountInfo { Number = "87654321", DebitAmount = 200 };

            Subject.Debit(info1);
            Subject.Debit(info2);

            MockFor<IAccountService>().Verify(s => s.CanDebit(
                Param.IsAny<string>(), Param.IsAny<int>()), Occurred.Exactly(2));
        }

        [Fact]
        public void VerifyExactlyOnceWithSeparateParams()
        {
            var info1 = new AccountInfo { Number = "12345678", DebitAmount = 100 };
            var info2 = new AccountInfo { Number = "87654321", DebitAmount = 200 };

            Subject.Debit(info1);
            Subject.Debit(info2);

            MockFor<IAccountService>().Verify(s => s.CanDebit(info1.Number, info1.DebitAmount), Occurred.Once());
            MockFor<IAccountService>().Verify(s => s.CanDebit(info2.Number, info2.DebitAmount), Occurred.Once());
        }

        [Fact]
        public void VerifyNeverThrowsException()
        {
            var info = new AccountInfo { Number = "12345678", DebitAmount = 100 };

            Subject.Debit(info);

            Should.Throw<VerificationException>(
                () => MockFor<IAccountService>().Verify(
                    s => s.CanDebit(info.Number, info.DebitAmount), Occurred.Never()));
        }

        [Fact]
        public void VerifyAtLeastThrowsException()
        {
            var info = new AccountInfo { Number = "12345678", DebitAmount = 100 };

            Subject.Debit(info);

            Should.Throw<VerificationException>(
                () => MockFor<IAccountService>().Verify(
                    s => s.CanDebit(info.Number, info.DebitAmount), Occurred.AtLeast(2)));
        }

        [Fact]
        public void VerifyExactlyThrowsException()
        {
            var info = new AccountInfo { Number = "12345678", DebitAmount = 100 };

            Subject.Debit(info);

            Should.Throw<VerificationException>(
                () => MockFor<IAccountService>().Verify(
                    s => s.CanDebit(info.Number, info.DebitAmount), Occurred.Exactly(2)));
        }
    }
}
