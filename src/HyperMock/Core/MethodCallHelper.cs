using System;
using System.Linq;
using System.Linq.Expressions;

namespace HyperMock.Core
{
    internal static class MethodCallHelper
    {
        internal static TAttr GetCustomAttribute<TAttr>(MethodCallExpression methodCall) where TAttr : Attribute
        {
            return methodCall?.Method.GetCustomAttributes(typeof(TAttr), false).FirstOrDefault() as TAttr;
        }
    }
}
