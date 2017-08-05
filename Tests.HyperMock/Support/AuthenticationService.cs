using System.Threading.Tasks;

namespace Tests.HyperMock.Support
{
    public class AuthenticationService
    {
        private readonly ISettingsService _settingsService;
        private readonly IDataService _dataService;

        public AuthenticationService(ISettingsService settingsService, IDataService dataService)
        {
            _settingsService = settingsService;
            _dataService = dataService;
        }

        public async Task<bool> AuthenticateUser(string username, string password)
        {
            var response = await _dataService.GetUserByUsernameAndPassword(username, password);

            if (response != null && response.ResponseType == ResponseTypes.Success)
            {
                _settingsService.CurrentUser = response.Result;
                return true;
            }

            return false;
        }
    }
}