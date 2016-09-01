using System;
using HyperMock.Matchers;

namespace HyperMock
{
    /// <summary>
    /// Mock helper for describing a parameter.
    /// </summary>
    public class Param
    {
        /// <summary>
        /// Indicates that any value of the type is allowed and will resolve.
        /// </summary>
        /// <typeparam name="T">Parameter type</typeparam>
        /// <returns></returns>
        [ParameterMatcher(typeof(AnyParameterMatcher))]
        public static T IsAny<T>()
        {
            return default(T);
        }

        /// <summary>
        /// Provides a predicate approach to determining a match for a parameter.
        /// </summary>
        /// <typeparam name="T">Parameter type</typeparam>
        /// <param name="predicate">Predicate</param>
        /// <returns></returns>
        [ParameterMatcher(typeof(PredicateParameterMatcher))]
        public static T Is<T>(Func<T, bool> predicate)
        {
            return default(T);
        }
    }
}
