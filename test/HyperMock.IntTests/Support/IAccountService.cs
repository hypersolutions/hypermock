using System.Threading.Tasks;

// ReSharper disable UnusedParameter.Global

namespace HyperMock.IntTests.Support
{
    public interface IAccountService
    {
        Account this[string number] { get; set; }
        bool HasAccounts { get; set; }
        void Credit(string account, int amount);
        bool CanDebit(string account, int amount);
        void Debit(string account, int amount);
        Task<StatementInfo> GetStatementAsync(string account);
    }
}
