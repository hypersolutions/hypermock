using System;

namespace HyperMock.Core
{
    internal abstract class TypeHelperBase
    {
        internal abstract bool IsAssignableFrom(Type type, Type assignableType);

        internal abstract bool IsInterface(Type type);
    }
}
