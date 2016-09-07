using System;
using HyperMock;
#if WINDOWS_UWP
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif
using Tests.HyperMock.Support;

namespace Tests.HyperMock.Integration
{
    [TestClass]
    public class SetupIndexerTests : TestBase<AccountController>
    {
        [TestMethod]
        public void SetupGetIndexerReturnsMatchingAccount()
        {
            MockFor<IAccountService>().SetupGet(
                s => s["12345678"]).Returns(new Account { Name = "Acc1", Number = "12345678" });
            MockFor<IAccountService>().SetupGet(
                s => s["87654321"]).Returns(new Account { Name = "Acc2", Number = "87654321" });

            var accountInfo = Subject.GetAccount("12345678");

            Assert.AreEqual("Acc1", accountInfo.Name);
        }

        [TestMethod]
        public void SetupSetIndexerUpdatesIndexerValue()
        {
            MockFor<IAccountService>().SetupGet(s => s["12345678"]).Returns(new Account { Number = "12345678" });

            Subject.UpdateAccount("12345678", "Acc1");

            MockFor<IAccountService>().VerifySet(s => s["12345678"], Param.Is<Account>(a => a.Name == "Acc1"), Occurred.Once());
        }

        #if WINDOWS_UWP
        [TestMethod]
        public void SetupSetIndexerThrowsException()
        {
            MockFor<IAccountService>().SetupGet(s => s["12345678"]).Returns(new Account { Number = "12345678" });
            MockFor<IAccountService>().SetupSet(s => s["12345678"]).Throws<InvalidOperationException>();

            Assert.ThrowsException<InvalidOperationException>(() => Subject.UpdateAccount("12345678", "Acc1"));
        }
#else
        [TestMethod, ExpectedException(typeof(InvalidOperationException))]
        public void SetupSetIndexerThrowsException()
        {
            MockFor<IAccountService>().SetupGet(s => s["12345678"]).Returns(new Account { Number = "12345678" });
            MockFor<IAccountService>().SetupSet(s => s["12345678"]).Throws<InvalidOperationException>();

            Subject.UpdateAccount("12345678", "Acc1");
        }
#endif
    }
}