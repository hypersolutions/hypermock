using System;

namespace HyperMock.Core
{
    internal abstract class MockHelperBase
    {
        internal abstract Mock<T> Create<T>();

        internal abstract Mock Create(Type type);
    }
}
