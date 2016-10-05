// ReSharper disable once CheckNamespace
namespace HyperMock.Core
{
    internal class PredicateHelper : PredicateHelperBase
    {
        internal override bool Invoke(dynamic predicate, object value)
        {
            return (bool) predicate((dynamic) value);
        }
    }
}
