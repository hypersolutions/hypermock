using System.Threading.Tasks;

namespace Tests.HyperMock.Support
{
    public class AccountController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public void Credit(AccountInfo info)
        {
            _accountService.Credit(info.Number, info.CreditAmount);
        }

        public void Debit(AccountInfo info)
        {
            if (_accountService.CanDebit(info.Number, info.DebitAmount))
                _accountService.Debit(info.Number, info.DebitAmount);
        }

        public bool HasAccounts()
        {
            return _accountService.HasAccounts;
        }

        public void Manage(bool hasAccounts)
        {
            _accountService.HasAccounts = hasAccounts;
        }

        public async Task<StatementInfo> GetStatementAsync(AccountInfo info)
        {
            return await _accountService.GetStatementAsync(info.Number);
        }

        public AccountInfo GetAccount(string number)
        {
            var account = _accountService[number];
            return new AccountInfo {Name = account.Name, Number = account.Number};
        }
    }
}
