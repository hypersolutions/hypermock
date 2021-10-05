using HyperMock.IntTests.Support;
using Shouldly;
using Xunit;

namespace HyperMock.IntTests
{
    public class SetupOutAndRefArgumentTests : TestBase<ConverterService>
    {
        [Fact]
        public void GetValueReturnsIntFromOutArg()
        {
            int value;
            MockFor<IConverter>().Setup(c => c.TryParse("123", out value)).WithOutArgs(123).Returns(true);

            var result = Subject.GetValue("123");

            result.ShouldBe(123);
        }

        [Fact]
        public void FormatTextReturnsStringFromRefArg()
        {
            string text = null;
            MockFor<IConverter>().Setup(c => c.Format(ref text)).WithRefArgs("321");

            var result = Subject.FormatText("123");

            result.ShouldBe("321");
        }

        [Fact]
        public void InsertSpaceReturnsStringWithSpaceBetweenOutAndRefArgs()
        {
            string text = null;
            int value;
            MockFor<IConverter>().Setup(c => c.TryParse(ref text, out value))
                .WithOutArgs(123).WithRefArgs("321").Returns(true);

            var result = Subject.InsertSpace("123");

            result.ShouldBe("321 123");
        }
    }
}
