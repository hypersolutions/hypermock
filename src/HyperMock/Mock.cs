using System;
using System.Linq;
using System.Linq.Expressions;
using HyperMock.Behaviors;
using HyperMock.Core;

namespace HyperMock
{
    /// <summary>
    /// Provides a mock wrapper around the generated proxy type that groups the proxy and its dispatcher.
    /// </summary>
    public class Mock
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="obj">Proxy object</param>
        /// <param name="dispatcher">Dispatcher</param>
        /// <param name="behavior">Mock behavior to apply</param>
        public Mock(object obj, IMockProxyDispatcher dispatcher, MockBehavior behavior)
        {
            Object = obj;
            Dispatcher = dispatcher;
            Dispatcher.MockBehavior = behavior;
        }

        /// <summary>
        /// Gets the proxy instance.
        /// </summary>
        public object Object { get; }

        internal IMockProxyDispatcher Dispatcher { get; }

        /// <summary>
        /// Creates a proxy from a template interface.
        /// </summary>
        /// <typeparam name="T">Interface type</typeparam>
        /// <returns>Proxy instance</returns>
        public static Mock<T> Create<T>(MockBehavior behavior = MockBehavior.Loose)
        {
            CheckInstanceType(typeof(T));

            return MockHelper.Create<T>(behavior);
        }

        /// <summary>
        /// Creates a proxy from a interface type.
        /// </summary>
        /// <param name="type">Interface type</param>
        /// <param name="behavior">Mock behavior</param>
        /// <returns>Proxy instance</returns>
        public static Mock Create(Type type, MockBehavior behavior = MockBehavior.Loose)
        {
            CheckInstanceType(type);

            return MockHelper.Create(type, behavior);
        }

        /// <summary>
        /// Creates a group to ensure that all calls are reset when the group disposes inside a using block.
        /// This provides support for situations whereby you require static mocks to be reused across tests and need 
        /// visit metrics to be reset.
        /// </summary>
        /// <param name="mocks">List of mocks to reset</param>
        public static MockCallGroupContainer CallGroup(params Mock[] mocks)
        {
            return new MockCallGroupContainer(mocks);
        }

        private static void CheckInstanceType(Type instanceType)
        {
            if (!TypeHelper.IsInterface(instanceType))
                throw new NotSupportedException("Only interface types are supported for proxy generation.");
        }
    }

    /// <summary>
    /// Provides a mock wrapper around the generated proxy type that groups the proxy and its dispatcher.
    /// </summary>
    /// <typeparam name="T">Proxy type to mock</typeparam>
    public sealed class Mock<T> : Mock
    {
        private EventDispatcher<T> _eventDispatcher;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="obj">Proxy object</param>
        /// <param name="dispatcher">Dispatcher</param>
        /// <param name="behavior">Mock behavior to apply</param>
        public Mock(T obj, IMockProxyDispatcher dispatcher, MockBehavior behavior) : base(obj, dispatcher, behavior)
        {
        }

        /// <summary>
        /// Gets the proxy instance generated.
        /// </summary>
        public new T Object => (T) base.Object;

        /// <summary>
        /// Sets up the behaviour on a method.
        /// </summary>
        /// <param name="expression">Method expression</param>
        /// <returns>Function call behaviours</returns>
        public MethodCall Setup(Expression<Action<T>> expression)
        {
            var setupInfo = Dispatcher.Setups.AddOrGet(expression, CallType.Method);

            return new MethodCall(setupInfo);
        }

        /// <summary>
        /// Sets up the behaviour on a function.
        /// </summary>
        /// <typeparam name="TReturn">Return type</typeparam>
        /// <param name="expression">Function expression</param>
        /// <returns>Function call behaviours</returns>
        public FunctionCall<TReturn> Setup<TReturn>(Expression<Func<T, TReturn>> expression)
        {
            var setupInfo = Dispatcher.Setups.AddOrGet(expression, CallType.Function);

            return new FunctionCall<TReturn>(setupInfo);
        }

        /// <summary>
        /// Sets up the behaviour on a get property.
        /// </summary>
        /// <typeparam name="TReturn">Return type</typeparam>
        /// <param name="expression">Get property expression</param>
        /// <returns>Get property call behaviours</returns>
        public GetPropertyCall<TReturn> SetupGet<TReturn>(Expression<Func<T, TReturn>> expression)
        {
            var setupInfo = Dispatcher.Setups.AddOrGet(expression, CallType.GetProperty);

            return new GetPropertyCall<TReturn>(setupInfo);
        }

        /// <summary>
        /// Sets up the behaviour on a set property.
        /// </summary>
        /// <param name="expression">Set property expression</param>
        /// <returns>Method call behaviours</returns>
        public SetPropertyCall<TValue> SetupSet<TValue>(Expression<Func<T, TValue>> expression)
        {
            var setupInfo = Dispatcher.Setups.AddOrGet(expression, CallType.SetProperty);

            return new SetPropertyCall<TValue>(setupInfo);
        }
        
        /// <summary>
        /// Verifies the call occurrence of the method described.
        /// </summary>
        /// <param name="expression">Method expression</param>
        /// <param name="occurred">Occurrence pattern to check</param>
        public void Verify(Expression<Action<T>> expression, Occurred occurred)
        {
            var visits = Dispatcher.Visits.FindBy(expression, CallType.Method);

            var count = visits.Sum(v => v.VisitCount);

            occurred.Assert(count);
        }

        /// <summary>
        /// Verifies the call occurrence of the function described.
        /// </summary>
        /// <param name="expression">Function expression</param>
        /// <param name="occurred">Occurrence pattern to check</param>
        public void Verify<TReturn>(Expression<Func<T, TReturn>> expression, Occurred occurred)
        {
            var visits = Dispatcher.Visits.FindBy(expression, CallType.Function);

            var count = visits.Sum(v => v.VisitCount);

            occurred.Assert(count);
        }

        /// <summary>
        /// Verifies the call occurrence of the get property described.
        /// </summary>
        /// <param name="expression">Get property expression</param>
        /// <param name="occurred">Occurrence pattern to check</param>
        public void VerifyGet<TReturn>(Expression<Func<T, TReturn>> expression, Occurred occurred)
        {
            var visits = Dispatcher.Visits.FindBy(expression, CallType.GetProperty);

            var count = visits.Sum(v => v.VisitCount);

            occurred.Assert(count);
        }

        /// <summary>
        /// Verifies the call occurrence of the set property described.
        /// </summary>
        /// <param name="expression">Set property expression</param>
        /// <param name="occurred">Occurrence pattern to check</param>
        public void VerifySet<TValue>(Expression<Func<T, TValue>> expression, Occurred occurred)
        {
            var visits = Dispatcher.Visits.FindBy(expression, CallType.SetProperty);

            var count = visits.Sum(v => v.VisitCount);

            occurred.Assert(count);
        }

        /// <summary>
        /// Verifies the call occurrence of the set property described.
        /// </summary>
        /// <param name="expression">Set property expression</param>
        /// <param name="propertyValue">Set value to check occurred</param>
        /// <param name="occurred">Occurrence pattern to check</param>
        public void VerifySet<TValue>(Expression<Func<T, TValue>> expression, TValue propertyValue, Occurred occurred)
        {
            var visits = Dispatcher.Visits.FindBy(expression, CallType.SetProperty, new object[] {propertyValue});

            var count = visits.Sum(v => v.VisitCount);

            occurred.Assert(count);
        }

        /// <summary>
        /// Raises the event with the event args.
        /// </summary>
        /// <typeparam name="TArgs">Event args type</typeparam>
        /// <param name="expression">Event handler expression</param>
        /// <param name="args">Event args instance</param>
        public void Raise<TArgs>(Action<T> expression, TArgs args) where TArgs : EventArgs
        {
            _eventDispatcher ??= new EventDispatcher<T>(this);

            _eventDispatcher.Raise(expression, args);
        }
    }
}
