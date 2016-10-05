namespace HyperMock.Core
{
    internal abstract class PredicateHelperBase
    {
        internal abstract bool Invoke(dynamic predicate, object value);
    }
}
