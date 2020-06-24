using System;

namespace HyperMock.Exceptions
{
    /// <summary>
    /// Defines a general mock framework exception.
    /// </summary>
    public class MockException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="message">Error message</param>
        public MockException(string message) : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="innerException">Inner exception</param>
        public MockException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}