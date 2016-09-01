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
    public class VerifyGetPropertyTests : TestBase<AccountController>
    {
        [TestMethod]
        public void VerifyGetNever()
        {
            MockFor<IAccountService>().VerifyGet(s => s.HasAccounts, Occurred.Never());
        }

        [TestMethod]
        public void VerifyGetExactlyOnce()
        {
            Subject.HasAccounts();

            MockFor<IAccountService>().VerifyGet(s => s.HasAccounts, Occurred.Once());
        }

        [TestMethod]
        public void VerifyGetExactMatch()
        {
            Subject.HasAccounts();
            Subject.HasAccounts();
            Subject.HasAccounts();

            MockFor<IAccountService>().VerifyGet(s => s.HasAccounts, Occurred.Exactly(3));
        }

        // Diff between windows and uwp MSTest. Windows one supports DataSource and UWP supports DataRows! 
        [TestMethod]
        public void VerifyGetAtLeast()
        {
            var data = new[] { 1, 2, 3 };

            foreach (var atLeastCount in data)
            {
                Subject.HasAccounts();
                Subject.HasAccounts();
                Subject.HasAccounts();

                MockFor<IAccountService>().VerifyGet(s => s.HasAccounts, Occurred.AtLeast(atLeastCount));
            }
        }

#if WINDOWS_UWP
        [TestMethod]
        public void VerifyGetNeverThrowsException()
        {
            Subject.HasAccounts();

            Assert.ThrowsException<VerificationException>(
                () => MockFor<IAccountService>().VerifyGet(s => s.HasAccounts, Occurred.Never()));
        }

        [TestMethod]
        public void VerifyGetAtLeastThrowsException()
        {
            Subject.HasAccounts();

            Assert.ThrowsException<VerificationException>(
                () => MockFor<IAccountService>().VerifyGet(s => s.HasAccounts, Occurred.AtLeast(2)));
        }

        [TestMethod]
        public void VerifyGetExactlyThrowsException()
        {
            Subject.HasAccounts();

            Assert.ThrowsException<VerificationException>(
                () => MockFor<IAccountService>().VerifyGet(s => s.HasAccounts, Occurred.Exactly(2)));
        }
#else
        [TestMethod, ExpectedException(typeof(VerificationException))]
        public void VerifyGetNeverThrowsException()
        {
            Subject.HasAccounts();

            MockFor<IAccountService>().VerifyGet(s => s.HasAccounts, Occurred.Never());
        }

        [TestMethod, ExpectedException(typeof(VerificationException))]
        public void VerifyGetAtLeastThrowsException()
        {
            Subject.HasAccounts();

            MockFor<IAccountService>().VerifyGet(s => s.HasAccounts, Occurred.AtLeast(2));
        }

        [TestMethod, ExpectedException(typeof(VerificationException))]
        public void VerifyGetExactlyThrowsException()
        {
            Subject.HasAccounts();

            MockFor<IAccountService>().VerifyGet(s => s.HasAccounts, Occurred.Exactly(2));
        }
#endif
    }
}
