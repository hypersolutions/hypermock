using System.Threading.Tasks;
using HyperMock;
using Tests.HyperMock.Support;
using Xunit;

namespace Tests.HyperMock.Integration
{
    public class ResetOccurenceTests : IClassFixture<ResetOccurenceTests.AuthenticationServiceBuilder>
    {
        private readonly Mock<IDataService> _mockDataService;
        private readonly Mock<ISettingsService> _mockSettingsService;
        private readonly AuthenticationService _authenticationService;

        public ResetOccurenceTests(ResetOccurenceTests.AuthenticationServiceBuilder builder)
        {
            _authenticationService = builder.AuthenticationService;
            _mockSettingsService = builder.MockSettingsService;
            _mockDataService = builder.MockDataService;
        }
        
        [Fact]
        public async Task AuthenticateUserReturnsValidTrueForKnownUser()
        {
            using (Mock.CallGroup(_mockDataService, _mockSettingsService))
            {
                var authenticatedUserResult = await _authenticationService.AuthenticateUser("TestUser", "TestPassword");

                _mockDataService.Verify(s => s.GetUserByUsernameAndPassword("TestUser", "TestPassword"),
                    Occurred.Once());
                _mockSettingsService.VerifySet(s => s.CurrentUser, Occurred.Once());

                Assert.True(authenticatedUserResult);
            }
        }

        [Fact]
        public async Task AuthenticateUserReturnsValidFalseForUnknownUser()
        {
            using (Mock.CallGroup(_mockDataService, _mockSettingsService))
            {
                var authenticatedUserResult = await _authenticationService.AuthenticateUser("Hacker", "LetMeIn");

                _mockDataService.Verify(s => s.GetUserByUsernameAndPassword("Hacker", "LetMeIn"), Occurred.Once());
                _mockSettingsService.VerifySet(s => s.CurrentUser, Occurred.Never());

                Assert.False(authenticatedUserResult);
            }
        }

        public sealed class AuthenticationServiceBuilder
        {
            public readonly Mock<IDataService> MockDataService;
            public readonly Mock<ISettingsService> MockSettingsService;
            public readonly AuthenticationService AuthenticationService;

            public AuthenticationServiceBuilder()
            {
                // Create and setup mock data service
                MockDataService = Mock.Create<IDataService>();

                MockDataService
                    .Setup(s => s.GetUserByUsernameAndPassword("TestUser", "TestPassword"))
                    .Returns(Task.Run(() =>
                        new DataServiceResponse<UserModel>
                        {
                            ResponseType = ResponseTypes.Success,
                            Result = new UserModel(),
                        }));

                // Setup mock for an invalid user
                MockDataService
                    .Setup(s => s.GetUserByUsernameAndPassword(Param.IsAny<string>(), Param.IsAny<string>()))
                    .Returns(Task.Run(() =>
                        new DataServiceResponse<UserModel>
                        {
                            ResponseType = ResponseTypes.HttpError,
                            Result = null,
                            Error = new ErrorModel(),
                        }));

                // Create mock settings service
                MockSettingsService = Mock.Create<ISettingsService>();

                AuthenticationService = new AuthenticationService(MockSettingsService.Object, MockDataService.Object);
            }
        }
    }
}
