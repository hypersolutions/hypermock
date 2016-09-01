using System;
#if WINDOWS_UWP
using System.Reflection;
#endif

namespace HyperMock.Matchers
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class ParameterMatcherAttribute : Attribute
    {
        internal ParameterMatcherAttribute(Type type)
        {
#if WINDOWS_UWP
            if (!typeof(ParameterMatcher).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()))
#else
            if (!typeof(ParameterMatcher).IsAssignableFrom(type))
#endif
            {
                throw new ArgumentException($"Type {type} is not derived from ParameterMatcher.");
            }

            ParameterMatcherType = type;
        }

        internal Type ParameterMatcherType { get; }
    }
}
