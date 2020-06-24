namespace HyperMock.Core
{
    internal class PredicateHelper
    {
        internal bool Invoke(dynamic predicate, object value)
        {
            return (bool) predicate((dynamic) value);
        }
    }
}
