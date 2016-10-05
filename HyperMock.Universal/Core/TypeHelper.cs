using System;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace HyperMock.Core
{
    internal class TypeHelper : TypeHelperBase
    {
        internal override bool IsAssignableFrom(Type type, Type assignableType)
        {
            return type.GetTypeInfo().IsAssignableFrom(assignableType.GetTypeInfo());
        }

        internal override bool IsInterface(Type type)
        {
            return type.GetTypeInfo().IsInterface;
        }
    }
}
