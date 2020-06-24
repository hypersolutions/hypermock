using System;
using System.Reflection;

namespace HyperMock.Exceptions
{
    /// <summary>
    /// Defines an exception for violations of the strict mock setup.
    /// </summary>
    public sealed class StrictMockViolationException : Exception
    {
        private const string MessageTemplate = 
            "There is no setup for the call '{0}'. All calls using a strict mock must be defined.";

        internal StrictMockViolationException(MethodBase method) : base(string.Format(MessageTemplate, method.Name))
        {
            
        }
    }
}
