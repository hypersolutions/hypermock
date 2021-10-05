using System;

namespace HyperMock.IntTests.Support
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