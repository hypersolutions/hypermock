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
                if (parameterList.IsMatchFor(setupInfo.Parameters ?? new Parameter[0], args ?? new object[0]))
                    return setupInfo;
            }

            return null;
        }

        private SetupInfo AddOrGetGetter(LambdaExpression expression)
        {
            var body = expression.Body as MemberExpression;

            if (body == null)
                throw new ArgumentException("Expression body is not a MemberExpression.");

            var propInfo = (PropertyInfo)body.Member;
            var getMethodInfo = propInfo.GetMethod;

            if (getMethodInfo == null)
                throw new ArgumentException("Expression refers to property with no getter.");

            var setupInfo = _setupInfoList.FirstOrDefault(s => s.Name == getMethodInfo.Name);

            if (setupInfo != null) return setupInfo;

            setupInfo = new SetupInfo {Name = getMethodInfo.Name};
            _setupInfoList.Add(setupInfo);
            return setupInfo;
        }

        private SetupInfo AddOrGetSetter(LambdaExpression expression)
        {
            var body = expression.Body as MemberExpression;

            if (body == null)
                throw new ArgumentException("Expression body is not a MemberExpression.");

            var propInfo = (PropertyInfo)body.Member;
            var setMethodInfo = propInfo.SetMethod;

            if (setMethodInfo == null)
                throw new ArgumentException("Expression refers to property with no setter.");

            var setupInfo = _setupInfoList.FirstOrDefault(s => s.Name == setMethodInfo.Name);

            if (setupInfo != null) return setupInfo;

            setupInfo = new SetupInfo { Name = setMethodInfo.Name };
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

            var matchedSetupInfoList = _setupInfoList
                .Where(s => s.Name == body.Method.Name && s.Parameters.Length == parameters.Length)
                .ToList();

            if (matchedSetupInfoList.Any())
            {
                foreach (var matchedSetupInfo in matchedSetupInfoList)
                {
                    var matched = true;

                    for (var i = 0; i < parameters.Length; i++)
                    {
                        if (parameters[i].Matcher.GetType() != matchedSetupInfo.Parameters[i].GetType())
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

            var setupInfo = new SetupInfo {Name = body.Method.Name, Parameters = parameters.ToArray()};
            _setupInfoList.Add(setupInfo);
            return setupInfo;
        }
    }
}
