using System.Text.RegularExpressions;

namespace HyperMock.Matchers
{
    internal class RegexParameterMatcher : ParameterMatcher
    {
        private readonly string _pattern;

        internal RegexParameterMatcher(string pattern)
        {
            _pattern = pattern ?? "";
        }

        internal override bool IsMatch(object expected, object actual)
        {
            return actual != null && Regex.IsMatch(actual.ToString(), _pattern);
        }
    }
}