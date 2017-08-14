using System;
using System.Linq;
using HyperMock;
using HyperMock.Behaviors;
using Tests.HyperMock.Support;
using Xunit;

namespace Tests.HyperMock
{
    public class MockTests
    {
        [Fact]
        public void CreateGenericThrowsExceptionForAbstractClass()
        {
            Assert.Throws<NotSupportedException>(() => Mock.Create<NonSupportedAbstractClass>());
        }

        [Fact]
        public void CreateGenericThrowsExceptionForConcreteClass()
        {
            Assert.Throws<NotSupportedException>(() => Mock.Create<AccountService>());
        }

        [Fact]
        public void CreateTypeThrowsExceptionForAbstractClass()
        {
            Assert.Throws<NotSupportedException>(() => Mock.Create(typeof(NonSupportedAbstractClass)));
        }

        [Fact]
        public void CreateTypeThrowsExceptionForConcreteClass()
        {
            Assert.Throws<NotSupportedException>(() => Mock.Create(typeof(AccountService)));
        }

        [Fact]
        public void CreateGenericReturnsMockInstance()
        {
            var mock = Mock.Create<IAccountService>();

            Assert.NotNull(mock);
        }

        [Fact]
        public void CreateGenericReturnsMockObjectInstance()
        {
            var mock = Mock.Create<IAccountService>();

            Assert.NotNull(mock.Object);
        }

        [Fact]
        public void CreateGenericReturnsMockDispatcherInstance()
        {
            var mock = Mock.Create<IAccountService>();

            Assert.NotNull(mock.Dispatcher);
        }
        
        [Fact]
        public void CreateTypeReturnsMockInstance()
        {
            var mock = Mock.Create(typeof(IAccountService));

            Assert.NotNull(mock);
        }

        [Fact]
        public void CreateTypeReturnsMockObjectInstance()
        {
            var mock = Mock.Create(typeof(IAccountService));

            Assert.NotNull(mock.Object);
        }

        [Fact]
        public void CreateTypeReturnsMockDispatcherInstance()
        {
            var mock = Mock.Create(typeof(IAccountService));

            Assert.NotNull(mock.Dispatcher);
        }

        [Fact]
        public void SetupAddsMethodBehaviourInfoToDispatcher()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Setup(s => s.Credit("12345678", 100));
            
            Assert.True(mock.Dispatcher.Setups.InfoList.Any());
        }

        [Fact]
        public void SetupReturnsMethodCall()
        {
            var mock = Mock.Create<IAccountService>();

            var result = mock.Setup(s => s.Credit("12345678", 100));

            Assert.IsType<MethodCall>(result);
        }

        [Fact]
        public void SetupAddsFunctionBehaviourInfoToDispatcher()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Setup(s => s.CanDebit("12345678", 100));

            Assert.True(mock.Dispatcher.Setups.InfoList.Any());
        }

        [Fact]
        public void SetupReturnsFunctionCall()
        {
            var mock = Mock.Create<IAccountService>();

            var result = mock.Setup(s => s.CanDebit("12345678", 100));

            Assert.IsType<FunctionCall<bool>>(result);
        }

        [Fact]
        public void SetupGetAddsPropertyBehaviourInfoToDispatcher()
        {
            var mock = Mock.Create<IAccountService>();

            mock.SetupGet(s => s.HasAccounts);

            Assert.True(mock.Dispatcher.Setups.InfoList.Any());
        }

        [Fact]
        public void SetupGetReturnsFunctionCall()
        {
            var mock = Mock.Create<IAccountService>();

            var result = mock.SetupGet(s => s.HasAccounts);

            Assert.IsType<GetPropertyCall<bool>>(result);
        }

        [Fact]
        public void SetupSetAddsPropertyBehaviourInfoToDispatcher()
        {
            var mock = Mock.Create<IAccountService>();

            mock.SetupSet(s => s.HasAccounts);

            Assert.True(mock.Dispatcher.Setups.InfoList.Any());
        }

        [Fact]
        public void SetupSetReturnsMethodCall()
        {
            var mock = Mock.Create<IAccountService>();

            var result = mock.SetupSet(s => s.HasAccounts);

            Assert.IsType<SetPropertyCall<bool>>(result);
        }
    }
}
