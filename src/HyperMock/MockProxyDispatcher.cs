using System.Reflection;
using HyperMock.Core;
using HyperMock.Setups;

namespace HyperMock
{
    /// <summary>
    /// Provides the interceptor for all calls and records and resolves behaviors for each call.
    /// </summary>
    public class MockProxyDispatcher : DispatchProxy, IMockProxyDispatcher
    {
        private readonly MockProxyDispatcherHelper _helper;

        public MockProxyDispatcher()
        {
            _helper = new MockProxyDispatcherHelper(this);
        }

        VisitList IMockProxyDispatcher.Visits { get; } = new();

        SetupInfoList IMockProxyDispatcher.Setups { get; } = new();

        MockBehavior IMockProxyDispatcher.MockBehavior { get; set; }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            var response = _helper.Handle(targetMethod, args, args);

            if (response.Exception != null)
                throw response.Exception;

            return response.ReturnValue;
        }
    }
}
