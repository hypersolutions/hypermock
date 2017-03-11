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

        [TestMethod]
        public void VerifyExactlyTwiceWithAnyParam()
        {
            var info1 = new AccountInfo { Number = "12345678", DebitAmount = 100 };
            var info2 = new AccountInfo { Number = "87654321", DebitAmount = 200 };

            Subject.Debit(info1);
            Subject.Debit(info2);

            MockFor<IAccountService>().Verify(s => s.CanDebit(
                Param.IsAny<string>(), Param.IsAny<int>()), Occurred.Exactly(2));
        }

        [TestMethod]
        public void VerifyExactlyOnceWithSeparateParams()
        {
            var info1 = new AccountInfo { Number = "12345678", DebitAmount = 100 };
            var info2 = new AccountInfo { Number = "87654321", DebitAmount = 200 };

            Subject.Debit(info1);
            Subject.Debit(info2);

            MockFor<IAccountService>().Verify(s => s.CanDebit(info1.Number, info1.DebitAmount), Occurred.Once());
            MockFor<IAccountService>().Verify(s => s.CanDebit(info2.Number, info2.DebitAmount), Occurred.Once());
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
