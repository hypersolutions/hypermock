#if WINDOWS_UWP
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif
using Tests.HyperMock.Support;

namespace Tests.HyperMock.Integration
{
    [TestClass]
    public class SetupGetIndexerTests : TestBase<AccountController>
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
    }
}