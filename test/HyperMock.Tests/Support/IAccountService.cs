using System.Threading.Tasks;

namespace HyperMock.Tests.Support
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
