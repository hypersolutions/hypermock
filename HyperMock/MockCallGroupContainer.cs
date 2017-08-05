using System;
using System.Linq;

namespace HyperMock
{
    /// <summary>
    /// Provides a disposable wrapper to a list of Mocks which are grouped for resetting visit metrics. Use inside a using block.
    /// </summary>
    public sealed class MockCallGroupContainer : IDisposable
    {
        private readonly IMockProxyDispatcher[] _dispatchers;
        private bool _disposed;

        internal MockCallGroupContainer(params Mock[] mocks)
        {
            if (mocks != null && mocks.Any())
                _dispatchers = mocks.Select(m => m?.Dispatcher).ToArray();
        }

        ~MockCallGroupContainer()
        {
            Dispose(false);
        }

        /// <summary>
        /// Calls a reset of visit metrics on each mock.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _dispatchers?.ToList().ForEach(d => d.Visits.Reset());
            }

            _disposed = true;
        }
    }
}