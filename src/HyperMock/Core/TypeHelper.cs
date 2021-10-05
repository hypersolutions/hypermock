using System;
using System.Reflection;

namespace HyperMock.Core
{
    internal static class TypeHelper 
    {
        internal static bool IsAssignableFrom(Type type, Type assignableType)
        {
            return type.GetTypeInfo().IsAssignableFrom(assignableType.GetTypeInfo());
        }

        internal static bool IsInterface(Type type)
        {
            return type.GetTypeInfo().IsInterface;
        }
    }
}
