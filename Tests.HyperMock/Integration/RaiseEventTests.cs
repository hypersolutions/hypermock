using System;
using HyperMock;
#if WINDOWS_UWP
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif
using Tests.HyperMock.Support;

namespace Tests.HyperMock.Integration
{
    [TestClass]
    public class RaiseEventTests : TestBase<ThermostatController>
    {
        [TestMethod]
        public void RaiseInvokesAddHandler()
        {
            MockFor<IThermostatService>().Raise(s => s.Hot += null, new EventArgs());

            MockFor<IThermostatService>().Verify(p => p.SwitchOff(), Occurred.Once());
        }
        
        [TestMethod]
        public void RaiseDoesNotInvokeAddHandler()
        {
            MockFor<IThermostatService>().Raise(s => s.Cold += null, new EventArgs());

            MockFor<IThermostatService>().Verify(p => p.SwitchOn(), Occurred.Never());
        }

        [TestMethod]
        public void RaiseInvokesAddHandlerWithParams()
        {
            const int temp = 100;

            MockFor<IThermostatService>().Raise(s => s.TempChanged += null, new TempChangedEventArgs(temp));

            MockFor<IThermostatService>().Verify(p => p.ChangeTemp(temp), Occurred.Once());
        }
    }
}