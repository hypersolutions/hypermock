namespace HyperMock.Universal.Examples
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
    }
}