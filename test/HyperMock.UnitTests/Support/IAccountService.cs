

// ReSharper disable UnusedParameter.Global

namespace HyperMock.UnitTests.Support
{
    public interface IAccountService
    {
        bool HasAccounts { get; set; }
        void Credit(string account, int amount);
        bool CanDebit(string account, int amount);
    }
}
