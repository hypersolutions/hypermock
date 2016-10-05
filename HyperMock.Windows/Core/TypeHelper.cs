using System;

// ReSharper disable once CheckNamespace
namespace HyperMock.Core
{
    internal class TypeHelper : TypeHelperBase
    {
        internal override bool IsAssignableFrom(Type type, Type assignableType)
        {
            return type.IsAssignableFrom(assignableType);
        }

        internal override bool IsInterface(Type type)
        {
            return type.IsInterface;
        }
    }
}
