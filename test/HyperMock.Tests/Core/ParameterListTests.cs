using System;
using System.Linq.Expressions;
using System.Reflection;
using HyperMock.Core;
using HyperMock.Matchers;
using Shouldly;
using Xunit;

namespace HyperMock.Tests.Core
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

            isMatch.ShouldBeFalse();
        }

        [Fact]
        public void IsMatchForReturnsTrueForExactMatchOnArg()
        {
            var parameters = new[]
            {
                new Parameter {Value = 10, Matcher = new ExactParameterMatcher()}
            };

            var isMatch = _parameterList.IsMatchFor(parameters, 10);

            isMatch.ShouldBeTrue();
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

            isMatch.ShouldBeFalse();
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

            isMatch.ShouldBeTrue();
        }

        [Fact]
        public void IsMatchForReturnsTrueForAnyMatchOnArg()
        {
            var parameters = new[]
            {
                new Parameter {Value = 10, Matcher = new AnyParameterMatcher()}
            };

            var isMatch = _parameterList.IsMatchFor(parameters, 100);

            isMatch.ShouldBeTrue();
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

            isMatch.ShouldBeTrue();
        }

        [Fact]
        public void BuildFromReturnsNoParametersForNullMethodExpression()
        {
            Expression<Action> expression = () => TestMethod();
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            parameters.Length.ShouldBe(0);
        }

        [Fact]
        public void BuildFromReturnsNoParametersForEmptyMethodArgs()
        {
            Expression<Action> expression = () => TestMethod();

            var parameters = _parameterList.BuildFrom(null, expression);

            parameters.Length.ShouldBe(0);
        }

        [Fact]
        public void BuildFromReturnsParametersForMethodArgs()
        {
            Expression<Action> expression = () => TestMethod2(10, 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            parameters.Length.ShouldBe(2);
        }

        [Fact]
        public void BuildFromReturnsParameterValuesForMethodArgs()
        {
            Expression<Action> expression = () => TestMethod2(10, 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            parameters[0].Value.ShouldBe(10);
            parameters[1].Value.ShouldBe(20);
        }

        [Fact]
        public void BuildFromReturnsExactParameterMatchersForMethodArgs()
        {
            Expression<Action> expression = () => TestMethod2(10, 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            parameters[0].Matcher.ShouldBeOfType<ExactParameterMatcher>();
            parameters[1].Matcher.ShouldBeOfType<ExactParameterMatcher>();
        }

        [Fact]
        public void BuildFromReturnsAnyParameterMatchersForMethodArgs()
        {
            Expression<Action> expression = () => TestMethod2(Param.IsAny<int>(), 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            parameters[0].Matcher.ShouldBeOfType<AnyParameterMatcher>();
            parameters[1].Matcher.ShouldBeOfType<ExactParameterMatcher>();
        }

        [Fact]
        public void BuildFromReturnsRefParameterForMethodArgs()
        {
            string text = null;
            Expression<Action> expression = () => TestMethod3(ref text);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            parameters[0].Type.ShouldBe(ParameterType.Ref);
        }

        [Fact]
        public void BuildFromReturnsNoParametersForEmptyFunctionArgs()
        {
            Expression<Func<int>> expression = () => TestFunction();
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            parameters.Length.ShouldBe(0);
        }

        [Fact]
        public void BuildFromReturnsParametersForFunctionArgs()
        {
            Expression<Func<int>> expression = () => TestFunction2(10, 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            parameters.Length.ShouldBe(2);
        }

        [Fact]
        public void BuildFromReturnsParameterValuesForFunctionArgs()
        {
            Expression<Func<int>> expression = () => TestFunction2(10, 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            parameters[0].Value.ShouldBe(10);
            parameters[1].Value.ShouldBe(20);
        }

        [Fact]
        public void BuildFromReturnsExactParameterMatchersForFunctionArgs()
        {
            Expression<Func<int>> expression = () => TestFunction2(10, 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            parameters[0].Matcher.ShouldBeOfType<ExactParameterMatcher>();
            parameters[1].Matcher.ShouldBeOfType<ExactParameterMatcher>();
        }

        [Fact]
        public void BuildFromReturnsParameterTypeForFunctionInArgs()
        {
            Expression<Func<int>> expression = () => TestFunction2(10, 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            parameters[0].Type.ShouldBe(ParameterType.In);
            parameters[1].Type.ShouldBe(ParameterType.In);
        }

        [Fact]
        public void BuildFromReturnsParameterTypeForFunctionOutArgs()
        {
            int x;
            Expression<Func<bool>> expression = () => TryTestFunction("test", out x);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            parameters[0].Type.ShouldBe(ParameterType.In);
            parameters[1].Type.ShouldBe(ParameterType.Out);
        }

        [Fact]
        public void BuildFromReturnsAnyParameterMatchersForFunctionArgs()
        {
            Expression<Func<int>> expression = () => TestFunction2(Param.IsAny<int>(), 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            parameters[0].Matcher.ShouldBeOfType<AnyParameterMatcher>();
            parameters[1].Matcher.ShouldBeOfType<ExactParameterMatcher>();
        }

        [Fact]
        public void BuildFromReturnsPredicateParameterMatchersForMethodArgs()
        {
            Expression<Action> expression = () => TestMethod2(Param.Is<int>(p => p > 10 && p < 20), 20);
            var body = expression.Body as MethodCallExpression;

            var parameters = _parameterList.BuildFrom(body, expression);

            parameters[0].Matcher.ShouldBeOfType<PredicateParameterMatcher>();
            parameters[1].Matcher.ShouldBeOfType<ExactParameterMatcher>();
        }

        [Fact]
        public void BuildFromOverloadReturnsEmptyParametersForMethodWithNoArgs()
        {
            var method = GetMethod("TestMethod");
            
            var parameters = _parameterList.BuildFrom(method);

            parameters.Length.ShouldBe(0);
        }

        [Fact]
        public void BuildFromOverloadReturnsParametersForMethodArgs()
        {
            var method = GetMethod("TestMethod2");
            
            var parameters = _parameterList.BuildFrom(method, 10, 20);

            parameters.Length.ShouldBe(2);
        }

        [Fact]
        public void BuildFromOverloadReturnsParameterValuesForMethodArgs()
        {
            var method = GetMethod("TestMethod2");

            var parameters = _parameterList.BuildFrom(method, 10, 20);

            parameters[0].Value.ShouldBe(10);
            parameters[1].Value.ShouldBe(20);
        }

        [Fact]
        public void BuildFromReturnsParameterInTypeForMethodArgs()
        {
            var method = GetMethod("TestMethod2");

            var parameters = _parameterList.BuildFrom(method, 10, 20);

            parameters[0].Type.ShouldBe(ParameterType.In);
            parameters[1].Type.ShouldBe(ParameterType.In);
        }

        [Fact]
        public void BuildFromReturnsParameterExactMatcherForMethodArgs()
        {
            var method = GetMethod("TestMethod2");

            var parameters = _parameterList.BuildFrom(method, 10, 20);

            parameters[0].Matcher.ShouldBeOfType<ExactParameterMatcher>();
            parameters[1].Matcher.ShouldBeOfType<ExactParameterMatcher>();
        }

        [Fact]
        public void BuildFromReturnsParameterOutTypeForMethodArgs()
        {
            var method = GetMethod("TryTestFunction");

            var parameters = _parameterList.BuildFrom(method, "test", 0);

            parameters[0].Type.ShouldBe(ParameterType.In);
            parameters[1].Type.ShouldBe(ParameterType.Out);
        }

        [Fact]
        public void BuildFromReturnsParameterRefTypeForMethodArgs()
        {
            var method = GetMethod("TestMethod3");

            var parameters = _parameterList.BuildFrom(method, "test");

            parameters[0].Type.ShouldBe(ParameterType.Ref);
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
