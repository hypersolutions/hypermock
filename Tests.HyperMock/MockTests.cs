using System;
using System.Linq;
using HyperMock;
using HyperMock.Behaviors;
#if WINDOWS_UWP
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif
using Tests.HyperMock.Support;

namespace Tests.HyperMock
{
    [TestClass]
    public class MockTests
    {
#if WINDOWS_UWP
        [TestMethod]
        public void CreateGenericThrowsExceptionForAbstractClass()
        {
            Assert.ThrowsException<NotSupportedException>(Mock.Create<NonSupportedAbstractClass>);
        }

        [TestMethod]
        public void CreateGenericThrowsExceptionForConcreteClass()
        {
            Assert.ThrowsException<NotSupportedException>(Mock.Create<AccountService>);
        }

        [TestMethod]
        public void CreateTypeThrowsExceptionForAbstractClass()
        {
            Assert.ThrowsException<NotSupportedException>(() => Mock.Create(typeof(NonSupportedAbstractClass)));
        }

        [TestMethod]
        public void CreateTypeThrowsExceptionForConcreteClass()
        {
            Assert.ThrowsException<NotSupportedException>(() => Mock.Create(typeof(AccountService)));
        }
#else
        [TestMethod, ExpectedException(typeof(NotSupportedException))]
        public void CreateGenericThrowsExceptionForAbstractClass()
        {
            Mock.Create<NonSupportedAbstractClass>();
        }

        [TestMethod, ExpectedException(typeof(NotSupportedException))]
        public void CreateGenericThrowsExceptionForConcreteClass()
        {
            Mock.Create<AccountService>();
        }

        [TestMethod, ExpectedException(typeof(NotSupportedException))]
        public void CreateTypeThrowsExceptionForAbstractClass()
        {
            Mock.Create(typeof(NonSupportedAbstractClass));
        }

        [TestMethod, ExpectedException(typeof(NotSupportedException))]
        public void CreateTypeThrowsExceptionForConcreteClass()
        {
            Mock.Create(typeof(AccountService));
        }
#endif

        [TestMethod]
        public void CreateGenericReturnsMockInstance()
        {
            var mock = Mock.Create<IAccountService>();

            Assert.IsNotNull(mock);
        }

        [TestMethod]
        public void CreateGenericReturnsMockObjectInstance()
        {
            var mock = Mock.Create<IAccountService>();

            Assert.IsNotNull(mock.Object);
        }

        [TestMethod]
        public void CreateGenericReturnsMockDispatcherInstance()
        {
            var mock = Mock.Create<IAccountService>();

            Assert.IsNotNull(mock.Dispatcher);
        }
        
        [TestMethod]
        public void CreateTypeReturnsMockInstance()
        {
            var mock = Mock.Create(typeof(IAccountService));

            Assert.IsNotNull(mock);
        }

        [TestMethod]
        public void CreateTypeReturnsMockObjectInstance()
        {
            var mock = Mock.Create(typeof(IAccountService));

            Assert.IsNotNull(mock.Object);
        }

        [TestMethod]
        public void CreateTypeReturnsMockDispatcherInstance()
        {
            var mock = Mock.Create(typeof(IAccountService));

            Assert.IsNotNull(mock.Dispatcher);
        }

        [TestMethod]
        public void SetupAddsMethodBehaviourInfoToDispatcher()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Setup(s => s.Credit("12345678", 100));
            
            Assert.IsTrue(mock.Dispatcher.Setups.InfoList.Any());
        }

        [TestMethod]
        public void SetupReturnsMethodCall()
        {
            var mock = Mock.Create<IAccountService>();

            var result = mock.Setup(s => s.Credit("12345678", 100));

            Assert.IsInstanceOfType(result, typeof(MethodCall));
        }

        [TestMethod]
        public void SetupAddsFunctionBehaviourInfoToDispatcher()
        {
            var mock = Mock.Create<IAccountService>();

            mock.Setup(s => s.CanDebit("12345678", 100));

            Assert.IsTrue(mock.Dispatcher.Setups.InfoList.Any());
        }

        [TestMethod]
        public void SetupReturnsFunctionCall()
        {
            var mock = Mock.Create<IAccountService>();

            var result = mock.Setup(s => s.CanDebit("12345678", 100));

            Assert.IsInstanceOfType(result, typeof(FunctionCall<bool>));
        }

        [TestMethod]
        public void SetupGetAddsPropertyBehaviourInfoToDispatcher()
        {
            var mock = Mock.Create<IAccountService>();

            mock.SetupGet(s => s.HasAccounts);

            Assert.IsTrue(mock.Dispatcher.Setups.InfoList.Any());
        }

        [TestMethod]
        public void SetupGetReturnsFunctionCall()
        {
            var mock = Mock.Create<IAccountService>();

            var result = mock.SetupGet(s => s.HasAccounts);

            Assert.IsInstanceOfType(result, typeof(GetPropertyCall<bool>));
        }

        [TestMethod]
        public void SetupSetAddsPropertyBehaviourInfoToDispatcher()
        {
            var mock = Mock.Create<IAccountService>();

            mock.SetupSet(s => s.HasAccounts);

            Assert.IsTrue(mock.Dispatcher.Setups.InfoList.Any());
        }

        [TestMethod]
        public void SetupSetReturnsMethodCall()
        {
            var mock = Mock.Create<IAccountService>();

            var result = mock.SetupSet(s => s.HasAccounts);

            Assert.IsInstanceOfType(result, typeof(SetPropertyCall<bool>));
        }
    }
}
