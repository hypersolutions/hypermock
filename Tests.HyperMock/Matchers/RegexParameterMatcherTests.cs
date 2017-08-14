using System;
using System.Linq.Expressions;
using HyperMock;
using HyperMock.Matchers;
using Xunit;

namespace Tests.HyperMock.Matchers
{
    public class RegexParameterMatcherTests
    {
        private readonly RegexParameterMatcher _matcher;

        public RegexParameterMatcherTests()
        {
            _matcher = new RegexParameterMatcher();
        }

        [Fact]
        public void IsMatchReturnsFalseForNullCallContext()
        {
            var isMatch = _matcher.IsMatch(null, null);

            Assert.False(isMatch);
        }

        [Fact]
        public void IsMatchReturnsFalseForNoCallContextArgs()
        {
            Expression<Func<string>> expression = () => TestFunction();
            _matcher.CallContext = expression.Body as MethodCallExpression;

            var isMatch = _matcher.IsMatch(null, null);

            Assert.False(isMatch);
        }

        [Fact]
        public void IsMatchReturnsFalseForNullPatternMatch()
        {
            Expression<Func<string>> expression = () => Param.IsRegex(null);
            _matcher.CallContext = expression.Body as MethodCallExpression;

            var isMatch = _matcher.IsMatch(null, null);

            Assert.False(isMatch);
        }

        [Fact]
        public void IsMatchReturnsTrueForEmptyPatternMatch()
        {
            Expression<Func<string>> expression = () => Param.IsRegex("");
            _matcher.CallContext = expression.Body as MethodCallExpression;

            var isMatch = _matcher.IsMatch(null, "");

            Assert.True(isMatch);
        }

        [Fact]
        public void IsMatchReturnsFalseForEmptyPatternAndNullValueMatch()
        {
            Expression<Func<string>> expression = () => Param.IsRegex("");
            _matcher.CallContext = expression.Body as MethodCallExpression;

            var isMatch = _matcher.IsMatch(null, null);

            Assert.False(isMatch);
        }

        [Fact]
        public void IsMatchReturnsTrueForPatternMatch()
        {
            Expression<Func<string>> expression = () => Param.IsRegex("^[0-9]{8}$");
            _matcher.CallContext = expression.Body as MethodCallExpression;

            var isMatch = _matcher.IsMatch(null, "12345678");

            Assert.True(isMatch);
        }

        [Theory]
        [InlineData("1234567")]
        [InlineData("123456789")]
        [InlineData("ABCDEFGH")]
        public void IsMatchReturnsFalseForPatternMismatch(string value)
        {
            Expression<Func<string>> expression = () => Param.IsRegex("^[0-9]{8}$");
            _matcher.CallContext = expression.Body as MethodCallExpression;

            var isMatch = _matcher.IsMatch(null, value);

            Assert.False(isMatch);
        }

        private string TestFunction()
        {
            return null;
        }
    }
}
