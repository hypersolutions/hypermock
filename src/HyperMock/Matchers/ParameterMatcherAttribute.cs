using System;
using HyperMock.Core;

namespace HyperMock.Matchers
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class ParameterMatcherAttribute : Attribute
    {
        internal ParameterMatcherAttribute(Type type)
        {
            if (!TypeHelper.IsAssignableFrom(typeof(ParameterMatcher), type))
                throw new ArgumentException($"Type {type} is not derived from ParameterMatcher.");

            ParameterMatcherType = type;
        }

        internal Type ParameterMatcherType { get; }
    }
}
