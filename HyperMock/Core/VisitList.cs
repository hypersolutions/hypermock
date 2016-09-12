using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace HyperMock.Core
{
    internal class VisitList
    {
        private readonly ParameterList _parameterList = new ParameterList();

        internal VisitList()
        {
            RecordedVisits = new List<Visit>();
        }

        internal List<Visit> RecordedVisits { get; }

        internal Visit Record(MethodBase method, object[] args)
        {
            var parameters = _parameterList.BuildFrom(method, args);

            var visit = RecordedVisits.FirstOrDefault(
                v => v.Name == method.Name && IsMatchFor(v.Parameters, parameters));

            if (visit != null)
            {
                visit.VisitCount++;
            }
            else
            {
                visit = new Visit(method.Name, parameters);
                RecordedVisits.Add(visit);
            }

            return visit;
        }

        internal Visit FindBy(LambdaExpression expression, CallType callType, object[] values = null)
        {
            switch (callType)
            {
                case CallType.Method:
                case CallType.Function:
                    return FindMethodVisit(expression);
                case CallType.GetProperty:
                    return FindGetPropertyVisit(expression, values);
                case CallType.SetProperty:
                    return FindSetPropertyVisit(expression, values);
            }

            return null;
        }

        private Visit FindMethodVisit(LambdaExpression expression)
        {
            var body = expression.Body as MethodCallExpression;

            if (body == null) return null;

            var matchingVisitationsByName = RecordedVisits.Where(v => v.Name == body.Method.Name).ToList();

            if (!matchingVisitationsByName.Any()) return null;

            var parameterList = new ParameterList();
            var parameters = parameterList.BuildFrom(body, expression);

            return matchingVisitationsByName.FirstOrDefault(
                v => parameterList.IsMatchFor(parameters, v.Parameters.Select(p => p.Value).ToArray()));
        }

        private Visit FindGetPropertyVisit(LambdaExpression expression, object[] values = null)
        {
            var body = expression.Body as MemberExpression;
            MethodInfo getMethodInfo;

            if (body == null)
            {
                var indexerBody = expression.Body as MethodCallExpression;
                getMethodInfo = indexerBody?.Method;
            }
            else
            {
                var propInfo = (PropertyInfo)body.Member;
                getMethodInfo = propInfo.GetMethod;
            }

            if (getMethodInfo == null) return null;

            var parameterList = new ParameterList();
            var parameters = parameterList.BuildFrom(getMethodInfo, values);

            if (values != null && values.Length > 0)
                return RecordedVisits.FirstOrDefault(
                    v => v.Name == getMethodInfo.Name && IsMatchFor(v.Parameters, parameters));

            return RecordedVisits.FirstOrDefault(v => v.Name == getMethodInfo.Name);
        }

        private Visit FindSetPropertyVisit(LambdaExpression expression, object[] values = null)
        {
            var body = expression.Body as MemberExpression;
            MethodInfo setMethodInfo;

            if (body == null)
            {
                var indexerBody = expression.Body as MethodCallExpression;
                setMethodInfo = indexerBody?.Method;
            }
            else
            {
                var propInfo = (PropertyInfo)body.Member;
                setMethodInfo = propInfo.SetMethod;
            }

            if (setMethodInfo == null) return null;

            var parameterList = new ParameterList();
            var parameters = parameterList.BuildFrom(setMethodInfo, values);

            // Special case! The way the mock is setup means that for sets the method may come through as a get!
            var name = setMethodInfo.Name.Replace("get_Item", "set_Item");

            if (values != null && values.Length > 0)
                return RecordedVisits.FirstOrDefault(
                    v => v.Name == name && IsMatchFor(v.Parameters, parameters));

            return RecordedVisits.FirstOrDefault(v => v.Name == name);
        }

        private static bool IsMatchFor(Parameter[] args, Parameter[] otherArgs)
        {
            if (args == null && otherArgs == null) return true;
            if (args == null || otherArgs == null) return false;
            if (args.Length != otherArgs.Length) return false;

            for (var i = 0; i < args.Length; i++)
            {
                if (!args[i].Matcher.IsMatch(args[i].Value, otherArgs[i].Value)) return false;
                //if (!Equals(args[i], otherArgs[i])) return false;
            }

            return true;
        }
    }
}
