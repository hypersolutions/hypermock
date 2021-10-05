using System;

namespace HyperMock.Exceptions
{
    /// <summary>
    /// Defines a verification framework exception thrown when a verify check fails.
    /// </summary>
    public class VerificationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="message">Error message</param>
        public VerificationException(string message) : base(message)
        {

        }
    }
}