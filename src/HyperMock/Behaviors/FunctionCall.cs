using System;
using System.Linq;
using HyperMock.Core;
using HyperMock.Exceptions;
using HyperMock.Setups;

namespace HyperMock.Behaviors
{
    /// <summary>
    /// Provides method call behaviours to be added.
    /// </summary>
    public class FunctionCall<TReturn>
    {
        internal FunctionCall(SetupInfo setupInfo)
        {
            SetupInfo = setupInfo;
        }
        
        internal SetupInfo SetupInfo { get; }

        /// <summary>
        /// The mocked function returns this value.
        /// </summary>
        /// <param name="returnValue">Value to return</param>
        public FunctionCall<TReturn> Returns(TReturn returnValue)
        {
            SetupInfo.AddValue(returnValue);
            return this;
        }

        /// <summary>
        /// The mocked function returns the resolved value at the point in which it is called.
        /// </summary>
        /// <param name="func">Func to call</param>
        public FunctionCall<TReturn> Returns(Func<TReturn> func)
        {
            SetupInfo.AddValue(func);
            return this;
        }

        /// <summary>
        /// Sets the out arg values to match the out params in the function. These must be in order and complete.
        /// </summary>
        /// <param name="args">List of args to set</param>
        /// <returns>Self</returns>
        public FunctionCall<TReturn> WithOutArgs(params object[] args)
        {
            var outParams = SetupInfo.Parameters.Where(p => p.Type == ParameterType.Out).ToArray();

            if (args.Length != outParams.Length)
                throw new MockException("Out parameter mismatch. Too many or not enough args have been provided.");

            for (var i = 0; i < args.Length; i++)
            {
                outParams[i].Value = args[i];
            }

            return this;
        }

        /// <summary>
        /// Sets the out arg values to match the out params in the function. These must be in order and complete.
        /// </summary>
        /// <param name="args">List of args to set</param>
        /// <returns>Self</returns>
        public FunctionCall<TReturn> WithRefArgs(params object[] args)
        {
            var refParams = SetupInfo.Parameters.Where(p => p.Type == ParameterType.Ref).ToArray();

            if (args.Length != refParams.Length)
                throw new MockException("Ref parameter mismatch. Too many or not enough args have been provided.");

            for (var i = 0; i < args.Length; i++)
            {
                refParams[i].Value = args[i];
            }

            return this;
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
