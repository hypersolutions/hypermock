namespace HyperMock.Matchers
{
    internal class ExactParameterMatcher : ParameterMatcher
    {
        internal override bool IsMatch(object expected, object actual)
        {
            return Equals(expected, actual);
        }
    }
}