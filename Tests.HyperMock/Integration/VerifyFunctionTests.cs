using HyperMock;
using HyperMock.Exceptions;
#if WINDOWS_UWP
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif
using Tests.HyperMock.Support;

namespace Tests.HyperMock.Integration
{
    [TestClass]
    public class VerifyFunctionTests : TestBase<AccountController>
    {
        [TestMethod]
        public void VerifyNever()
        {
            var info = new AccountInfo { Number = "12345678", DebitAmount = 100 };

            MockFor<IAccountService>().Verify(s => s.CanDebit(info.Number, info.DebitAmount), Occurred.Never());
        }

        [TestMethod]
        public void VerifyExactlyOnce()
        {
            var info = new AccountInfo {Number = "12345678", DebitAmount = 100};

            Subject.Debit(info);

            MockFor<IAccountService>().Verify(s => s.CanDebit(info.Number, info.DebitAmount), Occurred.Once());
        }

        [TestMethod]
        public void VerifyExactMatch()
        {
            var info = new AccountInfo { Number = "12345678", DebitAmount = 100 };

            Subject.Debit(info);
            Subject.Debit(info);
            Subject.Debit(info);

            MockFor<IAccountService>().Verify(s => s.CanDebit(info.Number, info.DebitAmount), Occurred.Exactly(3));
        }

        // Diff between windows and uwp MSTest. Windows one supports DataSource and UWP supports DataRows! 
        [TestMethod]
        public void VerifyAtLeast()
        {
            var data = new[] {1, 2, 3};

            foreach (var atLeastCount in data)
            {
                var info = new AccountInfo {Number = "12345678", DebitAmount = 100};

                Subject.Debit(info);
                Subject.Debit(info);
                Subject.Debit(info);

                MockFor<IAccountService>().Verify(
                    s => s.CanDebit(info.Number, info.DebitAmount), Occurred.AtLeast(atLeastCount));
            }
        }

#if WINDOWS_UWP
        [TestMethod]
        public void VerifyNeverThrowsException()
        {
            var info = new AccountInfo { Number = "12345678", DebitAmount = 100 };

            Subject.Debit(info);

            Assert.ThrowsException<VerificationException>(
                () => MockFor<IAccountService>().Verify(
                    s => s.CanDebit(info.Number, info.DebitAmount), Occurred.Never()));
        }

        [TestMethod]
        public void VerifyAtLeastThrowsException()
        {
            var info = new AccountInfo { Number = "12345678", DebitAmount = 100 };

            Subject.Debit(info);

            Assert.ThrowsException<VerificationException>(
                () => MockFor<IAccountService>().Verify(
                    s => s.CanDebit(info.Number, info.DebitAmount), Occurred.AtLeast(2)));
        }

        [TestMethod]
        public void VerifyExactlyThrowsException()
        {
            var info = new AccountInfo { Number = "12345678", DebitAmount = 100 };

            Subject.Debit(info);

            Assert.ThrowsException<VerificationException>(
                () => MockFor<IAccountService>().Verify(
                    s => s.CanDebit(info.Number, info.DebitAmount), Occurred.Exactly(2)));
        }
#else
        [TestMethod, ExpectedException(typeof(VerificationException))]
        public void VerifyNeverThrowsException()
        {
            var info = new AccountInfo { Number = "12345678", DebitAmount = 100 };

            Subject.Debit(info);

            MockFor<IAccountService>().Verify(s => s.CanDebit(info.Number, info.DebitAmount), Occurred.Never());
        }

        [TestMethod, ExpectedException(typeof(VerificationException))]
        public void VerifyAtLeastThrowsException()
        {
            var info = new AccountInfo { Number = "12345678", DebitAmount = 100 };

            Subject.Debit(info);

            MockFor<IAccountService>().Verify(s => s.CanDebit(info.Number, info.DebitAmount), Occurred.AtLeast(2));
        }

        [TestMethod, ExpectedException(typeof(VerificationException))]
        public void VerifyExactlyThrowsException()
        {
            var info = new AccountInfo { Number = "12345678", DebitAmount = 100 };

            Subject.Debit(info);

            MockFor<IAccountService>().Verify(s => s.CanDebit(info.Number, info.DebitAmount), Occurred.Exactly(2));
        }
#endif
    }
}
