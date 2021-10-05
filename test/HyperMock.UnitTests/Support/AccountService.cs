namespace HyperMock.UnitTests.Support
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