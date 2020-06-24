namespace HyperMock.Matchers
{
    internal class AnyParameterMatcher : ParameterMatcher
    {
        internal override bool IsMatch(object expected, object actual)
        {
            if (expected == null && actual == null) return true;
            if (expected != null && actual != null && expected.GetType() != actual.GetType()) return false;

            return true;
        }
    }
}