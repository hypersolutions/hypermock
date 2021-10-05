using HyperMock.Exceptions;
using HyperMock.IntTests.Support;
using Shouldly;
using Xunit;

namespace HyperMock.IntTests
{
    public class VerifySetPropertyTests : TestBase<AccountController>
    {
        [Fact]
        public void VerifySetNever()
        {
            MockFor<IAccountService>().VerifySet(s => s.HasAccounts, Occurred.Never());
        }

        [Fact]
        public void VerifySetExactlyOnce()
        {
            Subject.Manage(true);

            MockFor<IAccountService>().VerifySet(s => s.HasAccounts, Occurred.Once());
        }

        [Fact]
        public void VerifySetExactMatch()
        {
            Subject.Manage(true);
            Subject.Manage(true);
            Subject.Manage(true);

            MockFor<IAccountService>().VerifySet(s => s.HasAccounts, Occurred.Exactly(3));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void VerifySetAtLeast(int atLeastCount)
        {
            Subject.Manage(true);
            Subject.Manage(true);
            Subject.Manage(true);

            MockFor<IAccountService>().VerifySet(s => s.HasAccounts, Occurred.AtLeast(atLeastCount));
        }

        [Fact]
        public void VerifySetNeverThrowsException()
        {
            Subject.Manage(true);

            Should.Throw<VerificationException>(
                () => MockFor<IAccountService>().VerifySet(s => s.HasAccounts, Occurred.Never()));
        }

        [Fact]
        public void VerifySetAtLeastThrowsException()
        {
            Subject.Manage(true);

            Should.Throw<VerificationException>(
                () => MockFor<IAccountService>().VerifySet(s => s.HasAccounts, Occurred.AtLeast(2)));
        }

        [Fact]
        public void VerifySetExactlyThrowsException()
        {
            Subject.Manage(true);

            Should.Throw<VerificationException>(
                () => MockFor<IAccountService>().VerifySet(s => s.HasAccounts, Occurred.Exactly(2)));
        }

        [Fact]
        public void VerifySetWithValueThrowsException()
        {
            Subject.Manage(true);

            Should.Throw<VerificationException>(
                () => MockFor<IAccountService>().VerifySet(s => s.HasAccounts, false, Occurred.Once()));
        }

        [Fact]
        public void VerifySetWithValueExactMatch()
        {
            Subject.Manage(true);

            MockFor<IAccountService>().VerifySet(s => s.HasAccounts, true, Occurred.Once());
        }

        [Fact]
        public void VerifySetWithValueNever()
        {
            Subject.Manage(true);

            MockFor<IAccountService>().VerifySet(s => s.HasAccounts, false, Occurred.Never());
        }
    }
}
