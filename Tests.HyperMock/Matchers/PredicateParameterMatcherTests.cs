using System;
#if WINDOWS_UWP
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif
using HyperMock.Matchers;

namespace Tests.HyperMock.Matchers
{
    [TestClass]
    public class PredicateParameterMatcherTests
    {
        [TestMethod]
        public void IsMatchReturnsTrueForMatchingSingleConditionOnActualValue()
        {
            Func<int, bool> func = p => p < 10;
            var matcher = new PredicateParameterMatcher(func);

            var isMatch = matcher.IsMatch(null, 9);

            Assert.IsTrue(isMatch);
        }

        [TestMethod]
        public void IsMatchReturnsFalseForMatchingSingleConditionOnActualValue()
        {
            Func<int, bool> func = p => p < 10;
            var matcher = new PredicateParameterMatcher(func);

            var isMatch = matcher.IsMatch(null, 10);

            Assert.IsFalse(isMatch);
        }

        // Diff between windows and uwp MSTest. Windows one supports DataSource and UWP supports DataRows! 
        [TestMethod]
        public void IsMatchReturnsTrueForMatchingMultipleConditionsOnActualValue()
        {
            var data = new[] {2, 3, 4};

            foreach (var value in data)
            {
                Func<int, bool> func = p => p > 1 && p < 5;
                var matcher = new PredicateParameterMatcher(func);

                var isMatch = matcher.IsMatch(null, value);

                Assert.IsTrue(isMatch);
            }
        }

        // Diff between windows and uwp MSTest. Windows one supports DataSource and UWP supports DataRows! 
        [TestMethod]
        public void IsMatchReturnsFalseForMatchingMultipleConditionsOnActualValue()
        {
            var data = new[] { 1, 5 };

            foreach (var value in data)
            {
                Func<int, bool> func = p => p > 1 && p < 5;
                var matcher = new PredicateParameterMatcher(func);

                var isMatch = matcher.IsMatch(null, value);

                Assert.IsFalse(isMatch);
            }
        }
    }
}
