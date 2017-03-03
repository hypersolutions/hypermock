using System;

namespace HyperMock.Core
{
    internal abstract class MockHelperBase
    {
        internal abstract Mock<T> Create<T>(MockBehavior behavior);

        internal abstract Mock Create(Type type, MockBehavior behavior);
    }
}
