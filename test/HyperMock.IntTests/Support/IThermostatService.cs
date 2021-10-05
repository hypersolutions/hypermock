using System;

namespace HyperMock.IntTests.Support
{
    public interface IThermostatService
    {
        event EventHandler Hot;
        event EventHandler Cold;
        event EventHandler<TempChangedEventArgs> TempChanged;

        void SwitchOn();
        void SwitchOff();
        void ChangeTemp(int temp);
    }
}