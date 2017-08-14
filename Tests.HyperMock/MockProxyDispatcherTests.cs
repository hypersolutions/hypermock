using System.Linq;
using HyperMock;
using Tests.HyperMock.Support;
using Xunit;

namespace Tests.HyperMock
{
    public class MockProxyDispatcherTests
    {
        [Fact]
        public void CallToMethodIsRecorded()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Object.Credit("12345678", 100);

            var visit = mock.Dispatcher.Visits.RecordedVisits.Last();
            Assert.NotNull(visit);
        }

        [Fact]
        public void CallToMethodIsRecordsName()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Object.Credit("12345678", 100);

            var visit = mock.Dispatcher.Visits.RecordedVisits.Last();
            Assert.Equal("Credit", visit.Name);
        }

        [Fact]
        public void CallToMethodIsRecordsArgs()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Object.Credit("12345678", 100);

            var visit = mock.Dispatcher.Visits.RecordedVisits.Last();
            Assert.Equal(2, visit.Parameters.Length);
            Assert.Equal("12345678", visit.Parameters[0].Value);
            Assert.Equal(100, visit.Parameters[1].Value);
        }

        [Fact]
        public void CallToFunctionIsRecorded()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Object.CanDebit("12345678", 100);

            var visit = mock.Dispatcher.Visits.RecordedVisits.Last();
            Assert.NotNull(visit);
        }

        [Fact]
        public void CallToFunctionIsRecordsName()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Object.CanDebit("12345678", 100);

            var visit = mock.Dispatcher.Visits.RecordedVisits.Last();
            Assert.Equal("CanDebit", visit.Name);
        }

        [Fact]
        public void CallToFunctionIsRecordsArgs()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Object.CanDebit("12345678", 100);

            var visit = mock.Dispatcher.Visits.RecordedVisits.Last();
            Assert.Equal(2, visit.Parameters.Length);
            Assert.Equal("12345678", visit.Parameters[0].Value);
            Assert.Equal(100, visit.Parameters[1].Value);
        }

        [Fact]
        public void CallToGetPropertyIsRecorded()
        {
            var mock = Mock.Create<IAccountService>();

            // ReSharper disable once UnusedVariable
            var result = mock.Object.HasAccounts;

            var visit = mock.Dispatcher.Visits.RecordedVisits.Last();
            Assert.NotNull(visit);
        }

        [Fact]
        public void CallToGetPropertyIsRecordsName()
        {
            var mock = Mock.Create<IAccountService>();

            // ReSharper disable once UnusedVariable
            var result = mock.Object.HasAccounts;

            var visit = mock.Dispatcher.Visits.RecordedVisits.Last();
            Assert.Equal("get_HasAccounts", visit.Name);
        }

        [Fact]
        public void CallToSetPropertyIsRecorded()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Object.HasAccounts = true;

            var visit = mock.Dispatcher.Visits.RecordedVisits.Last();
            Assert.NotNull(visit);
        }

        [Fact]
        public void CallToSetPropertyIsRecordsName()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Object.HasAccounts = true;

            var visit = mock.Dispatcher.Visits.RecordedVisits.Last();
            Assert.Equal("set_HasAccounts", visit.Name);
        }

        [Fact]
        public void CallToSetPropertyIsRecordsArgs()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Object.HasAccounts = true;

            var visit = mock.Dispatcher.Visits.RecordedVisits.Last();
            Assert.Equal(1, visit.Parameters.Length);
            Assert.Equal(true, visit.Parameters[0].Value);
        }
    }
}
