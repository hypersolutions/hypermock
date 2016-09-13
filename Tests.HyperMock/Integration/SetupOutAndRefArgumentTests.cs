#if WINDOWS_UWP
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif
using Tests.HyperMock.Support;

namespace Tests.HyperMock.Integration
{
    [TestClass]
    public class SetupOutAndRefArgumentTests : TestBase<ConverterService>
    {
        [TestMethod]
        public void GetValueReturnsIntFromOutArg()
        {
            int value;
            MockFor<IConverter>().Setup(c => c.TryParse("123", out value)).WithOutArgs(123).Returns(true);

            var result = Subject.GetValue("123");

            Assert.AreEqual(123, result);
        }

        [TestMethod]
        public void FormatTextReturnsStringFromRefArg()
        {
            string text = null;
            MockFor<IConverter>().Setup(c => c.Format(ref text)).WithRefArgs("321");

            var result = Subject.FormatText("123");

            Assert.AreEqual("321", result);
        }

        [TestMethod]
        public void InsertSpaceReturnsStringWithSpaceBetweenOutAndRefArgs()
        {
            string text = null;
            int value;
            MockFor<IConverter>().Setup(c => c.TryParse(ref text, out value))
                .WithOutArgs(123).WithRefArgs("321").Returns(true);

            var result = Subject.InsertSpace("123");

            Assert.AreEqual("321 123", result);
        }
    }
}
