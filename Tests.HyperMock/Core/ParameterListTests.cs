using System;
using System.Linq.Expressions;
using System.Reflection;
using HyperMock;
using HyperMock.Core;
using HyperMock.Matchers;
using Xunit;

namespace Tests.HyperMock.Core
{
    public class ParameterListTests
    {
        private readonly ParameterList _parameterList;

        public ParameterListTests()
        {
            _parameterList = new ParameterList();
        }

        [Fact]
        public void IsMatchForReturnsFalseForExactMatchOnArg()
        {
            var parameters = new[]
            {
                new Parameter {Value = 10, Matcher = new ExactParameterMatcher()}
            };

            var isMatch = _parameterList.IsMatchFor(parameters, 100);

            Assert.False(isMatch);
        }

        [Fact]
        public void IsMatchForReturnsTrueForExactMatchOnArg()
        {
            var parameters = new[]
            {
                new Parameter {Value = 10, Matcher = new ExactParameterMatcher()}
            };

            var isMatch = _parameterList.IsMatchFor(parameters, 10);

            Assert.True(isMatch);
        }

        [Fact]
        public void IsMatchForReturnsFalseForExactMatchOnMultipleArgs()
        {
            var parameters = new[]
            {
                new Parameter {Value = 10, Matcher = new ExactParameterMatcher()},
                new Parameter {Value = 20, Matcher = new ExactParameterMatcher()}
            };

            var isMatch = _parameterList.IsMatchFor(parameters, 100, 200);

            Assert.False(isMatch);
        }

        [Fact]
        public void IsMatchForReturnsTrueForExactMatchOnMultipleArgs()
        {
            var parameters = new[]
            {
                new Parameter {Value = 10, Matcher = new ExactParameterMatcher()},
                new Parameter {Value = 20, Matcher = new ExactParameterMatcher()}
            };

            var isMatch = _parameterList.IsMatchFor(parameters, 10, 20);

            Assert.True(isMatch);
        }

        [Fact]
        public void IsMatchForReturnsTrueForAnyMatchOnArg()
        {
            var parameters = new[]
            {
                new Parameter {Value = 10, Matcher = new AnyParameterMatcher()}
            };

            var isMatch = _parameterList.IsMatchFor(parameters, 100);

            Assert.True(isMatch);
        }

        [Fact]
        public void IsMatchForReturnsTrueForAnyMatchOnMultipleArgs()
        {
            var parameters = new[]
            {
                new Parameter {Value = 10, Matcher = new AnyParameterMatcher()},
                new Parameter {Value = 20, Matcher = new AnyParameterMatcher()}
            };

            var isMatch = _parameterList.IsMatchFor(parameters, 100, 200);

            Assert.True(isMatch);
        }

        [Fact]
        public void BuildFromReturnsNoParametersForNullMethodExpression()
        {
            Expression<Action> expression = () => TestMethod();
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.Equal(0, parameters.Length);
        }

        [Fact]
        public void BuildFromReturnsNoParametersForEmptyMethodArgs()
        {
            Expression<Action> expression = () => TestMethod();

            var parameters = _parameterList.BuildFrom(null, expression);

            Assert.Equal(0, parameters.Length);
        }

        [Fact]
        public void BuildFromReturnsParametersForMethodArgs()
        {
            Expression<Action> expression = () => TestMethod2(10, 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.Equal(2, parameters.Length);
        }

        [Fact]
        public void BuildFromReturnsParameterValuesForMethodArgs()
        {
            Expression<Action> expression = () => TestMethod2(10, 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.Equal(10, parameters[0].Value);
            Assert.Equal(20, parameters[1].Value);
        }

        [Fact]
        public void BuildFromReturnsExactParameterMatchersForMethodArgs()
        {
            Expression<Action> expression = () => TestMethod2(10, 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.IsType<ExactParameterMatcher>(parameters[0].Matcher);
            Assert.IsType<ExactParameterMatcher>(parameters[1].Matcher);
        }

        [Fact]
        public void BuildFromReturnsAnyParameterMatchersForMethodArgs()
        {
            Expression<Action> expression = () => TestMethod2(Param.IsAny<int>(), 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.IsType<AnyParameterMatcher>(parameters[0].Matcher);
            Assert.IsType<ExactParameterMatcher>(parameters[1].Matcher);
        }

        [Fact]
        public void BuildFromReturnsRefParameterForMethodArgs()
        {
            string text = null;
            Expression<Action> expression = () => TestMethod3(ref text);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.Equal(ParameterType.Ref, parameters[0].Type);
        }

        [Fact]
        public void BuildFromReturnsNoParametersForEmptyFunctionArgs()
        {
            Expression<Func<int>> expression = () => TestFunction();
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.Equal(0, parameters.Length);
        }

        [Fact]
        public void BuildFromReturnsParametersForFunctionArgs()
        {
            Expression<Func<int>> expression = () => TestFunction2(10, 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.Equal(2, parameters.Length);
        }

        [Fact]
        public void BuildFromReturnsParameterValuesForFunctionArgs()
        {
            Expression<Func<int>> expression = () => TestFunction2(10, 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.Equal(10, parameters[0].Value);
            Assert.Equal(20, parameters[1].Value);
        }

        [Fact]
        public void BuildFromReturnsExactParameterMatchersForFunctionArgs()
        {
            Expression<Func<int>> expression = () => TestFunction2(10, 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.IsType<ExactParameterMatcher>(parameters[0].Matcher);
            Assert.IsType<ExactParameterMatcher>(parameters[1].Matcher);
        }

        [Fact]
        public void BuildFromReturnsParameterTypeForFunctionInArgs()
        {
            Expression<Func<int>> expression = () => TestFunction2(10, 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.Equal(ParameterType.In, parameters[0].Type);
            Assert.Equal(ParameterType.In, parameters[1].Type);
        }

        [Fact]
        public void BuildFromReturnsParameterTypeForFunctionOutArgs()
        {
            int x;
            Expression<Func<bool>> expression = () => TryTestFunction("test", out x);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.Equal(ParameterType.In, parameters[0].Type);
            Assert.Equal(ParameterType.Out, parameters[1].Type);
        }

        [Fact]
        public void BuildFromReturnsAnyParameterMatchersForFunctionArgs()
        {
            Expression<Func<int>> expression = () => TestFunction2(Param.IsAny<int>(), 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.IsType<AnyParameterMatcher>(parameters[0].Matcher);
            Assert.IsType<ExactParameterMatcher>(parameters[1].Matcher);
        }

        [Fact]
        public void BuildFromReturnsPredicateParameterMatchersForMethodArgs()
        {
            Expression<Action> expression = () => TestMethod2(Param.Is<int>(p => p > 10 && p < 20), 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            Assert.IsType<PredicateParameterMatcher>(parameters[0].Matcher);
            Assert.IsType<ExactParameterMatcher>(parameters[1].Matcher);
        }

        [Fact]
        public void BuildFromOverloadReturnsEmptyParametersForMethodWithNoArgs()
        {
            var method = GetMethod("TestMethod");
            var parameters = _parameterList.BuildFrom(method);

            Assert.Equal(0, parameters.Length);
        }

        [Fact]
        public void BuildFromOverloadReturnsParametersForMethodArgs()
        {
            var method = GetMethod("TestMethod2");
            
            var parameters = _parameterList.BuildFrom(method, 10, 20);

            Assert.Equal(2, parameters.Length);
        }

        [Fact]
        public void BuildFromOverloadReturnsParameterValuesForMethodArgs()
        {
            var method = GetMethod("TestMethod2");

            var parameters = _parameterList.BuildFrom(method, 10, 20);

            Assert.Equal(10, parameters[0].Value);
            Assert.Equal(20, parameters[1].Value);
        }

        [Fact]
        public void BuildFromReturnsParameterInTypeForMethodArgs()
        {
            var method = GetMethod("TestMethod2");

            var parameters = _parameterList.BuildFrom(method, 10, 20);

            Assert.Equal(ParameterType.In, parameters[0].Type);
            Assert.Equal(ParameterType.In, parameters[1].Type);
        }

        [Fact]
        public void BuildFromReturnsParameterExactMatcherForMethodArgs()
        {
            var method = GetMethod("TestMethod2");

            var parameters = _parameterList.BuildFrom(method, 10, 20);

            Assert.IsType<ExactParameterMatcher>(parameters[0].Matcher);
            Assert.IsType<ExactParameterMatcher>(parameters[1].Matcher);
        }

        [Fact]
        public void BuildFromReturnsParameterOutTypeForMethodArgs()
        {
            var method = GetMethod("TryTestFunction");

            var parameters = _parameterList.BuildFrom(method, "test", 0);

            Assert.Equal(ParameterType.In, parameters[0].Type);
            Assert.Equal(ParameterType.Out, parameters[1].Type);
        }

        [Fact]
        public void BuildFromReturnsParameterRefTypeForMethodArgs()
        {
            var method = GetMethod("TestMethod3");

            var parameters = _parameterList.BuildFrom(method, "test");

            Assert.Equal(ParameterType.Ref, parameters[0].Type);
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
            System.Diagnostics.Debug.WriteLine(text);
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
