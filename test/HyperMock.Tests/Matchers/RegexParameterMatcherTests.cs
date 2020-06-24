using System;
using System.Linq.Expressions;
using HyperMock.Matchers;
using Shouldly;
using Xunit;

namespace HyperMock.Tests.Matchers
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

            isMatch.ShouldBeFalse();
        }

        [Fact]
        public void IsMatchReturnsFalseForNoCallContextArgs()
        {
            Expression<Func<string>> expression = () => TestFunction();
            _matcher.CallContext = expression.Body as MethodCallExpression;

            var isMatch = _matcher.IsMatch(null, null);

            isMatch.ShouldBeFalse();
        }

        [Fact]
        public void IsMatchReturnsFalseForNullPatternMatch()
        {
            Expression<Func<string>> expression = () => Param.IsRegex(null);
            _matcher.CallContext = expression.Body as MethodCallExpression;

            var isMatch = _matcher.IsMatch(null, null);

            isMatch.ShouldBeFalse();
        }

        [Fact]
        public void IsMatchReturnsTrueForEmptyPatternMatch()
        {
            Expression<Func<string>> expression = () => Param.IsRegex("");
            _matcher.CallContext = expression.Body as MethodCallExpression;

            var isMatch = _matcher.IsMatch(null, "");

            isMatch.ShouldBeTrue();
        }

        [Fact]
        public void IsMatchReturnsFalseForEmptyPatternAndNullValueMatch()
        {
            Expression<Func<string>> expression = () => Param.IsRegex("");
            _matcher.CallContext = expression.Body as MethodCallExpression;

            var isMatch = _matcher.IsMatch(null, null);

            isMatch.ShouldBeFalse();
        }

        [Fact]
        public void IsMatchReturnsTrueForPatternMatch()
        {
            Expression<Func<string>> expression = () => Param.IsRegex("^[0-9]{8}$");
            _matcher.CallContext = expression.Body as MethodCallExpression;

            var isMatch = _matcher.IsMatch(null, "12345678");

            isMatch.ShouldBeTrue();
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

            isMatch.ShouldBeFalse();
        }

        private string TestFunction()
        {
            return null;
        }
    }
}
