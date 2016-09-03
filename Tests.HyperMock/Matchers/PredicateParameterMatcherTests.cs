using System;
using System.Linq.Expressions;
#if WINDOWS_UWP
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif
using HyperMock.Exceptions;
using HyperMock.Matchers;

namespace Tests.HyperMock.Matchers
{
    [TestClass]
    public class PredicateParameterMatcherTests
    {
        private PredicateParameterMatcher _matcher;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _matcher = new PredicateParameterMatcher();
        }

#if WINDOWS_UWP
        [TestMethod]
        public void IsMatchThrowsExceptionForInvalidCallContext()
        {
            Assert.ThrowsException<MockException>(() => _matcher.IsMatch(null, null));
        }
#else
        [TestMethod, ExpectedException(typeof(MockException))]
        public void IsMatchThrowsExceptionForInvalidCallContext()
        {
            _matcher.IsMatch(null, null);
        }
#endif

        [TestMethod]
        public void IsMatchReturnsTrueForMatchingSingleConditionOnActualValue()
        {
            Expression<Func<bool>> expression = () => TestFunction(p => p < 10);
            _matcher.CallContext = expression.Body as MethodCallExpression;

            var isMatch = _matcher.IsMatch(null, 9);

            Assert.IsTrue(isMatch);
        }

        [TestMethod]
        public void IsMatchReturnsFalseForMatchingSingleConditionOnActualValue()
        {
            Expression<Func<bool>> expression = () => TestFunction(p => p < 10);
            _matcher.CallContext = expression.Body as MethodCallExpression;

            var isMatch = _matcher.IsMatch(null, 10);

            Assert.IsFalse(isMatch);
        }

        // Diff between windows and uwp MSTest. Windows one supports DataSource and UWP supports DataRows! 
        [TestMethod]
        public void IsMatchReturnsTrueForMatchingMultipleConditionsOnActualValue()
        {
            var data = new[] {2, 3, 4};
            Expression<Func<bool>> expression = () => TestFunction(p => p > 1 && p < 5);
            _matcher.CallContext = expression.Body as MethodCallExpression;

            foreach (var value in data)
            {
                var isMatch = _matcher.IsMatch(null, value);

                Assert.IsTrue(isMatch);
            }
        }

        // Diff between windows and uwp MSTest. Windows one supports DataSource and UWP supports DataRows! 
        [TestMethod]
        public void IsMatchReturnsFalseForMatchingMultipleConditionsOnActualValue()
        {
            var data = new[] { 1, 5 };
            Expression<Func<bool>> expression = () => TestFunction(p => p > 1 && p < 5);
            _matcher.CallContext = expression.Body as MethodCallExpression;

            foreach (var value in data)
            {
                var isMatch = _matcher.IsMatch(null, value);

                Assert.IsFalse(isMatch);
            }
        }

        private bool TestFunction(Func<int, bool> func)
        {
            return func(10);
        }
    }
}
