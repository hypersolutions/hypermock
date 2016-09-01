using System;
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
    public class SetupSetPropertyTests : TestBase<AccountController>
    {
#if WINDOWS_UWP
        [TestMethod]
        public void SetupThrowsExceptionType()
        {
            MockFor<IAccountService>().SetupSet(s => s.HasAccounts).SetValue(true).Throws<NotSupportedException>();

            Assert.ThrowsException<NotSupportedException>(() => Subject.Manage(true));
        }

        [TestMethod]
        public void SetupThrowsExceptionInstance()
        {
            MockFor<IAccountService>().SetupSet(s => s.HasAccounts).SetValue(true).Throws(new NotSupportedException());

            Assert.ThrowsException<NotSupportedException>(() => Subject.Manage(true));
        }

        // Diff between windows and uwp MSTest. Windows one supports DataSource and UWP supports DataRows! 
        [TestMethod]
        public void SetupThrowsExceptionTypeForAny()
        {
            var data = new[] {true, false};

            foreach (var value in data)
            {
                MockFor<IAccountService>().SetupSet(s => s.HasAccounts).AnySetValue().Throws<NotSupportedException>();

                Assert.ThrowsException<NotSupportedException>(() => Subject.Manage(value));
            }
        }
#else
        [TestMethod, ExpectedException(typeof(NotSupportedException))]
        public void SetupThrowsExceptionType()
        {
            MockFor<IAccountService>().SetupSet(s => s.HasAccounts).SetValue(true).Throws<NotSupportedException>();

            Subject.Manage(true);
        }

        [TestMethod, ExpectedException(typeof(NotSupportedException))]
        public void SetupThrowsExceptionInstance()
        {
            MockFor<IAccountService>().SetupSet(s => s.HasAccounts).SetValue(true).Throws(new NotSupportedException());

            Subject.Manage(true);
        }

        // Diff between windows and uwp MSTest. Windows one supports DataSource and UWP supports DataRows! 
        [TestMethod, ExpectedException(typeof(NotSupportedException))]
        public void SetupThrowsExceptionTypeForAny()
        {
            var data = new[] {true, false};

            foreach (var value in data)
            {
                MockFor<IAccountService>().SetupSet(s => s.HasAccounts).AnySetValue().Throws<NotSupportedException>();

                Subject.Manage(value);
            }
        }
#endif

        [TestMethod]
        public void SetupDoesNotThrowExceptionType()
        {
            MockFor<IAccountService>().SetupSet(s => s.HasAccounts).SetValue(false).Throws<NotSupportedException>();

            try
            {
                Subject.Manage(true);
            }
            catch (VerificationException verifyError)
            {
                Assert.Fail($"Successful verification threw a VerificationException: {verifyError}");
            }
        }

        [TestMethod]
        public void SetupDoesNotThrowExceptionInstance()
        {
            MockFor<IAccountService>().SetupSet(s => s.HasAccounts).SetValue(false).Throws(new NotSupportedException());

            try
            {
                Subject.Manage(true);
            }
            catch (VerificationException verifyError)
            {
                Assert.Fail($"Successful verification threw a VerificationException: {verifyError}");
            }
        }
    }
}