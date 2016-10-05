using System;

// ReSharper disable once CheckNamespace
namespace HyperMock.Core
{
    internal class MockHelper : MockHelperBase
    {
        internal override Mock<T> Create<T>()
        {
            var dispatcher = new MockProxyDispatcher(typeof(T));
            return new Mock<T>((T)dispatcher.GetTransparentProxy(), dispatcher);
        }

        internal override Mock Create(Type type)
        {
            var dispatcher = new MockProxyDispatcher(type);
            return new Mock(dispatcher.GetTransparentProxy(), dispatcher);
        }
    }
}
