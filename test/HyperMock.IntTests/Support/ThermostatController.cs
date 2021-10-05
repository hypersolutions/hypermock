using System;

namespace HyperMock.IntTests.Support
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ThermostatController
    {
        private readonly IThermostatService _thermostatService;

        public ThermostatController(IThermostatService thermostatService)
        {
            _thermostatService = thermostatService;
            _thermostatService.Hot += OnSwitchOff;
            _thermostatService.Cold -= OnSwitchOn;
            _thermostatService.TempChanged += OnTempChanged;
        }

        private void OnSwitchOff(object sender, EventArgs args)
        {
            _thermostatService.SwitchOff();
        }

        private void OnSwitchOn(object sender, EventArgs args)
        {
            _thermostatService.SwitchOn();
        }

        private void OnTempChanged(object sender, TempChangedEventArgs args)
        {
            _thermostatService.ChangeTemp(args.Value);
        }
    }
}