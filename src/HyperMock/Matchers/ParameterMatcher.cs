using System.Linq.Expressions;

namespace HyperMock.Matchers
{
    internal abstract class ParameterMatcher
    {
        internal MethodCallExpression CallContext { get; set; }

        internal abstract bool IsMatch(object expected, object actual);
    }
}