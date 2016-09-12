using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using HyperMock.Matchers;

namespace HyperMock.Core
{
    internal sealed class ParameterList
    {
        private static readonly ParameterMatcherFactory _matcherFactory = new ParameterMatcherFactory();

        internal bool IsMatchFor(Parameter[] parameters, params object[] args)
        {
            if (args == null && parameters.Length == 0) return true;
            if (args != null && args.Length == 0 && args.Length == parameters.Length) return true;

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

        internal Parameter[] BuildFrom(MethodCallExpression body, LambdaExpression expression)
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
                        Matcher = _matcherFactory.Create(lambda)
                    });
                }
            }

            return parameters.ToArray();
        }

        internal Parameter[] BuildFrom(MethodBase method, params object[] args)
        {
            var parameters = new List<Parameter>();

            if (args != null && args.Length > 0)
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
            if (paramInfo.ParameterType.IsByRef) return ParameterType.Ref;

            return ParameterType.In;
        }
    }
}