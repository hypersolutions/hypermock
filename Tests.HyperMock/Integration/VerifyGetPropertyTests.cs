﻿using HyperMock;
using HyperMock.Exceptions;
using Tests.HyperMock.Support;
using Xunit;

namespace Tests.HyperMock.Integration
{
    public class VerifyGetPropertyTests : TestBase<AccountController>
    {
        [Fact]
        public void VerifyGetNever()
        {
            MockFor<IAccountService>().VerifyGet(s => s.HasAccounts, Occurred.Never());
        }

        [Fact]
        public void VerifyGetExactlyOnce()
        {
            Subject.HasAccounts();

            MockFor<IAccountService>().VerifyGet(s => s.HasAccounts, Occurred.Once());
        }

        [Fact]
        public void VerifyGetExactMatch()
        {
            Subject.HasAccounts();
            Subject.HasAccounts();
            Subject.HasAccounts();

            MockFor<IAccountService>().VerifyGet(s => s.HasAccounts, Occurred.Exactly(3));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void VerifyGetAtLeast(int atLeastCount)
        {
            Subject.HasAccounts();
            Subject.HasAccounts();
            Subject.HasAccounts();

            MockFor<IAccountService>().VerifyGet(s => s.HasAccounts, Occurred.AtLeast(atLeastCount));
        }

        [Fact]
        public void VerifyGetNeverThrowsException()
        {
            Subject.HasAccounts();

            Assert.Throws<VerificationException>(
                () => MockFor<IAccountService>().VerifyGet(s => s.HasAccounts, Occurred.Never()));
        }

        [Fact]
        public void VerifyGetAtLeastThrowsException()
        {
            Subject.HasAccounts();

            Assert.Throws<VerificationException>(
                () => MockFor<IAccountService>().VerifyGet(s => s.HasAccounts, Occurred.AtLeast(2)));
        }

        [Fact]
        public void VerifyGetExactlyThrowsException()
        {
            Subject.HasAccounts();

            Assert.Throws<VerificationException>(
                () => MockFor<IAccountService>().VerifyGet(s => s.HasAccounts, Occurred.Exactly(2)));
        }
    }
}
