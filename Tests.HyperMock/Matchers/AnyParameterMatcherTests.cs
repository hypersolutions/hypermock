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
        [TestMethod]
        public void IsMatchReturnsTrueForNull()
        {
            var matcher = new AnyParameterMatcher();

            var result = matcher.IsMatch(null, null);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsMatchReturnsTrueForSameReferenceTypeInstance()
        {
            var matcher = new AnyParameterMatcher();
            var expected = new object();
            var actual = expected;

            var result = matcher.IsMatch(expected, actual);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsMatchReturnsTrueForDifferentReferenceTypeInstance()
        {
            var matcher = new AnyParameterMatcher();
            var expected = new object();
            var actual = new object();

            var result = matcher.IsMatch(expected, actual);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsMatchReturnsTrueForSameValueTypeInstance()
        {
            var matcher = new AnyParameterMatcher();
            var expected = 10;
            var actual = expected;

            var result = matcher.IsMatch(expected, actual);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsMatchReturnsFalseForAltDataType()
        {
            var matcher = new AnyParameterMatcher();
            var expected = 10;
            var actual = "Homer";

            var result = matcher.IsMatch(expected, actual);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsMatchReturnsTrueForDifferentValueTypeInstance()
        {
            var matcher = new AnyParameterMatcher();
            var expected = 10;
            var actual = 20;

            var result = matcher.IsMatch(expected, actual);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsMatchReturnsTrueForSameStringValue()
        {
            var matcher = new AnyParameterMatcher();
            var expected = "Homer";
            var actual = expected;

            var result = matcher.IsMatch(expected, actual);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsMatchReturnsTrueForDifferentStringValue()
        {
            var matcher = new AnyParameterMatcher();
            var expected = "Homer";
            var actual = "Marge";

            var result = matcher.IsMatch(expected, actual);

            Assert.IsTrue(result);
        }
    }
}
