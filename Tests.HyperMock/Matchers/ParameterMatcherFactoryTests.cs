using System;
using System.Linq.Expressions;
using HyperMock;
using HyperMock.Matchers;
using Xunit;

namespace Tests.HyperMock.Matchers
{
    public class ParameterMatcherFactoryTests
    {
        private ParameterMatcherFactory _factory;

        public ParameterMatcherFactoryTests()
        {
            _factory = new ParameterMatcherFactory();
        }

        [Fact]
        public void CreateMethodWithNoArgsReturnsExactMatcher()
        {
            Expression<Func<int>> expression = () => 10;

            var matcher = _factory.Create(expression);

            Assert.IsType<ExactParameterMatcher>(matcher);
        }

        [Fact]
        public void CreateMethodWithParamPredicateArgReturnsPredicateMatcher()
        {
            Expression<Func<int>> expression = () => Param.Is<int>(p => p < 10);

            var matcher = _factory.Create(expression);

            Assert.IsType<PredicateParameterMatcher>(matcher);
        }

        [Fact]
        public void CreateMethodWithParamAnyArgReturnsAnyMatcher()
        {
            Expression<Func<int>> expression = () => Param.IsAny<int>();

            var matcher = _factory.Create(expression);

            Assert.IsType<AnyParameterMatcher>(matcher);
        }

        [Fact]
        public void CreateMethodWithParamRegexArgReturnsRegexMatcher()
        {
            Expression<Func<string>> expression = () => Param.IsRegex("^[0-9]{8}$");

            var matcher = _factory.Create(expression);

            Assert.IsType<RegexParameterMatcher>(matcher);
        }
    }
}
