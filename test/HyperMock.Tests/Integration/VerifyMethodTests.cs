using HyperMock.Exceptions;
using HyperMock.Tests.Support;
using Shouldly;
using Xunit;

namespace HyperMock.Tests.Integration
{
    public class VerifyMethodTests : TestBase<AccountController>
    {
        [Fact]
        public void VerifyNever()
        {
            var info = new AccountInfo { Number = "12345678", CreditAmount = 100 };

            MockFor<IAccountService>().Verify(s => s.Credit(info.Number, info.CreditAmount), Occurred.Never());
        }

        [Fact]
        public void VerifyExactlyOnce()
        {
            var info = new AccountInfo {Number = "12345678", CreditAmount = 100};

            Subject.Credit(info);

            MockFor<IAccountService>().Verify(s => s.Credit(info.Number, info.CreditAmount), Occurred.Once());
        }

        [Fact]
        public void VerifyExactMatch()
        {
            var info = new AccountInfo { Number = "12345678", CreditAmount = 100 };

            Subject.Credit(info);
            Subject.Credit(info);
            Subject.Credit(info);

            MockFor<IAccountService>().Verify(s => s.Credit(info.Number, info.CreditAmount), Occurred.Exactly(3));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void VerifyAtLeast(int atLeastCount)
        {
            var info = new AccountInfo { Number = "12345678", CreditAmount = 100 };

            Subject.Credit(info);
            Subject.Credit(info);
            Subject.Credit(info);

            MockFor<IAccountService>().Verify(
                s => s.Credit(info.Number, info.CreditAmount), Occurred.AtLeast(atLeastCount));
        }

        [Fact]
        public void VerifyNeverThrowsException()
        {
            var info = new AccountInfo { Number = "12345678", CreditAmount = 100 };

            Subject.Credit(info);

            Should.Throw<VerificationException>(
                () => MockFor<IAccountService>().Verify(
                    s => s.Credit(info.Number, info.CreditAmount), Occurred.Never()));
        }

        [Fact]
        public void VerifyAtLeastThrowsException()
        {
            var info = new AccountInfo { Number = "12345678", CreditAmount = 100 };

            Subject.Credit(info);

            Should.Throw<VerificationException>(
                () => MockFor<IAccountService>().Verify(
                    s => s.Credit(info.Number, info.CreditAmount), Occurred.AtLeast(2)));
        }

        [Fact]
        public void VerifyExactlyThrowsException()
        {
            var info = new AccountInfo { Number = "12345678", CreditAmount = 100 };

            Subject.Credit(info);

            Should.Throw<VerificationException>(
                () => MockFor<IAccountService>().Verify(
                    s => s.Credit(info.Number, info.CreditAmount), Occurred.Exactly(2)));
        }
    }
}
