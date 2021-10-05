using System;
using System.Linq.Expressions;
using HyperMock.Matchers;
using Shouldly;
using Xunit;

namespace HyperMock.UnitTests.Matchers
{
    public class ParameterMatcherFactoryTests
    {
        [Fact]
        public void CreateMethodWithNoArgsReturnsExactMatcher()
        {
            Expression<Func<int>> expression = () => 10;

            var matcher = ParameterMatcherFactory.Create(expression);

            matcher.ShouldBeOfType<ExactParameterMatcher>();
        }

        [Fact]
        public void CreateMethodWithParamPredicateArgReturnsPredicateMatcher()
        {
            Expression<Func<int>> expression = () => Param.Is<int>(p => p < 10);

            var matcher = ParameterMatcherFactory.Create(expression);

            matcher.ShouldBeOfType<PredicateParameterMatcher>();
        }

        [Fact]
        public void CreateMethodWithParamAnyArgReturnsAnyMatcher()
        {
            Expression<Func<int>> expression = () => Param.IsAny<int>();

            var matcher = ParameterMatcherFactory.Create(expression);

            matcher.ShouldBeOfType<AnyParameterMatcher>();
        }

        [Fact]
        public void CreateMethodWithParamRegexArgReturnsRegexMatcher()
        {
            Expression<Func<string>> expression = () => Param.IsRegex("^[0-9]{8}$");

            var matcher = ParameterMatcherFactory.Create(expression);

            matcher.ShouldBeOfType<RegexParameterMatcher>();
        }
    }
}
