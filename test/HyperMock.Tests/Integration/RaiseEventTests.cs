using System;
using HyperMock.Tests.Support;
using Xunit;

namespace HyperMock.Tests.Integration
{
    public class RaiseEventTests : TestBase<ThermostatController>
    {
        [Fact]
        public void RaiseInvokesAddHandler()
        {
            MockFor<IThermostatService>().Raise(s => s.Hot += null, new EventArgs());

            MockFor<IThermostatService>().Verify(p => p.SwitchOff(), Occurred.Once());
        }
        
        [Fact]
        public void RaiseDoesNotInvokeAddHandler()
        {
            MockFor<IThermostatService>().Raise(s => s.Cold += null, new EventArgs());

            MockFor<IThermostatService>().Verify(p => p.SwitchOn(), Occurred.Never());
        }

        [Fact]
        public void RaiseInvokesAddHandlerWithParams()
        {
            const int temp = 100;

            MockFor<IThermostatService>().Raise(s => s.TempChanged += null, new TempChangedEventArgs(temp));

            MockFor<IThermostatService>().Verify(p => p.ChangeTemp(temp), Occurred.Once());
        }
    }
}