using System.Linq;
using HyperMock;
#if WINDOWS_UWP
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif
using Tests.HyperMock.Support;

namespace Tests.HyperMock
{
    [TestClass]
    public class MockProxyDispatcherTests
    {
        [TestMethod]
        public void CallToMethodIsRecorded()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Object.Credit("12345678", 100);

            var visit = mock.Dispatcher.Visits.RecordedVisits.Last();
            Assert.IsNotNull(visit);
        }

        [TestMethod]
        public void CallToMethodIsRecordsName()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Object.Credit("12345678", 100);

            var visit = mock.Dispatcher.Visits.RecordedVisits.Last();
            Assert.AreEqual("Credit", visit.Name);
        }

        [TestMethod]
        public void CallToMethodIsRecordsArgs()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Object.Credit("12345678", 100);

            var visit = mock.Dispatcher.Visits.RecordedVisits.Last();
            Assert.AreEqual(2, visit.Args.Length);
            Assert.AreEqual("12345678", visit.Args[0]);
            Assert.AreEqual(100, visit.Args[1]);
        }

        [TestMethod]
        public void CallToFunctionIsRecorded()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Object.CanDebit("12345678", 100);

            var visit = mock.Dispatcher.Visits.RecordedVisits.Last();
            Assert.IsNotNull(visit);
        }

        [TestMethod]
        public void CallToFunctionIsRecordsName()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Object.CanDebit("12345678", 100);

            var visit = mock.Dispatcher.Visits.RecordedVisits.Last();
            Assert.AreEqual("CanDebit", visit.Name);
        }

        [TestMethod]
        public void CallToFunctionIsRecordsArgs()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Object.CanDebit("12345678", 100);

            var visit = mock.Dispatcher.Visits.RecordedVisits.Last();
            Assert.AreEqual(2, visit.Args.Length);
            Assert.AreEqual("12345678", visit.Args[0]);
            Assert.AreEqual(100, visit.Args[1]);
        }

        [TestMethod]
        public void CallToGetPropertyIsRecorded()
        {
            var mock = Mock.Create<IAccountService>();

            // ReSharper disable once UnusedVariable
            var result = mock.Object.HasAccounts;

            var visit = mock.Dispatcher.Visits.RecordedVisits.Last();
            Assert.IsNotNull(visit);
        }

        [TestMethod]
        public void CallToGetPropertyIsRecordsName()
        {
            var mock = Mock.Create<IAccountService>();

            // ReSharper disable once UnusedVariable
            var result = mock.Object.HasAccounts;

            var visit = mock.Dispatcher.Visits.RecordedVisits.Last();
            Assert.AreEqual("get_HasAccounts", visit.Name);
        }

        [TestMethod]
        public void CallToSetPropertyIsRecorded()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Object.HasAccounts = true;

            var visit = mock.Dispatcher.Visits.RecordedVisits.Last();
            Assert.IsNotNull(visit);
        }

        [TestMethod]
        public void CallToSetPropertyIsRecordsName()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Object.HasAccounts = true;

            var visit = mock.Dispatcher.Visits.RecordedVisits.Last();
            Assert.AreEqual("set_HasAccounts", visit.Name);
        }

        [TestMethod]
        public void CallToSetPropertyIsRecordsArgs()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Object.HasAccounts = true;

            var visit = mock.Dispatcher.Visits.RecordedVisits.Last();
            Assert.AreEqual(1, visit.Args.Length);
            Assert.AreEqual(true, visit.Args[0]);
        }
    }
}
