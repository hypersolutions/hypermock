using System;
using System.Linq.Expressions;
using System.Reflection;
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
        public void BuildFromReturnsNoParametersForNullMethodExpression()
        {
            Expression<Action> expression = () => TestMethod();
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.AreEqual(0, parameters.Length);
        }

        [TestMethod]
        public void BuildFromReturnsNoParametersForEmptyMethodArgs()
        {
            Expression<Action> expression = () => TestMethod();

            var parameters = _parameterList.BuildFrom(null, expression);

            Assert.AreEqual(0, parameters.Length);
        }

        [TestMethod]
        public void BuildFromReturnsParametersForMethodArgs()
        {
            Expression<Action> expression = () => TestMethod2(10, 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.AreEqual(2, parameters.Length);
        }

        [TestMethod]
        public void BuildFromReturnsParameterValuesForMethodArgs()
        {
            Expression<Action> expression = () => TestMethod2(10, 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.AreEqual(10, parameters[0].Value);
            Assert.AreEqual(20, parameters[1].Value);
        }

        [TestMethod]
        public void BuildFromReturnsExactParameterMatchersForMethodArgs()
        {
            Expression<Action> expression = () => TestMethod2(10, 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.IsInstanceOfType(parameters[0].Matcher, typeof(ExactParameterMatcher));
            Assert.IsInstanceOfType(parameters[1].Matcher, typeof(ExactParameterMatcher));
        }

        [TestMethod]
        public void BuildFromReturnsAnyParameterMatchersForMethodArgs()
        {
            Expression<Action> expression = () => TestMethod2(Param.IsAny<int>(), 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.IsInstanceOfType(parameters[0].Matcher, typeof(AnyParameterMatcher));
            Assert.IsInstanceOfType(parameters[1].Matcher, typeof(ExactParameterMatcher));
        }

        [TestMethod]
        public void BuildFromReturnsRefParameterForMethodArgs()
        {
            string text = null;
            Expression<Action> expression = () => TestMethod3(ref text);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.AreEqual(ParameterType.Ref, parameters[0].Type);
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
            Expression<Func<int>> expression = () => TestFunction2(10, 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.AreEqual(2, parameters.Length);
        }

        [TestMethod]
        public void BuildFromReturnsParameterValuesForFunctionArgs()
        {
            Expression<Func<int>> expression = () => TestFunction2(10, 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.AreEqual(10, parameters[0].Value);
            Assert.AreEqual(20, parameters[1].Value);
        }

        [TestMethod]
        public void BuildFromReturnsExactParameterMatchersForFunctionArgs()
        {
            Expression<Func<int>> expression = () => TestFunction2(10, 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.IsInstanceOfType(parameters[0].Matcher, typeof(ExactParameterMatcher));
            Assert.IsInstanceOfType(parameters[1].Matcher, typeof(ExactParameterMatcher));
        }

        [TestMethod]
        public void BuildFromReturnsParameterTypeForFunctionInArgs()
        {
            Expression<Func<int>> expression = () => TestFunction2(10, 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.AreEqual(ParameterType.In, parameters[0].Type);
            Assert.AreEqual(ParameterType.In, parameters[1].Type);
        }

        [TestMethod]
        public void BuildFromReturnsParameterTypeForFunctionOutArgs()
        {
            int x;
            Expression<Func<bool>> expression = () => TryTestFunction("test", out x);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.AreEqual(ParameterType.In, parameters[0].Type);
            Assert.AreEqual(ParameterType.Out, parameters[1].Type);
        }

        [TestMethod]
        public void BuildFromReturnsAnyParameterMatchersForFunctionArgs()
        {
            Expression<Func<int>> expression = () => TestFunction2(Param.IsAny<int>(), 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.IsInstanceOfType(parameters[0].Matcher, typeof(AnyParameterMatcher));
            Assert.IsInstanceOfType(parameters[1].Matcher, typeof(ExactParameterMatcher));
        }

        [TestMethod]
        public void BuildFromReturnsPredicateParameterMatchersForMethodArgs()
        {
            Expression<Action> expression = () => TestMethod2(Param.Is<int>(p => p > 10 && p < 20), 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.IsInstanceOfType(parameters[0].Matcher, typeof(PredicateParameterMatcher));
            Assert.IsInstanceOfType(parameters[1].Matcher, typeof(ExactParameterMatcher));
        }

        [TestMethod]
        public void BuildFromOverloadReturnsEmptyParametersForMethodWithNoArgs()
        {
            var method = GetMethod("TestMethod");
            var parameters = _parameterList.BuildFrom(method);

            Assert.AreEqual(0, parameters.Length);
        }

        [TestMethod]
        public void BuildFromOverloadReturnsParametersForMethodArgs()
        {
            var method = GetMethod("TestMethod2");
            
            var parameters = _parameterList.BuildFrom(method, 10, 20);

            Assert.AreEqual(2, parameters.Length);
        }

        [TestMethod]
        public void BuildFromOverloadReturnsParameterValuesForMethodArgs()
        {
            var method = GetMethod("TestMethod2");

            var parameters = _parameterList.BuildFrom(method, 10, 20);

            Assert.AreEqual(10, parameters[0].Value);
            Assert.AreEqual(20, parameters[1].Value);
        }

        [TestMethod]
        public void BuildFromReturnsParameterInTypeForMethodArgs()
        {
            var method = GetMethod("TestMethod2");

            var parameters = _parameterList.BuildFrom(method, 10, 20);

            Assert.AreEqual(ParameterType.In, parameters[0].Type);
            Assert.AreEqual(ParameterType.In, parameters[1].Type);
        }
        
        [TestMethod]
        public void BuildFromReturnsParameterOutTypeForMethodArgs()
        {
            var method = GetMethod("TryTestFunction");

            var parameters = _parameterList.BuildFrom(method, "test", 0);

            Assert.AreEqual(ParameterType.In, parameters[0].Type);
            Assert.AreEqual(ParameterType.Out, parameters[1].Type);
        }

        [TestMethod]
        public void BuildFromReturnsParameterRefTypeForMethodArgs()
        {
            var method = GetMethod("TestMethod3");

            var parameters = _parameterList.BuildFrom(method, "test");

            Assert.AreEqual(ParameterType.Ref, parameters[0].Type);
        }

        private void TestMethod()
        {

        }

        private void TestMethod2(int x, int y)
        {
            System.Diagnostics.Debug.WriteLine($"{x}, {y}");
        }

        private void TestMethod3(ref string text)
        {
            
        }

        private int TestFunction()
        {
            return 0;
        }

        private int TestFunction2(int x, int y)
        {
            System.Diagnostics.Debug.WriteLine($"{x}, {y}");
            return 0;
        }

        private bool TryTestFunction(string text, out int value)
        {
            value = text != null ? 1 : 0;
            return false;
        }

        private MethodBase GetMethod(string name)
        {
            return GetType().GetMethod(name, BindingFlags.NonPublic|BindingFlags.Instance);
        }
    }
}
