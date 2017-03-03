using System;

// ReSharper disable once CheckNamespace
namespace HyperMock.Core
{
    internal class MockHelper : MockHelperBase
    {
        internal override Mock<T> Create<T>(MockBehavior behavior)
        {
            var dispatcher = new MockProxyDispatcher(typeof(T));
            return new Mock<T>((T)dispatcher.GetTransparentProxy(), dispatcher, behavior);
        }

        internal override Mock Create(Type type, MockBehavior behavior)
        {
            var dispatcher = new MockProxyDispatcher(type);
            return new Mock(dispatcher.GetTransparentProxy(), dispatcher, behavior);
        }
    }
}
