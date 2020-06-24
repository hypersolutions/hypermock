using System.Threading.Tasks;

namespace HyperMock.Tests.Support
{
    public interface IDataService
    {
        Task<DataServiceResponse<UserModel>> GetUserByUsernameAndPassword(string username, string password);
    }
}