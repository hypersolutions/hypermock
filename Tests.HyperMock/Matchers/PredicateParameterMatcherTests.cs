using System;
using System.Linq.Expressions;
using HyperMock.Exceptions;
using HyperMock.Matchers;
using Xunit;

namespace Tests.HyperMock.Matchers
{
    public class PredicateParameterMatcherTests
    {
        private readonly PredicateParameterMatcher _matcher;

        public PredicateParameterMatcherTests()
        {
            _matcher = new PredicateParameterMatcher();
        }

        [Fact]
        public void IsMatchThrowsExceptionForInvalidCallContext()
        {
            Assert.Throws<MockException>(() => _matcher.IsMatch(null, null));
        }

        [Fact]
        public void IsMatchReturnsTrueForMatchingSingleConditionOnActualValue()
        {
            Expression<Func<bool>> expression = () => TestFunction(p => p < 10);
            _matcher.CallContext = expression.Body as MethodCallExpression;

            var isMatch = _matcher.IsMatch(null, 9);

            Assert.True(isMatch);
        }

        [Fact]
        public void IsMatchReturnsFalseForMatchingSingleConditionOnActualValue()
        {
            Expression<Func<bool>> expression = () => TestFunction(p => p < 10);
            _matcher.CallContext = expression.Body as MethodCallExpression;

            var isMatch = _matcher.IsMatch(null, 10);

            Assert.False(isMatch);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void IsMatchReturnsTrueForMatchingMultipleConditionsOnActualValue(int value)
        {
            Expression<Func<bool>> expression = () => TestFunction(p => p > 1 && p < 5);
            _matcher.CallContext = expression.Body as MethodCallExpression;

            var isMatch = _matcher.IsMatch(null, value);

            Assert.True(isMatch);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        public void IsMatchReturnsFalseForMatchingMultipleConditionsOnActualValue(int value)
        {
            Expression<Func<bool>> expression = () => TestFunction(p => p > 1 && p < 5);
            _matcher.CallContext = expression.Body as MethodCallExpression;

            var isMatch = _matcher.IsMatch(null, value);

            Assert.False(isMatch);
        }

        private bool TestFunction(Func<int, bool> func)
        {
            return func(10);
        }
    }
}
