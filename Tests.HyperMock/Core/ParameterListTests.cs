using System;
using System.Linq.Expressions;
using HyperMock;
using HyperMock.Core;
using HyperMock.Matchers;
#if WINDOWS_UWP
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace Tests.HyperMock.Core
{
    [TestClass]
    public class ParameterListTests
    {
        private ParameterList _parameterList;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _parameterList = new ParameterList();
        }

        [TestMethod]
        public void IsMatchForReturnsFalseForExactMatchOnArg()
        {
            var parameters = new[]
            {
                new Parameter {Value = 10, Matcher = new ExactParameterMatcher()}
            };

            var isMatch = _parameterList.IsMatchFor(parameters, 100);

            Assert.IsFalse(isMatch);
        }

        [TestMethod]
        public void IsMatchForReturnsTrueForExactMatchOnArg()
        {
            var parameters = new[]
            {
                new Parameter {Value = 10, Matcher = new ExactParameterMatcher()}
            };

            var isMatch = _parameterList.IsMatchFor(parameters, 10);

            Assert.IsTrue(isMatch);
        }

        [TestMethod]
        public void IsMatchForReturnsFalseForExactMatchOnMultipleArgs()
        {
            var parameters = new[]
            {
                new Parameter {Value = 10, Matcher = new ExactParameterMatcher()},
                new Parameter {Value = 20, Matcher = new ExactParameterMatcher()}
            };

            var isMatch = _parameterList.IsMatchFor(parameters, 100, 200);

            Assert.IsFalse(isMatch);
        }

        [TestMethod]
        public void IsMatchForReturnsTrueForExactMatchOnMultipleArgs()
        {
            var parameters = new[]
            {
                new Parameter {Value = 10, Matcher = new ExactParameterMatcher()},
                new Parameter {Value = 20, Matcher = new ExactParameterMatcher()}
            };

            var isMatch = _parameterList.IsMatchFor(parameters, 10, 20);

            Assert.IsTrue(isMatch);
        }

        [TestMethod]
        public void IsMatchForReturnsTrueForAnyMatchOnArg()
        {
            var parameters = new[]
            {
                new Parameter {Value = 10, Matcher = new AnyParameterMatcher()}
            };

            var isMatch = _parameterList.IsMatchFor(parameters, 100);

            Assert.IsTrue(isMatch);
        }

        [TestMethod]
        public void IsMatchForReturnsTrueForAnyMatchOnMultipleArgs()
        {
            var parameters = new[]
            {
                new Parameter {Value = 10, Matcher = new AnyParameterMatcher()},
                new Parameter {Value = 20, Matcher = new AnyParameterMatcher()}
            };

            var isMatch = _parameterList.IsMatchFor(parameters, 100, 200);

            Assert.IsTrue(isMatch);
        }

        [TestMethod]
        public void BuildFromReturnsNoParametersForEmptyMethodArgs()
        {
            Expression<Action> expression = () => TestMethod();
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.AreEqual(0, parameters.Length);
        }

        [TestMethod]
        public void BuildFromReturnsParametersForMethodArgs()
        {
            Expression<Action> expression = () => TestMethod(10, 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.AreEqual(2, parameters.Length);
        }

        [TestMethod]
        public void BuildFromReturnsParameterValuesForMethodArgs()
        {
            Expression<Action> expression = () => TestMethod(10, 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.AreEqual(10, parameters[0].Value);
            Assert.AreEqual(20, parameters[1].Value);
        }

        [TestMethod]
        public void BuildFromReturnsExactParameterMatchersForMethodArgs()
        {
            Expression<Action> expression = () => TestMethod(10, 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.IsInstanceOfType(parameters[0].Matcher, typeof(ExactParameterMatcher));
            Assert.IsInstanceOfType(parameters[1].Matcher, typeof(ExactParameterMatcher));
        }

        [TestMethod]
        public void BuildFromReturnsAnyParameterMatchersForMethodArgs()
        {
            Expression<Action> expression = () => TestMethod(Param.IsAny<int>(), 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.IsInstanceOfType(parameters[0].Matcher, typeof(AnyParameterMatcher));
            Assert.IsInstanceOfType(parameters[1].Matcher, typeof(ExactParameterMatcher));
        }

        [TestMethod]
        public void BuildFromReturnsNoParametersForEmptyFunctionArgs()
        {
            Expression<Func<int>> expression = () => TestFunction();
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.AreEqual(0, parameters.Length);
        }

        [TestMethod]
        public void BuildFromReturnsParametersForFunctionArgs()
        {
            Expression<Func<int>> expression = () => TestFunction(10, 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.AreEqual(2, parameters.Length);
        }

        [TestMethod]
        public void BuildFromReturnsParameterValuesForFunctionArgs()
        {
            Expression<Func<int>> expression = () => TestFunction(10, 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.AreEqual(10, parameters[0].Value);
            Assert.AreEqual(20, parameters[1].Value);
        }

        [TestMethod]
        public void BuildFromReturnsExactParameterMatchersForFunctionArgs()
        {
            Expression<Func<int>> expression = () => TestFunction(10, 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.IsInstanceOfType(parameters[0].Matcher, typeof(ExactParameterMatcher));
            Assert.IsInstanceOfType(parameters[1].Matcher, typeof(ExactParameterMatcher));
        }

        [TestMethod]
        public void BuildFromReturnsAnyParameterMatchersForFunctionArgs()
        {
            Expression<Func<int>> expression = () => TestFunction(Param.IsAny<int>(), 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.IsInstanceOfType(parameters[0].Matcher, typeof(AnyParameterMatcher));
            Assert.IsInstanceOfType(parameters[1].Matcher, typeof(ExactParameterMatcher));
        }

        [TestMethod]
        public void BuildFromReturnsPredicateParameterMatchersForMethodArgs()
        {
            Expression<Action> expression = () => TestMethod(Param.Is<int>(p => p > 10 && p < 20), 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.IsInstanceOfType(parameters[0].Matcher, typeof(PredicateParameterMatcher));
            Assert.IsInstanceOfType(parameters[1].Matcher, typeof(ExactParameterMatcher));
        }

        private void TestMethod()
        {

        }

        private void TestMethod(int x, int y)
        {
            System.Diagnostics.Debug.WriteLine($"{x}, {y}");
        }

        private int TestFunction()
        {
            return 0;
        }

        private int TestFunction(int x, int y)
        {
            System.Diagnostics.Debug.WriteLine($"{x}, {y}");
            return 0;
        }
    }
}
