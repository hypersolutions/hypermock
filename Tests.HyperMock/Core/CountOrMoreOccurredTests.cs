using HyperMock.Core;
using HyperMock.Exceptions;
using Xunit;

namespace Tests.HyperMock.Core
{
    public class CountOrMoreOccurredTests
    {
        [Fact]
        public void AssertThrowsExceptionForMismatch()
        {
            var occurred = new CountOrMoreOccurred(1);
            
            Assert.Throws<VerificationException>(() => occurred.Assert(0));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void AssertSuccessForMatchOrGreater(int actualCount)
        {
            var occurred = new CountOrMoreOccurred(1);

            occurred.Assert(actualCount);
        }
    }
}
