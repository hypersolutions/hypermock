using System;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using HyperMock.Core;
using HyperMock.Setups;

// ReSharper disable once CheckNamespace
namespace HyperMock
{
    /// <summary>
    /// Provides the interceptor for all calls and records and resolves behaviors for each call.
    /// </summary>
    public class MockProxyDispatcher : RealProxy, IMockProxyDispatcher
    {
        private readonly MockProxyDispatcherHelper _helper;

        public MockProxyDispatcher(Type interfaceType) : base(interfaceType)
        {
            _helper = new MockProxyDispatcherHelper(this);
        }

        VisitList IMockProxyDispatcher.Visits { get; } = new VisitList();

        SetupInfoList IMockProxyDispatcher.Setups { get; } = new SetupInfoList();

        MockBehavior IMockProxyDispatcher.MockBehavior { get; set; }

        public override IMessage Invoke(IMessage msg)
        {
            var methodCall = msg as IMethodCallMessage;

            if (methodCall == null)
                throw new NotSupportedException($"Proxy invoke called with an unsupported message: {msg}");

            var response = _helper.Handle(methodCall.MethodBase, methodCall.InArgs, methodCall.Args);

            if (response.Exception != null)
                return new ReturnMessage(response.Exception, methodCall);

            return new ReturnMessage(
                response.ReturnValue,
                response.ReturnArgs,
                response.ReturnArgs?.Length ?? 0,
                methodCall.LogicalCallContext,
                methodCall);
        }
    }
}
