using System.Threading.Tasks;
using HyperMock;
using Tests.HyperMock.Support;
#if WINDOWS_UWP
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace Tests.HyperMock.Integration
{
    [TestClass]
    public class ResetOccurenceTests
    {
        private static Mock<IDataService> _mockDataService;
        private static Mock<ISettingsService> _mockSettingsService;
        private static AuthenticationService _authenticationService;

        [ClassInitialize]
        public static void Initialise(TestContext context)
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

        [TestMethod]
        public async Task AuthenticateUserReturnsValidTrueForKnownUser()
        {
            using (Mock.CallGroup(_mockDataService, _mockSettingsService))
            {
                var authenticatedUserResult = await _authenticationService.AuthenticateUser("TestUser", "TestPassword");

                _mockDataService.Verify(s => s.GetUserByUsernameAndPassword("TestUser", "TestPassword"),
                    Occurred.Once());
                _mockSettingsService.VerifySet(s => s.CurrentUser, Occurred.Once());

                Assert.IsTrue(authenticatedUserResult);
            }
        }

        [TestMethod]
        public async Task AuthenticateUserReturnsValidFalseForUnknownUser()
        {
            using (Mock.CallGroup(_mockDataService, _mockSettingsService))
            {
                var authenticatedUserResult = await _authenticationService.AuthenticateUser("Hacker", "LetMeIn");

                _mockDataService.Verify(s => s.GetUserByUsernameAndPassword("Hacker", "LetMeIn"), Occurred.Once());
                _mockSettingsService.VerifySet(s => s.CurrentUser, Occurred.Never());

                Assert.IsFalse(authenticatedUserResult);
            }
        }
    }

    
}
