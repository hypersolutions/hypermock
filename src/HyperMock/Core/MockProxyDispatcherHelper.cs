﻿using System;
using System.Linq;
using System.Reflection;
using HyperMock.Exceptions;
using HyperMock.Setups;

namespace HyperMock.Core
{
    internal sealed class MockProxyDispatcherHelper
    {
        private readonly IMockProxyDispatcher _dispatcher;

        internal MockProxyDispatcherHelper(IMockProxyDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        internal DispatcherResponse Handle(MethodBase method, object[] inArgs, object[] allArgs)
        {
            _dispatcher.Visits.Record(method, inArgs);

            var response = new DispatcherResponse {ReturnArgs = allArgs};

            var setupInfo = _dispatcher.Setups.FindBy(method.Name, inArgs);

            if (setupInfo == null)
            {
                if (_dispatcher.MockBehavior == MockBehavior.Strict)
                    throw new StrictMockViolationException(method);

                BuildDefaultResponse(method, response);
            }
            else
            {
                BuildSetupResponse(setupInfo, response);
            }

            return response;
        }

        private static void BuildDefaultResponse(MethodBase method, DispatcherResponse response)
        {
            var methodInfo = (MethodInfo)method;

            if (methodInfo.ReturnType != typeof(void) && methodInfo.ReturnType.IsValueType)
            {
                response.ReturnValue = Activator.CreateInstance(methodInfo.ReturnType);
            }
        }

        private static void BuildSetupResponse(SetupInfo setupInfo, DispatcherResponse response)
        {
            var setupValue = setupInfo.GetValue();

            if (setupValue?.IsException == true)
            {
                response.Exception = (Exception)setupValue.Value;
            }
            else
            {
                dynamic value = setupValue?.Value;
                response.ReturnValue = IsDeferredFunc(value) ? value() : value;

                var outAndRefParams = setupInfo.Parameters.Where(
                    p => p.Type is ParameterType.Out or ParameterType.Ref);

                foreach (var outAndRefParam in outAndRefParams)
                {
                    var index = Array.IndexOf(setupInfo.Parameters, outAndRefParam);
                    response.ReturnArgs[index] = outAndRefParam.Value;
                }
            }
        }

        private static bool IsDeferredFunc(object value)
        {
            if (value == null) return false;

            var valueType = value.GetType();

            return valueType.GetTypeInfo().IsGenericType &&
                   valueType.GetTypeInfo().GetGenericTypeDefinition() == typeof(Func<>);
        }
    }
}
