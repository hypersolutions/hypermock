using System;
using System.Linq.Expressions;

namespace HyperMock.Core
{
    internal abstract class MethodCallHelperBase
    {
        internal abstract TAttr GetCustomAttribute<TAttr>(MethodCallExpression methodCall) where TAttr : Attribute;
    }
}
