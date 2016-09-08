using System;
using System.Collections.Generic;
using System.Linq;
using HyperMock.Core;
using HyperMock.Matchers;
using HyperMock.Setups;

namespace HyperMock.Behaviors
{
    /// <summary>
    /// Provides set property call behaviours to be added.
    /// </summary>
    public class SetPropertyCall<TValue>
    {
        internal SetPropertyCall(SetupInfo setupInfo)
        {
            SetupInfo = setupInfo;
        }

        internal SetupInfo SetupInfo { get; }

        /// <summary>
        /// The mocked type property sets this value.
        /// </summary>
        /// <param name="setValue">Value to return</param>
        public SetPropertyCall<TValue> SetValue(TValue setValue)
        {
            var parameter = new Parameter {Value = setValue, Matcher = new ExactParameterMatcher()};

            SetupInfo.Parameters = SetupInfo.Parameters != null
                ? new List<Parameter>(SetupInfo.Parameters).Union(new[] {parameter}).ToArray()
                : new[] {parameter};
            
            return this;
        }

        /// <summary>
        /// The mocked type property sets any value.
        /// </summary>
        public SetPropertyCall<TValue> AnySetValue()
        {
            SetupInfo.Parameters = new[] { new Parameter { Matcher = new AnyParameterMatcher() } };
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