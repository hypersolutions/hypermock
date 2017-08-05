using System.Threading.Tasks;

namespace Tests.HyperMock.Support
{
    public interface IDataService
    {
        Task<DataServiceResponse<UserModel>> GetUserByUsernameAndPassword(string username, string password);
    }
}