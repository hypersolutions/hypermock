using HyperMock.Core;
using HyperMock.Exceptions;
using Shouldly;
using Xunit;

namespace HyperMock.Tests.Core
{
    public class CountOrMoreOccurredTests
    {
        [Fact]
        public void AssertThrowsExceptionForMismatch()
        {
            var occurred = new CountOrMoreOccurred(1);
            
            Should.Throw<VerificationException>(() => occurred.Assert(0));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void AssertSuccessForMatchOrGreater(int actualCount)
        {
            var occurred = new CountOrMoreOccurred(1);

            Should.NotThrow(() => occurred.Assert(actualCount));
        }
    }
}
