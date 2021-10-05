﻿using System;
using System.Linq.Expressions;
using HyperMock.Core;

namespace HyperMock.Matchers
{
    internal static class ParameterMatcherFactory
    {
        internal static ParameterMatcher Create(LambdaExpression expression)
        {
            var methodCall = expression.Body as MethodCallExpression;

            // Find the parameter matcher attribute (if exists)
            var paramMatcherAttr = MethodCallHelper.GetCustomAttribute<ParameterMatcherAttribute>(methodCall);

            // If exists then create the matcher else return the exact matcher
            return paramMatcherAttr != null 
                ? CreateFromExpression(paramMatcherAttr.ParameterMatcherType, expression) 
                : new ExactParameterMatcher();
        }

        private static ParameterMatcher CreateFromExpression(Type paramMatcherType, LambdaExpression expression)
        {
            var methodCall = (MethodCallExpression)expression.Body;

            var matcher = (ParameterMatcher)Activator.CreateInstance(paramMatcherType);
            matcher!.CallContext = methodCall;
            return matcher;
        }
    }
}