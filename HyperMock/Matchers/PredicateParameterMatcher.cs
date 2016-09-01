using System;
using HyperMock.Exceptions;

namespace HyperMock.Matchers
{
    internal class PredicateParameterMatcher : ParameterMatcher
    {
        private readonly dynamic _predicate;

        internal PredicateParameterMatcher(dynamic predicate)
        {
            _predicate = predicate;
        }

        internal override bool IsMatch(object expected, object actual)
        {
            try
            {
                if (_predicate == null) throw new InvalidOperationException("The predicate is undefined.");

#if WINDOWS_UWP
                return (bool)_predicate((dynamic)actual);
#else
                return (bool)_predicate.DynamicInvoke(actual);
#endif
            }
            catch (Exception error)
            {
                throw new MockException("Unable to invoke the predicate inside the PredicateParameterMatcher.", error);
            }
        }
    }
}