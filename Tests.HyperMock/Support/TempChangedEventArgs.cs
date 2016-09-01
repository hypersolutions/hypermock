using System;

namespace Tests.HyperMock.Support
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