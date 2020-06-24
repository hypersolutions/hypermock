using System;

namespace HyperMock.Tests.Support
{
    public class TempChangedEventArgs : EventArgs
    {
        public TempChangedEventArgs(int temp)
        {
            Value = temp;
        }

        public int Value { get; }
    }
}