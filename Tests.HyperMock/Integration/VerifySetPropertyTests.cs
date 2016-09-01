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
    public class VerifySetPropertyTests : TestBase<AccountController>
    {
        [TestMethod]
        public void VerifySetNever()
        {
            MockFor<IAccountService>().VerifySet(s => s.HasAccounts, Occurred.Never());
        }

        [TestMethod]
        public void VerifySetExactlyOnce()
        {
            Subject.Manage(true);

            MockFor<IAccountService>().VerifySet(s => s.HasAccounts, Occurred.Once());
        }

        [TestMethod]
        public void VerifySetExactMatch()
        {
            Subject.Manage(true);
            Subject.Manage(true);
            Subject.Manage(true);

            MockFor<IAccountService>().VerifySet(s => s.HasAccounts, Occurred.Exactly(3));
        }

        // Diff between windows and uwp MSTest. Windows one supports DataSource and UWP supports DataRows! 
        [TestMethod]
        public void VerifySetAtLeast()
        {
            var data = new[] { 1, 2, 3 };

            foreach (var atLeastCount in data)
            {
                Subject.Manage(true);
                Subject.Manage(true);
                Subject.Manage(true);

                MockFor<IAccountService>().VerifySet(s => s.HasAccounts, Occurred.AtLeast(atLeastCount));
            }
        }

#if WINDOWS_UWP
        [TestMethod]
        public void VerifySetNeverThrowsException()
        {
            Subject.Manage(true);

            Assert.ThrowsException<VerificationException>(
                () => MockFor<IAccountService>().VerifySet(s => s.HasAccounts, Occurred.Never()));
        }

        [TestMethod]
        public void VerifySetAtLeastThrowsException()
        {
            Subject.Manage(true);

            Assert.ThrowsException<VerificationException>(
                () => MockFor<IAccountService>().VerifySet(s => s.HasAccounts, Occurred.AtLeast(2)));
        }

        [TestMethod]
        public void VerifySetExactlyThrowsException()
        {
            Subject.Manage(true);

            Assert.ThrowsException<VerificationException>(
                () => MockFor<IAccountService>().VerifySet(s => s.HasAccounts, Occurred.Exactly(2)));
        }

        [TestMethod]
        public void VerifySetWithValueThrowsException()
        {
            Subject.Manage(true);

            Assert.ThrowsException<VerificationException>(
                () => MockFor<IAccountService>().VerifySet(s => s.HasAccounts, false, Occurred.Once()));
        }
#else
        [TestMethod, ExpectedException(typeof(VerificationException))]
        public void VerifySetNeverThrowsException()
        {
            Subject.Manage(true);

            MockFor<IAccountService>().VerifySet(s => s.HasAccounts, Occurred.Never());
        }

        [TestMethod, ExpectedException(typeof(VerificationException))]
        public void VerifySetAtLeastThrowsException()
        {
            Subject.Manage(true);

            MockFor<IAccountService>().VerifySet(s => s.HasAccounts, Occurred.AtLeast(2));
        }

        [TestMethod, ExpectedException(typeof(VerificationException))]
        public void VerifySetExactlyThrowsException()
        {
            Subject.Manage(true);

            MockFor<IAccountService>().VerifySet(s => s.HasAccounts, Occurred.Exactly(2));
        }

        [TestMethod, ExpectedException(typeof(VerificationException))]
        public void VerifySetWithValueThrowsException()
        {
            Subject.Manage(true);

            MockFor<IAccountService>().VerifySet(s => s.HasAccounts, false, Occurred.Once());
        }
#endif

        [TestMethod]
        public void VerifySetWithValueExactMatch()
        {
            Subject.Manage(true);

            MockFor<IAccountService>().VerifySet(s => s.HasAccounts, true, Occurred.Once());
        }

        [TestMethod]
        public void VerifySetWithValueNever()
        {
            Subject.Manage(true);

            MockFor<IAccountService>().VerifySet(s => s.HasAccounts, false, Occurred.Never());
        }
    }
}