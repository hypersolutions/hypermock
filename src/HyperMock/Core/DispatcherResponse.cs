using System;

namespace HyperMock.Core
{
    internal sealed class DispatcherResponse
    {
        internal object ReturnValue { get; set; }
        internal object[] ReturnArgs { get; set; }
        internal Exception Exception { get; set; }
    }
}