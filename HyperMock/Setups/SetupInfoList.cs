using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using HyperMock.Core;

namespace HyperMock.Setups
{
    internal sealed class SetupInfoList
    {
        private readonly List<SetupInfo> _setupInfoList = new List<SetupInfo>();

        internal IEnumerable<SetupInfo> InfoList => _setupInfoList;

        internal SetupInfo AddOrGet(LambdaExpression expression, CallType callType)
        {
            switch (callType)
            {
                case CallType.GetProperty:
                    return AddOrGetGetter(expression);
                case CallType.SetProperty:
                    return AddOrGetSetter(expression);
                case CallType.Function:
                case CallType.Method:
                    return AddOrGetMethod(expression);
            }

            throw new NotSupportedException();
        }

        internal SetupInfo FindBy(string name, object[] args)
        {
            var parameterList = new ParameterList();

            foreach (var setupInfo in _setupInfoList.Where(d => d.Name == name))
            {
                var parameters = setupInfo.Parameters.Where(p => p.Type == ParameterType.In);
                
                if (!parameters.Any() && (args?.Length ?? 0) == 0) return setupInfo;
                if (args?.Length < parameters.Count()) return null;

                var inArgs = new object[parameters.Count()];
                Array.Copy(args, 0, inArgs, 0, inArgs.Length);

                if (parameterList.IsMatchFor(parameters.ToArray(), inArgs))
                    return setupInfo;
            }

            return null;
        }

        private SetupInfo AddOrGetGetter(LambdaExpression expression)
        {
            var body = expression.Body as MemberExpression;
            MethodInfo getMethodInfo;
            MethodCallExpression methodCall = null;

            if (body == null)
            {
                methodCall = expression.Body as MethodCallExpression;
                getMethodInfo = methodCall?.Method;
            }
            else
            {
                var propInfo = (PropertyInfo)body.Member;
                getMethodInfo = propInfo.GetMethod;
            }

            if (getMethodInfo == null)
                throw new ArgumentException("Expression refers to property or indexer with no getter.");


            var parameterList = new ParameterList();
            var parameters = parameterList.BuildFrom(methodCall, expression);

            var matchedSetupInfo = FindSetupInfo(getMethodInfo.Name, parameters);

            if (matchedSetupInfo != null) return matchedSetupInfo;

            var setupInfo = new SetupInfo {Name = getMethodInfo.Name, Parameters = parameters};
            _setupInfoList.Add(setupInfo);
            return setupInfo;
        }

        private SetupInfo AddOrGetSetter(LambdaExpression expression)
        {
            var body = expression.Body as MemberExpression;
            MethodInfo setMethodInfo;
            MethodCallExpression methodCall = null;

            if (body == null)
            {
                methodCall = expression.Body as MethodCallExpression;
                setMethodInfo = methodCall?.Method;
            }
            else
            {
                var propInfo = (PropertyInfo)body.Member;
                setMethodInfo = propInfo.SetMethod;
            }

            if (setMethodInfo == null)
                throw new ArgumentException("Expression refers to property or indexer with no setter.");

            // Special case! The way the mock is setup means that for sets the method may come through as a get!
            var name = setMethodInfo.Name.Replace("get_Item", "set_Item");

            var parameterList = new ParameterList();
            var parameters = parameterList.BuildFrom(methodCall, expression);

            var matchedSetupInfo = FindSetupInfo(name, parameters);

            if (matchedSetupInfo != null) return matchedSetupInfo;

            var setupInfo = new SetupInfo { Name = name, Parameters = parameters };
            _setupInfoList.Add(setupInfo);
            return setupInfo;
        }

        private SetupInfo AddOrGetMethod(LambdaExpression expression)
        {
            var body = expression.Body as MethodCallExpression;

            if (body == null)
                throw new ArgumentException("Expression body is not a MethodCallExpression.");

            var parameterList = new ParameterList();
            var parameters = parameterList.BuildFrom(body, expression);

            var matchedSetupInfo = FindSetupInfo(body.Method.Name, parameters);

            if (matchedSetupInfo != null) return matchedSetupInfo;

            var setupInfo = new SetupInfo {Name = body.Method.Name, Parameters = parameters.ToArray()};
            _setupInfoList.Add(setupInfo);

            return setupInfo;
        }

        private SetupInfo FindSetupInfo(string name, Parameter[] parameters)
        {
            var matchedSetupInfoList = _setupInfoList
                .Where(s => s.Name == name && s.Parameters.Length == parameters.Length)
                .ToList();

            if (matchedSetupInfoList.Any())
            {
                foreach (var matchedSetupInfo in matchedSetupInfoList)
                {
                    var matched = true;

                    for (var i = 0; i < parameters.Length; i++)
                    {
                        if (parameters[i].Matcher.GetType() != matchedSetupInfo.Parameters[i].Matcher.GetType())
                        {
                            matched = false;
                            break;
                        }

                        if (!parameters[i].Matcher.IsMatch(parameters[i].Value, matchedSetupInfo.Parameters[i].Value))
                        {
                            matched = false;
                            break;
                        }
                    }

                    if (matched)
                        return matchedSetupInfo;
                }
            }

            return null;
        }
    }
}
