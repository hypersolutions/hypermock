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

            // No method call info - return the standard exact matcher
            if (methodCall == null) return new ExactParameterMatcher();

            // We have a param set, find the matcher to use...
            if (methodCall.Method.DeclaringType == typeof(Param))
            {
                var paramMatcherAttr = methodCall.Method.GetCustomAttributes(
                    typeof(ParameterMatcherAttribute), false).FirstOrDefault() as ParameterMatcherAttribute;

                // No attribute assigned (shouldn't happen) - return the standard exact matcher
                if (paramMatcherAttr == null) return new ExactParameterMatcher();

                // We have a matcher, return the instance...
                return CreateFromExpression(paramMatcherAttr.ParameterMatcherType, expression);
            }

            // Got here - return the standard exact matcher
            return new ExactParameterMatcher();
        }

        private ParameterMatcher CreateFromExpression(Type paramMatcherType, LambdaExpression expression)
        {
            var methodCall = (MethodCallExpression)expression.Body;

            if (methodCall.Arguments.Count == 0)
                return (ParameterMatcher)Activator.CreateInstance(paramMatcherType);

            var predicateDeletegate = Expression.Lambda(methodCall.Arguments[0]).Compile().DynamicInvoke();

            return new PredicateParameterMatcher(predicateDeletegate);
        }
    }
}