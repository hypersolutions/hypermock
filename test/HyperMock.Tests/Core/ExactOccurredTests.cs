using HyperMock.Core;
using HyperMock.Exceptions;
using Shouldly;
using Xunit;

namespace HyperMock.Tests.Core
{
    public class ExactOccurredTests
    {
        [Fact]
        public void AssertThrowsExceptionForMismatch()
        {
            var occurred = new ExactOccurred(1);
            
            Should.Throw<VerificationException>(() => occurred.Assert(0));
        }

        [Fact]
        public void AssertSuccessForMatch()
        {
            var occurred = new ExactOccurred(1);

            Should.NotThrow(() => occurred.Assert(1));
        }
    }
}
