using System;
using System.Linq;
using HyperMock.Behaviors;
using HyperMock.UnitTests.Support;
using Shouldly;
using Xunit;

namespace HyperMock.UnitTests
{
    public class MockTests
    {
        [Fact]
        public void CreateGenericThrowsExceptionForAbstractClass()
        {
            Should.Throw<NotSupportedException>(() => Mock.Create<NonSupportedAbstractClass>());
        }

        [Fact]
        public void CreateGenericThrowsExceptionForConcreteClass()
        {
            Should.Throw<NotSupportedException>(() => Mock.Create<AccountService>());
        }

        [Fact]
        public void CreateTypeThrowsExceptionForAbstractClass()
        {
            Should.Throw<NotSupportedException>(() => Mock.Create(typeof(NonSupportedAbstractClass)));
        }

        [Fact]
        public void CreateTypeThrowsExceptionForConcreteClass()
        {
            Should.Throw<NotSupportedException>(() => Mock.Create(typeof(AccountService)));
        }

        [Fact]
        public void CreateGenericReturnsMockInstance()
        {
            var mock = Mock.Create<IAccountService>();

            mock.ShouldNotBeNull();
        }

        [Fact]
        public void CreateGenericReturnsMockObjectInstance()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Object.ShouldNotBeNull();
        }

        [Fact]
        public void CreateGenericReturnsMockDispatcherInstance()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Dispatcher.ShouldNotBeNull();
        }
        
        [Fact]
        public void CreateTypeReturnsMockInstance()
        {
            var mock = Mock.Create(typeof(IAccountService));

            mock.ShouldNotBeNull();
        }

        [Fact]
        public void CreateTypeReturnsMockObjectInstance()
        {
            var mock = Mock.Create(typeof(IAccountService));

            mock.Object.ShouldNotBeNull();
        }

        [Fact]
        public void CreateTypeReturnsMockDispatcherInstance()
        {
            var mock = Mock.Create(typeof(IAccountService));

            mock.Dispatcher.ShouldNotBeNull();
        }

        [Fact]
        public void SetupAddsMethodBehaviourInfoToDispatcher()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Setup(s => s.Credit("12345678", 100));
            
            mock.Dispatcher.Setups.InfoList.Any().ShouldBeTrue();
        }

        [Fact]
        public void SetupReturnsMethodCall()
        {
            var mock = Mock.Create<IAccountService>();

            var result = mock.Setup(s => s.Credit("12345678", 100));

            result.ShouldBeOfType<MethodCall>();
        }

        [Fact]
        public void SetupAddsFunctionBehaviourInfoToDispatcher()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Setup(s => s.CanDebit("12345678", 100));

            mock.Dispatcher.Setups.InfoList.Any().ShouldBeTrue();
        }

        [Fact]
        public void SetupReturnsFunctionCall()
        {
            var mock = Mock.Create<IAccountService>();

            var result = mock.Setup(s => s.CanDebit("12345678", 100));

            result.ShouldBeOfType<FunctionCall<bool>>();
        }

        [Fact]
        public void SetupGetAddsPropertyBehaviourInfoToDispatcher()
        {
            var mock = Mock.Create<IAccountService>();

            mock.SetupGet(s => s.HasAccounts);

            mock.Dispatcher.Setups.InfoList.Any().ShouldBeTrue();
        }

        [Fact]
        public void SetupGetReturnsFunctionCall()
        {
            var mock = Mock.Create<IAccountService>();

            var result = mock.SetupGet(s => s.HasAccounts);

            result.ShouldBeOfType<GetPropertyCall<bool>>();
        }

        [Fact]
        public void SetupSetAddsPropertyBehaviourInfoToDispatcher()
        {
            var mock = Mock.Create<IAccountService>();

            mock.SetupSet(s => s.HasAccounts);

            mock.Dispatcher.Setups.InfoList.Any().ShouldBeTrue();
        }

        [Fact]
        public void SetupSetReturnsMethodCall()
        {
            var mock = Mock.Create<IAccountService>();

            var result = mock.SetupSet(s => s.HasAccounts);

            result.ShouldBeOfType<SetPropertyCall<bool>>();
        }
    }
}
