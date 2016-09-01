namespace HyperMock.Matchers
{
    internal abstract class ParameterMatcher
    {
        internal abstract bool IsMatch(object expected, object actual);
    }
}