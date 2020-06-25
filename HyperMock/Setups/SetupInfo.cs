using System;
using System.Collections.Generic;
using HyperMock.Core;

namespace HyperMock.Setups
{
    internal sealed class SetupInfo
    {
        private readonly List<SetupValue> _values = new List<SetupValue>();
        private int _valuePointer;
        
        internal string Name { get; set; }
        
        internal Parameter[] Parameters { get; set; }
        
        internal void AddValue(object value)
        {
            _values.Add(new SetupValue(value));
        }

        internal void AddException(Exception value)
        {
            _values.Add(new SetupValue(value, true));
        }
        
        internal SetupValue GetValue()
        {
            return _values.Count > 0 ? _values[_valuePointer++ % _values.Count] : null;
        }
    }
}
