using HyperMock.Core;
using HyperMock.Exceptions;
#if WINDOWS_UWP
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace Tests.HyperMock.Core
{
    [TestClass]
    public class ExactOccurredTests
    {
#if WINDOWS_UWP
        [TestMethod]
        public void AssertThrowsExceptionForMismatch()
        {
            var occurred = new ExactOccurred(1);
            
            Assert.ThrowsException<VerificationException>(() => occurred.Assert(0));
        }
#else
        [TestMethod, ExpectedException(typeof(VerificationException))]
        public void AssertThrowsExceptionForMismatch()
        {
            var occurred = new ExactOccurred(1);

            occurred.Assert(0);
        }
#endif

        [TestMethod]
        public void AssertSuccessForMatch()
        {
            var occurred = new ExactOccurred(1);

            try
            {
                occurred.Assert(1);
            }
            catch (VerificationException verifyError)
            {
                Assert.Fail($"Successful verification threw a VerificationException: {verifyError}");
            }
        }
    }
}