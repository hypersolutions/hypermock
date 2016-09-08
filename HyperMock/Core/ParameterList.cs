using System.Collections.Generic;
using System.Linq.Expressions;
#if WINDOWS_UWP
using System.Reflection;
#endif
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
                foreach (var argument in body.Arguments)
                {
                    var lambda = Expression.Lambda(argument, expression.Parameters);
                    var compiledDelegate = lambda.Compile();
                    var value = lambda.Parameters.Count == 0
                        ? compiledDelegate.DynamicInvoke()
                        : compiledDelegate.DynamicInvoke(new object[1]);
                    parameters.Add(new Parameter {Value = value, Matcher = _matcherFactory.Create(lambda)});
                }
            }

            return parameters.ToArray();
        }
    }
}