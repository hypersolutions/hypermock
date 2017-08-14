using HyperMock.Core;
using HyperMock.Exceptions;
using Xunit;

namespace Tests.HyperMock.Core
{
    public class ExactOccurredTests
    {
        [Fact]
        public void AssertThrowsExceptionForMismatch()
        {
            var occurred = new ExactOccurred(1);
            
            Assert.Throws<VerificationException>(() => occurred.Assert(0));
        }

        [Fact]
        public void AssertSuccessForMatch()
        {
            var occurred = new ExactOccurred(1);

            occurred.Assert(1);
        }
    }
}