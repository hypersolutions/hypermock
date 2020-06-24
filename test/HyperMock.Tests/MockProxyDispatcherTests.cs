using System.Linq;
using HyperMock.Tests.Support;
using Shouldly;
using Xunit;

namespace HyperMock.Tests
{
    public class MockProxyDispatcherTests
    {
        [Fact]
        public void CallToMethodIsRecorded()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Object.Credit("12345678", 100);

            var visit = mock.Dispatcher.Visits.RecordedVisits.Last();
            visit.ShouldNotBeNull();
        }

        [Fact]
        public void CallToMethodIsRecordsName()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Object.Credit("12345678", 100);

            var visit = mock.Dispatcher.Visits.RecordedVisits.Last();
            visit.Name.ShouldBe("Credit");
        }

        [Fact]
        public void CallToMethodIsRecordsArgs()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Object.Credit("12345678", 100);

            var visit = mock.Dispatcher.Visits.RecordedVisits.Last();
            visit.Parameters.Length.ShouldBe(2);
            visit.Parameters[0].Value.ShouldBe("12345678");
            visit.Parameters[1].Value.ShouldBe(100);
        }

        [Fact]
        public void CallToFunctionIsRecorded()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Object.CanDebit("12345678", 100);

            var visit = mock.Dispatcher.Visits.RecordedVisits.Last();
            visit.ShouldNotBeNull();
        }

        [Fact]
        public void CallToFunctionIsRecordsName()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Object.CanDebit("12345678", 100);

            var visit = mock.Dispatcher.Visits.RecordedVisits.Last();
            visit.Name.ShouldBe("CanDebit");
        }

        [Fact]
        public void CallToFunctionIsRecordsArgs()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Object.CanDebit("12345678", 100);

            var visit = mock.Dispatcher.Visits.RecordedVisits.Last();
            visit.Parameters.Length.ShouldBe(2);
            visit.Parameters[0].Value.ShouldBe("12345678");
            visit.Parameters[1].Value.ShouldBe(100);
        }

        [Fact]
        public void CallToGetPropertyIsRecorded()
        {
            var mock = Mock.Create<IAccountService>();

            // ReSharper disable once UnusedVariable
            var result = mock.Object.HasAccounts;

            var visit = mock.Dispatcher.Visits.RecordedVisits.Last();
            visit.ShouldNotBeNull();
        }

        [Fact]
        public void CallToGetPropertyIsRecordsName()
        {
            var mock = Mock.Create<IAccountService>();

            // ReSharper disable once UnusedVariable
            var result = mock.Object.HasAccounts;

            var visit = mock.Dispatcher.Visits.RecordedVisits.Last();
            visit.Name.ShouldBe("get_HasAccounts");
        }

        [Fact]
        public void CallToSetPropertyIsRecorded()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Object.HasAccounts = true;

            var visit = mock.Dispatcher.Visits.RecordedVisits.Last();
            visit.ShouldNotBeNull();
        }

        [Fact]
        public void CallToSetPropertyIsRecordsName()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Object.HasAccounts = true;

            var visit = mock.Dispatcher.Visits.RecordedVisits.Last();
            visit.Name.ShouldBe("set_HasAccounts");
        }

        [Fact]
        public void CallToSetPropertyIsRecordsArgs()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Object.HasAccounts = true;

            var visit = mock.Dispatcher.Visits.RecordedVisits.Last();
            visit.Parameters.Length.ShouldBe(1);
            visit.Parameters[0].Value.ShouldBe(true);
        }
    }
}
