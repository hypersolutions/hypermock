using System.Linq;
using System.Linq.Expressions;

// ReSharper disable once CheckNamespace
namespace HyperMock.Core
{
    internal class MethodCallHelper : MethodCallHelperBase
    {
        internal override TAttr GetCustomAttribute<TAttr>(MethodCallExpression methodCall)
        {
            return methodCall?.Method.GetCustomAttributes(typeof(TAttr), false).FirstOrDefault() as TAttr;
        }
    }
}
