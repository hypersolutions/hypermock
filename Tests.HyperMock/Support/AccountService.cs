using System.Threading.Tasks;

namespace Tests.HyperMock.Support
{
    public class AccountService : IAccountService
    {
        public bool HasAccounts { get; set; }

        public Account this[string number] => null;

        public void Credit(string account, int amount)
        {
            
        }

        public bool CanDebit(string account, int amount)
        {
            return false;
        }

        public void Debit(string account, int amount)
        {
        }

        public async Task<StatementInfo> GetStatementAsync(string account)
        {
            await Task.Delay(10);
            return new StatementInfo();
        }
    }
}