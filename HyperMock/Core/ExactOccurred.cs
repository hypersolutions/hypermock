using HyperMock.Exceptions;

namespace HyperMock.Core
{
    internal class ExactOccurred : Occurred
    {
        internal ExactOccurred(int count) : base(count)
        {

        }

        public override void Assert(int actualCount)
        {
            if (Count != actualCount)
                throw new VerificationException($"Verification mismatch: Expected {Count}; Actual {actualCount}");
        }
    }
}