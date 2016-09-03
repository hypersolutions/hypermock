
using System;
using System.Linq.Expressions;
using HyperMock;
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
        private RegexParameterMatcher _matcher;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _matcher = new RegexParameterMatcher();
        }

        [TestMethod]
        public void IsMatchReturnsFalseForNullCallContext()
        {
            var isMatch = _matcher.IsMatch(null, null);

            Assert.IsFalse(isMatch);
        }

        [TestMethod]
        public void IsMatchReturnsFalseForNoCallContextArgs()
        {
            Expression<Func<string>> expression = () => TestFunction();
            _matcher.CallContext = expression.Body as MethodCallExpression;

            var isMatch = _matcher.IsMatch(null, null);

            Assert.IsFalse(isMatch);
        }

        [TestMethod]
        public void IsMatchReturnsFalseForNullPatternMatch()
        {
            Expression<Func<string>> expression = () => Param.IsRegex(null);
            _matcher.CallContext = expression.Body as MethodCallExpression;

            var isMatch = _matcher.IsMatch(null, null);

            Assert.IsFalse(isMatch);
        }

        [TestMethod]
        public void IsMatchReturnsTrueForEmptyPatternMatch()
        {
            Expression<Func<string>> expression = () => Param.IsRegex("");
            _matcher.CallContext = expression.Body as MethodCallExpression;

            var isMatch = _matcher.IsMatch(null, "");

            Assert.IsTrue(isMatch);
        }

        [TestMethod]
        public void IsMatchReturnsFalseForEmptyPatternAndNullValueMatch()
        {
            Expression<Func<string>> expression = () => Param.IsRegex("");
            _matcher.CallContext = expression.Body as MethodCallExpression;

            var isMatch = _matcher.IsMatch(null, null);

            Assert.IsFalse(isMatch);
        }

        [TestMethod]
        public void IsMatchReturnsTrueForPatternMatch()
        {
            Expression<Func<string>> expression = () => Param.IsRegex("^[0-9]{8}$");
            _matcher.CallContext = expression.Body as MethodCallExpression;

            var isMatch = _matcher.IsMatch(null, "12345678");

            Assert.IsTrue(isMatch);
        }

        [TestMethod]
        public void IsMatchReturnsFalseForPatternMismatch()
        {
            var data = new[] { "1234567", "123456789", "ABCDEFGH" };
            Expression<Func<string>> expression = () => Param.IsRegex("^[0-9]{8}$");
            _matcher.CallContext = expression.Body as MethodCallExpression;

            foreach (var value in data)
            {
                var isMatch = _matcher.IsMatch(null, value);

                Assert.IsFalse(isMatch);
            }
        }

        private string TestFunction()
        {
            return null;
        }
    }
}
