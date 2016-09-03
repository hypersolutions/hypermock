using HyperMock.Matchers;
#if WINDOWS_UWP
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace Tests.HyperMock.Matchers
{
    [TestClass]
    public class AnyParameterMatcherTests
    {
        private AnyParameterMatcher _matcher;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _matcher = new AnyParameterMatcher();
        }

        [TestMethod]
        public void IsMatchReturnsTrueForNull()
        {
            var result = _matcher.IsMatch(null, null);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsMatchReturnsTrueForSameReferenceTypeInstance()
        {
            var expected = new object();
            var actual = expected;

            var result = _matcher.IsMatch(expected, actual);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsMatchReturnsTrueForDifferentReferenceTypeInstance()
        {
            var expected = new object();
            var actual = new object();

            var result = _matcher.IsMatch(expected, actual);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsMatchReturnsTrueForSameValueTypeInstance()
        {
            var expected = 10;
            var actual = expected;

            var result = _matcher.IsMatch(expected, actual);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsMatchReturnsFalseForAltDataType()
        {
            var expected = 10;
            var actual = "Homer";

            var result = _matcher.IsMatch(expected, actual);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsMatchReturnsTrueForDifferentValueTypeInstance()
        {
            var expected = 10;
            var actual = 20;

            var result = _matcher.IsMatch(expected, actual);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsMatchReturnsTrueForSameStringValue()
        {
            var expected = "Homer";
            var actual = expected;

            var result = _matcher.IsMatch(expected, actual);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsMatchReturnsTrueForDifferentStringValue()
        {
            var expected = "Homer";
            var actual = "Marge";

            var result = _matcher.IsMatch(expected, actual);

            Assert.IsTrue(result);
        }
    }
}
