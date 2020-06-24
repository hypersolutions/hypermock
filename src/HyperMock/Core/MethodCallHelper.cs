using System;
using System.Linq;
using System.Linq.Expressions;

namespace HyperMock.Core
{
    internal class MethodCallHelper
    {
        internal TAttr GetCustomAttribute<TAttr>(MethodCallExpression methodCall) where TAttr : Attribute
        {
            return methodCall?.Method.GetCustomAttributes(typeof(TAttr), false).FirstOrDefault() as TAttr;
        }
    }
}
