using System;
using System.Collections.Generic;
using HyperMock.Core;

namespace HyperMock.Setups
{
    internal sealed class SetupInfo
    {
        private readonly List<object> _values = new List<object>();
        private int _valuePointer;
        
        internal string Name { get; set; }
        
        internal Parameter[] Parameters { get; set; }
        
        internal Exception Exception { get; set; }

        internal void AddValue(object value)
        {
            _values.Add(value);
        }

        internal object GetValue()
        {
            return _values.Count > 0 ? _values[_valuePointer++ % _values.Count] : null;
        }
    }
}
