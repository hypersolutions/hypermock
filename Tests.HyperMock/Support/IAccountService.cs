using System.Threading.Tasks;

namespace Tests.HyperMock.Support
{
    public interface IAccountService
    {
        Account this[string number] { get; }
        bool HasAccounts { get; set; }
        void Credit(string account, int amount);
        bool CanDebit(string account, int amount);
        void Debit(string account, int amount);
        Task<StatementInfo> GetStatementAsync(string account);
    }
}
