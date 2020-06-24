using System;
using HyperMock.Core;

namespace HyperMock.Matchers
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class ParameterMatcherAttribute : Attribute
    {
        internal ParameterMatcherAttribute(Type type)
        {
            var typeHelper = new TypeHelper();

            if (!typeHelper.IsAssignableFrom(typeof(ParameterMatcher), type))
                throw new ArgumentException($"Type {type} is not derived from ParameterMatcher.");

            ParameterMatcherType = type;
        }

        internal Type ParameterMatcherType { get; }
    }
}
