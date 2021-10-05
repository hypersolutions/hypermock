namespace HyperMock.Core
{
    internal static class PredicateHelper
    {
        internal static bool Invoke(dynamic predicate, object value)
        {
            return (bool) predicate((dynamic) value);
        }
    }
}
