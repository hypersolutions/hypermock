using System;
#if WINDOWS_UWP
using System.Reflection;
#else
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
#endif
using HyperMock.Core;
using HyperMock.Setups;

namespace HyperMock
{
#if WINDOWS_UWP
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

        VisitList IMockProxyDispatcher.Visits { get; } = new VisitList();

        SetupInfoList IMockProxyDispatcher.Setups { get; } = new SetupInfoList();

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            var response = _helper.Handle(targetMethod, args, args);

            if (response.Exception != null)
                throw response.Exception;

            return response.ReturnValue;
        }
    }
#else
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
#endif    
}
