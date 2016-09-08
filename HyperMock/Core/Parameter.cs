using HyperMock.Matchers;

namespace HyperMock.Core
{
    internal class Parameter
    {
        internal Parameter()
        {
            Matcher = new ExactParameterMatcher();
        }

        internal object Value { get; set; }
        internal ParameterMatcher Matcher { get; set; }
        internal ParameterType Type { get; set; }
    }
}