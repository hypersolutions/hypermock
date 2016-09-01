#if WINDOWS_UWP
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif
using HyperMock.Matchers;

namespace Tests.HyperMock.Matchers
{
    [TestClass]
    public class RegexParameterMatcherTests
    {
        [TestMethod]
        public void IsMatchReturnsFalseForNullPatternMatch()
        {
            var matcher = new RegexParameterMatcher(null);

            var isMatch = matcher.IsMatch(null, null);

            Assert.IsFalse(isMatch);
        }

        [TestMethod]
        public void IsMatchReturnsTrueForEmptyPatternMatch()
        {
            var matcher = new RegexParameterMatcher("");

            var isMatch = matcher.IsMatch(null, "");

            Assert.IsTrue(isMatch);
        }

        [TestMethod]
        public void IsMatchReturnsFalseForEmptyPatternAndNullValueMatch()
        {
            var matcher = new RegexParameterMatcher("");

            var isMatch = matcher.IsMatch(null, null);

            Assert.IsFalse(isMatch);
        }

        [TestMethod]
        public void IsMatchReturnsTrueForPatternMatch()
        {
            var matcher = new RegexParameterMatcher("^[0-9]{8}$");

            var isMatch = matcher.IsMatch(null, "12345678");

            Assert.IsTrue(isMatch);
        }

        [TestMethod]
        public void IsMatchReturnsFalseForPatternMismatch()
        {
            var data = new[] {"1234567", "123456789", "ABCDEFGH"};

            foreach (var value in data)
            {
                var matcher = new RegexParameterMatcher("^[0-9]{8}$");

                var isMatch = matcher.IsMatch(null, value);

                Assert.IsFalse(isMatch);
            }
        }
    }
}
