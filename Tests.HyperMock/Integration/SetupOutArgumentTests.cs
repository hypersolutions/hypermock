using System;
#if WINDOWS_UWP
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif
using Tests.HyperMock.Support;

namespace Tests.HyperMock.Integration
{
    [TestClass]
    public class SetupOutArgumentTests : TestBase<ConverterService>
    {
        [TestMethod]
        public void GetValueReturnsIntFromOutArg()
        {
            int value;
            MockFor<IConverter>().Setup(c => c.TryParse("123", out value)).WithOutParams(123).Returns(true);

            var result = Subject.GetValue("123");

            Assert.AreEqual(123, result);
        }
    }
}
