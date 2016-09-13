using HyperMock.Core;
using HyperMock.Setups;

namespace HyperMock
{
    /// <summary>
    /// Defines the members of the IMockProxyDispatcher interface.
    /// </summary>
    public interface IMockProxyDispatcher
    {
        VisitList Visits { get; }
        SetupInfoList Setups { get; }
    }
}