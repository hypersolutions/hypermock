using HyperMock.Exceptions;

namespace HyperMock.Core
{
    internal class CountOrMoreOccurred : Occurred
    {
        internal CountOrMoreOccurred(int count) : base(count)
        {

        }

        public override void Assert(int actualCount)
        {
            if (actualCount < Count)
                throw new VerificationException($"Verification mismatch: Expected at least {Count}; Actual {actualCount}");
        }
    }
}