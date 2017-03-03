using System;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace HyperMock.Core
{
    internal class MockHelper : MockHelperBase
    {
        internal override Mock<T> Create<T>(MockBehavior behavior)
        {
            var dispatcher = DispatchProxy.Create<T, MockProxyDispatcher>();
            return new Mock<T>(dispatcher, dispatcher as MockProxyDispatcher, behavior);
        }

        internal override Mock Create(Type type, MockBehavior behavior)
        {
            var generatorType = typeof(DispatchProxy).GetTypeInfo()
                .Assembly.GetType("System.Reflection.DispatchProxyGenerator");
            var method = generatorType.GetMethod("CreateProxyInstance", BindingFlags.NonPublic | BindingFlags.Static);
            var dispatcher = (MockProxyDispatcher)method.Invoke(
                null, new object[] { typeof(MockProxyDispatcher), type });

            var constructedType = typeof(Mock<>).MakeGenericType(type);
            return (Mock)Activator.CreateInstance(constructedType, dispatcher, dispatcher, behavior);
        }
    }
}
