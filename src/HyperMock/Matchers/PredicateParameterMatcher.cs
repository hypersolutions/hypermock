using System;
using System.Linq.Expressions;
using HyperMock.Core;
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

                var predicateHelper = new PredicateHelper();
                return predicateHelper.Invoke(predicate, actual);
            }
            catch (Exception error)
            {
                throw new MockException("Unable to invoke the predicate inside the PredicateParameterMatcher.", error);
            }
        }

        private object GetPredicate()
        {
            if (CallContext?.Arguments == null || CallContext.Arguments.Count != 1) return null;

            var predicate = CallContext.Arguments[0];
            var compiledPredicate = Expression.Lambda(predicate).Compile();
            var value = compiledPredicate.DynamicInvoke();
            return value;
        }
    }
}
