using System;
using System.Linq;
using System.Linq.Expressions;
#if WINDOWS_UWP
using System.Reflection;
#endif

namespace HyperMock.Matchers
{
    internal class ParameterMatcherFactory
    {
        internal ParameterMatcher Create(LambdaExpression expression)
        {
            var methodCall = expression.Body as MethodCallExpression;

            // Find the parameter matcher attribute (if exists)
            var paramMatcherAttr = methodCall?.Method.GetCustomAttributes(
                    typeof(ParameterMatcherAttribute), false).FirstOrDefault() as ParameterMatcherAttribute;

            // If exists then create the matcher else return the exact matcher
            return paramMatcherAttr != null 
                ? CreateFromExpression(paramMatcherAttr.ParameterMatcherType, expression) 
                : new ExactParameterMatcher();
        }

        private ParameterMatcher CreateFromExpression(Type paramMatcherType, LambdaExpression expression)
        {
            var methodCall = (MethodCallExpression)expression.Body;

            if (methodCall.Arguments.Count == 0)
                return (ParameterMatcher)Activator.CreateInstance(paramMatcherType);

            var delegateExpr = Expression.Lambda(methodCall.Arguments[0]).Compile().DynamicInvoke();

            return delegateExpr is string ?
                (ParameterMatcher)new RegexParameterMatcher(delegateExpr.ToString()) 
                : new PredicateParameterMatcher(delegateExpr);
        }
    }
}