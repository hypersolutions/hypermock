using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using HyperMock.Matchers;

namespace HyperMock.Core
{
    internal static class ParameterList
    {
        internal static bool IsMatchFor(Parameter[] parameters, params object[] args)
        {
            if (args == null && parameters.Length == 0) return true;
            if (args is { Length: 0 } && args.Length == parameters.Length) return true;

            if (args != null && args.Length == parameters.Length)
            {
                for (var i = 0; i < parameters.Length; i++)
                {
                    if (!parameters[i].Matcher.IsMatch(parameters[i].Value, args[i])) return false;
                }

                return true;
            }

            return false;
        }

        internal static Parameter[] BuildFrom(MethodCallExpression body, LambdaExpression expression)
        {
            var parameters = new List<Parameter>();

            if (body != null)
            {
                var methodParameters = body.Method.GetParameters();

                for (var i = 0; i < body.Arguments.Count; i++)
                {
                    var argument = body.Arguments[i];
                    var paramInfo = methodParameters[i];

                    var lambda = Expression.Lambda(argument, expression.Parameters);
                    var compiledDelegate = lambda.Compile();
                    var value = lambda.Parameters.Count == 0
                        ? compiledDelegate.DynamicInvoke()
                        : compiledDelegate.DynamicInvoke(new object[1]);
                    parameters.Add(new Parameter
                    {
                        Value = value,
                        Type = GetParameterType(paramInfo),
                        Matcher = ParameterMatcherFactory.Create(lambda)
                    });
                }
            }

            return parameters.ToArray();
        }

        internal static Parameter[] BuildFrom(MethodBase method, params object[] args)
        {
            var parameters = new List<Parameter>();

            if (args is { Length: > 0 })
            {
                var methodParameters = method.GetParameters();

                for (var i = 0; i < args.Length; i++)
                {
                    var value = args[i];
                    var paramInfo = methodParameters[i];

                    parameters.Add(new Parameter
                    {
                        Value = value,
                        Type = GetParameterType(paramInfo),
                        Matcher = new ExactParameterMatcher()
                    });
                }
            }

            return parameters.ToArray();
        }

        private static ParameterType GetParameterType(ParameterInfo paramInfo)
        {
            if (paramInfo.IsOut) return ParameterType.Out;
            return paramInfo.ParameterType.IsByRef ? ParameterType.Ref : ParameterType.In;
        }
    }
}