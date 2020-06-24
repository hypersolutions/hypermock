using System;
using HyperMock.Setups;

namespace HyperMock.Behaviors
{
    /// <summary>
    /// Provides get property call behaviours to be added.
    /// </summary>
    public class GetPropertyCall<TReturn>
    {
        internal GetPropertyCall(SetupInfo setupInfo)
        {
            SetupInfo = setupInfo;
        }

        internal SetupInfo SetupInfo { get; }

        /// <summary>
        /// The mocked type property returns this value.
        /// </summary>
        /// <param name="returnValue">Value to return</param>
        public void Returns(TReturn returnValue)
        {
            SetupInfo.Value = returnValue;
        }

        /// <summary>
        /// The mocked get property returns the resolved value at the point in which it is called.
        /// </summary>
        /// <param name="func">Func to call</param>
        public void Returns(Func<TReturn> func)
        {
            SetupInfo.Value = func;
        }

        /// <summary>
        /// The mocked type method or parameter throws an exception.
        /// </summary>
        /// <typeparam name="TException">Exception type</typeparam>
        public void Throws<TException>() where TException : Exception, new()
        {
            Throws(new TException());
        }

        /// <summary>
        /// The mocked type method or parameter throws an exception.
        /// </summary>
        /// <param name="exception">Exception instance to throw</param>
        public void Throws(Exception exception)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));
            
            SetupInfo.Exception = exception;
        }
    }
}