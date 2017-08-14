using System.Threading.Tasks;
using HyperMock;
using Tests.HyperMock.Support;
using Xunit;

namespace Tests.HyperMock.Integration
{
    public class ResetOccurenceTests
    {
        private static readonly Mock<IDataService> _mockDataService;
        private static readonly Mock<ISettingsService> _mockSettingsService;
        private static readonly AuthenticationService _authenticationService;

        static ResetOccurenceTests()
        {
            // Create and setup mock data service
            _mockDataService = Mock.Create<IDataService>();

            _mockDataService
                .Setup(s => s.GetUserByUsernameAndPassword("TestUser", "TestPassword"))
                .Returns(Task.Run(() =>
                    new DataServiceResponse<UserModel>
                    {
                        ResponseType = ResponseTypes.Success,
                        Result = new UserModel(),
                    }));

            // Setup mock for an invalid user
            _mockDataService
                .Setup(s => s.GetUserByUsernameAndPassword(Param.IsAny<string>(), Param.IsAny<string>()))
                .Returns(Task.Run(() =>
                    new DataServiceResponse<UserModel>
                    {
                        ResponseType = ResponseTypes.HttpError,
                        Result = null,
                        Error = new ErrorModel(),
                    }));

            // Create mock settings service
            _mockSettingsService = Mock.Create<ISettingsService>();

            _authenticationService = new AuthenticationService(_mockSettingsService.Object, _mockDataService.Object);
        }

        [Fact]
        public async Task AuthenticateUserResetsOccurences()
        {
            using (Mock.CallGroup(_mockDataService, _mockSettingsService))
            {
                var authenticatedUserResult = await _authenticationService.AuthenticateUser("TestUser", "TestPassword");

                _mockDataService.Verify(s => s.GetUserByUsernameAndPassword("TestUser", "TestPassword"),
                    Occurred.Once());
                _mockSettingsService.VerifySet(s => s.CurrentUser, Occurred.Once());

                Assert.True(authenticatedUserResult);
            }

            using (Mock.CallGroup(_mockDataService, _mockSettingsService))
            {
                var authenticatedUserResult = await _authenticationService.AuthenticateUser("Hacker", "LetMeIn");

                _mockDataService.Verify(s => s.GetUserByUsernameAndPassword("Hacker", "LetMeIn"), Occurred.Once());
                _mockSettingsService.VerifySet(s => s.CurrentUser, Occurred.Never());

                Assert.False(authenticatedUserResult);
            }
        }
    }
}
