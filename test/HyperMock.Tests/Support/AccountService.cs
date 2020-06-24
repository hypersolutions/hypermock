using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyperMock.Tests.Support
{
    public class AccountService : IAccountService
    {
        // ReSharper disable once CollectionNeverUpdated.Local
        private readonly List<Account> _accounts = new List<Account>();

        public bool HasAccounts { get; set; }

        public Account this[string number]
        {
            get
            {
                return _accounts.FirstOrDefault(a => a.Number == number);
            }
            set
            {
                var account = _accounts.FirstOrDefault(a => a.Number == number);

                if (account != null)
                {
                    account.Name = value.Name;
                }
            }
        }

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