using System.Threading.Tasks;

namespace HyperMock.IntTests.Support
{
    public interface IDataService
    {
        Task<DataServiceResponse<UserModel>> GetUserByUsernameAndPassword(string username, string password);
    }
}