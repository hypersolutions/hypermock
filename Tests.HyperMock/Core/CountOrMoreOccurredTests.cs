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
    public class CountOrMoreOccurredTests
    {
#if WINDOWS_UWP
        [TestMethod]
        public void AssertThrowsExceptionForMismatch()
        {
            var occurred = new CountOrMoreOccurred(1);
            
            Assert.ThrowsException<VerificationException>(() => occurred.Assert(0));
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        public void AssertSuccessForMatchOrGreater(int actualCount)
        {
            var occurred = new CountOrMoreOccurred(1);

            try
            {
                occurred.Assert(actualCount);
            }
            catch (VerificationException verifyError)
            {
                Assert.Fail($"Successful verification threw a VerificationException: {verifyError}");
            }
        }

#else
        [TestMethod, ExpectedException(typeof(VerificationException))]
        public void AssertThrowsExceptionForMismatch()
        {
            var occurred = new CountOrMoreOccurred(1);

            occurred.Assert(0);
        }

        [TestMethod]
        public void AssertSuccessForMatchOrGreater()
        {
            var data = new[] {1, 2};

            foreach (var actualCount in data)
            {
                var occurred = new CountOrMoreOccurred(1);

                try
                {
                    occurred.Assert(actualCount);
                }
                catch (VerificationException verifyError)
                {
                    Assert.Fail($"Successful verification threw a VerificationException: {verifyError}");
                }
            }
        }
#endif
    }
}
