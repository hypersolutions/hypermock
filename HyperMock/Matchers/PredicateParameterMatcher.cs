using System;
using System.Linq.Expressions;
using HyperMock.Exceptions;

namespace HyperMock.Matchers
{
    internal class PredicateParameterMatcher : ParameterMatcher
    {
        internal override bool IsMatch(object expected, object actual)
        {
            try
            {
                var predicate = GetPredicate();

                if (predicate == null) throw new InvalidOperationException("The predicate is undefined.");

#if WINDOWS_UWP
                return (bool)predicate((dynamic)actual);
#else
                return (bool)predicate.DynamicInvoke(actual);
#endif
            }
            catch (Exception error)
            {
                throw new MockException("Unable to invoke the predicate inside the PredicateParameterMatcher.", error);
            }
        }

        private dynamic GetPredicate()
        {
            if (CallContext?.Arguments == null || CallContext.Arguments.Count != 1) return null;

            var predicate = CallContext.Arguments[0];
            var compiledPredicate = Expression.Lambda(predicate).Compile();
            var value = compiledPredicate.DynamicInvoke();
            return value;
        }
    }
}