using System.Threading.Tasks;
// ReSharper disable UnusedParameter.Global

namespace HyperMock.Tests.Support
{
    public interface IAccountService
    {
        bool HasAccounts { get; set; }
        void Credit(string account, int amount);
        bool CanDebit(string account, int amount);
    }
}
