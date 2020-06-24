using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace HyperMock.Matchers
{
    internal class RegexParameterMatcher : ParameterMatcher
    {
        internal override bool IsMatch(object expected, object actual)
        {
            var pattern = GetPattern();
            return actual != null && Regex.IsMatch(actual.ToString(), pattern);
        }

        private string GetPattern()
        {
            if (CallContext?.Arguments == null || CallContext.Arguments.Count != 1) return "";

            var predicate = CallContext.Arguments[0];
            var compiledPredicate = Expression.Lambda(predicate).Compile();
            var value = compiledPredicate.DynamicInvoke() as string ?? "";
            return value;
        }
    }
}