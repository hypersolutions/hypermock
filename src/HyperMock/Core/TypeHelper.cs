using System;
using System.Reflection;

namespace HyperMock.Core
{
    internal class TypeHelper 
    {
        internal bool IsAssignableFrom(Type type, Type assignableType)
        {
            return type.GetTypeInfo().IsAssignableFrom(assignableType.GetTypeInfo());
        }

        internal bool IsInterface(Type type)
        {
            return type.GetTypeInfo().IsInterface;
        }
    }
}
