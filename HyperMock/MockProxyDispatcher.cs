using System;
using System.Reflection;
#if !WINDOWS_UWP
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
    public class MockProxyDispatcher : DispatchProxy
    {
        public MockProxyDispatcher()
        {
            Visits = new VisitList();
            Setups = new SetupInfoList();
        }

        internal VisitList Visits { get; }

        internal SetupInfoList Setups { get; }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            Visits.Record(targetMethod, args);

            var setupInfo = Setups.FindBy(targetMethod.Name, args);

            if (setupInfo == null)
            {
                if (targetMethod.ReturnType == typeof(void))
                    return null;

                if (targetMethod.ReturnType.GetTypeInfo().IsValueType)
                {
                    return Activator.CreateInstance(targetMethod.ReturnType);
                }
            }
            else if (setupInfo.Exception != null)
            {
                throw setupInfo.Exception;
            }
            else
            {
                return setupInfo.Value;
            }
            
            return null;
        }
    }
#else
    /// <summary>
    /// Provides the interceptor for all calls and records and resolves behaviors for each call.
    /// </summary>
    public class MockProxyDispatcher : RealProxy
    {
        public MockProxyDispatcher(Type interfaceType) : base(interfaceType)
        {
            Visits = new VisitList();
            Setups = new SetupInfoList();
        }

        internal VisitList Visits { get; }

        internal SetupInfoList Setups { get; }

        public override IMessage Invoke(IMessage msg)
        {
            var methodCall = msg as IMethodCallMessage;

            if (methodCall == null)
                throw new NotSupportedException($"Proxy invoke called with an unsupported message: {msg}");

            Visits.Record(methodCall.MethodBase, methodCall.Args);

            var setupInfo = Setups.FindBy(methodCall.MethodName, methodCall.Args);

            object returnInstance = null;

            if (setupInfo == null)
            {
                var methodInfo = (MethodInfo) methodCall.MethodBase;

                if (methodInfo.ReturnType != typeof(void) && methodInfo.ReturnType.IsValueType)
                {
                    returnInstance = Activator.CreateInstance(methodInfo.ReturnType);
                }
            }
            else if (setupInfo.Exception != null)
            {
                return new ReturnMessage(setupInfo.Exception, methodCall);
            }
            else
            {
                returnInstance = setupInfo.Value;
            }

            return new ReturnMessage(returnInstance, null, 0, null, methodCall);
        }
    }
#endif
}
