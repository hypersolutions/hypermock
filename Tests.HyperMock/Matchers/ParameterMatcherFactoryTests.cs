using System;
using System.Linq.Expressions;
using HyperMock;
#if WINDOWS_UWP
using HyperMock.Matchers;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using HyperMock.Matchers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace Tests.HyperMock.Matchers
{
    [TestClass]
    public class ParameterMatcherFactoryTests
    {
        private ParameterMatcherFactory _factory;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _factory = new ParameterMatcherFactory();
        }

        [TestMethod]
        public void CreateMethodWithNoArgsReturnsExactMatcher()
        {
            Expression<Func<int>> expression = () => 10;

            var matcher = _factory.Create(expression);

            Assert.IsInstanceOfType(matcher, typeof(ExactParameterMatcher));
        }

        [TestMethod]
        public void CreateMethodWithParamPredicateArgReturnsPredicateMatcher()
        {
            Expression<Func<int>> expression = () => Param.Is<int>(p => p < 10);

            var matcher = _factory.Create(expression);

            Assert.IsInstanceOfType(matcher, typeof(PredicateParameterMatcher));
        }

        [TestMethod]
        public void CreateMethodWithParamAnyArgReturnsAnyMatcher()
        {
            Expression<Func<int>> expression = () => Param.IsAny<int>();

            var matcher = _factory.Create(expression);

            Assert.IsInstanceOfType(matcher, typeof(AnyParameterMatcher));
        }
    }
}
