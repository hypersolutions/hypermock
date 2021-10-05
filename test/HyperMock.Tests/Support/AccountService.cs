using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyperMock.Tests.Support
{
    public class AccountService : IAccountService
    {
        public bool HasAccounts { get; set; }

        public void Credit(string account, int amount)
        {
            
        }

        public bool CanDebit(string account, int amount)
        {
            return false;
        }
    }
}