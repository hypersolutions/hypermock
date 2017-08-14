using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HyperMock;

namespace Tests.HyperMock
{
    /// <summary>
    /// Helper base class that provides automatic initialisation of mock dependencies.
    /// </summary>
    /// <typeparam name="TSubject">Class under test</typeparam>
    public abstract class TestBase<TSubject>
    {
        private readonly Dictionary<Type, Mock> _mocks = new Dictionary<Type, Mock>();
        
        protected TestBase()
        {
            _mocks.Clear();

            var ctor = typeof(TSubject).GetConstructors().First();

            foreach (var ctorParam in ctor.GetParameters())
            {
                var method = typeof(Mock).GetMethod("Create", new[] { typeof(MockBehavior) });
                var generic = method.MakeGenericMethod(ctorParam.ParameterType);
                var mock = (Mock)generic.Invoke(this, new object[] { MockBehavior.Loose });

                _mocks.Add(ctorParam.ParameterType, mock);
            }

            Subject = (TSubject)ctor.Invoke(_mocks.Values.Select(m => m.Object).ToArray());
        }

        protected TSubject Subject { get; }

        protected Mock<TInterface> MockFor<TInterface>() where TInterface : class
        {
            if (_mocks.ContainsKey(typeof(TInterface)))
                return (Mock<TInterface>)_mocks[typeof(TInterface)];

            throw new InvalidOperationException("Cannot find mock for type: " + typeof(TInterface).FullName);
        }
    }
}
