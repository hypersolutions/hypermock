using System;
using HyperMock.Core;

namespace HyperMock.Setups
{
    internal sealed class SetupInfo
    {
        internal string Name { get; set; }
        internal Parameter[] Parameters { get; set; }
        internal object Value { get; set; }
        internal Exception Exception { get; set; }
    }
}