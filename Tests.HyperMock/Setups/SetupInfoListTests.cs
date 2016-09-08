using System;
using System.Linq.Expressions;
using HyperMock.Core;
using HyperMock.Setups;
#if WINDOWS_UWP
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace Tests.HyperMock.Setups
{
    [TestClass]
    public class SetupInfoListTests
    {
        private SetupInfoList _setupInfoList;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _setupInfoList = new SetupInfoList();
        }

#if WINDOWS_UWP
        [TestMethod]
        public void AddOrGetPropertyWithNoGetterThrowsException()
        {
            Expression<Func<int>> expression = () => 1;

            Assert.ThrowsException<ArgumentException>(() => _setupInfoList.AddOrGet(expression, CallType.GetProperty));
        }
#else
        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void AddOrGetPropertyWithNoGetterThrowsException()
        {
            Expression<Func<int>> expression = () => 1;

            _setupInfoList.AddOrGet(expression, CallType.GetProperty);
        }
#endif

        [TestMethod]
        public void AddOrGetGetterReturnsSetupInfo()
        {
            Expression<Func<int>> expression = () => TestProperty;

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.GetProperty);

            Assert.AreEqual("get_TestProperty", setupInfo.Name);
        }

        [TestMethod]
        public void AddOrGetGetterReturnsExistingSetupInfoForMatch()
        {
            Expression<Func<int>> expression = () => TestProperty;
            var existingSetupInfo = _setupInfoList.AddOrGet(expression, CallType.GetProperty);

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.GetProperty);

            Assert.AreEqual(existingSetupInfo, setupInfo);
        }

#if WINDOWS_UWP
        [TestMethod]
        public void AddOrGetPropertyWithNoSetterThrowsException()
        {
            Expression<Func<int>> expression = () => 1;

            Assert.ThrowsException<ArgumentException>(() => _setupInfoList.AddOrGet(expression, CallType.SetProperty));
        }

        [TestMethod]
        public void AddOrGetSetterReadOnlyPropertyThrowsException()
        {
            Expression<Func<int>> expression = () => TestProperty;

            Assert.ThrowsException<ArgumentException>(() => _setupInfoList.AddOrGet(expression, CallType.SetProperty));
        }
#else
        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void AddOrGetPropertyWithNoSetterThrowsException()
        {
            Expression<Func<int>> expression = () => 1;

            _setupInfoList.AddOrGet(expression, CallType.SetProperty);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void AddOrGetSetterReadOnlyPropertyThrowsException()
        {
            Expression<Func<int>> expression = () => TestProperty;

            _setupInfoList.AddOrGet(expression, CallType.SetProperty);
        }
#endif

        [TestMethod]
        public void AddOrGetSetterReturnsSetupInfo()
        {
            Expression<Func<int>> expression = () => TestProperty2;

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.SetProperty);

            Assert.AreEqual("set_TestProperty2", setupInfo.Name);
        }

        [TestMethod]
        public void AddOrGetSetterReturnsExistingSetupInfoForMatch()
        {
            Expression<Func<int>> expression = () => TestProperty2;
            var existingSetupInfo = _setupInfoList.AddOrGet(expression, CallType.SetProperty);

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.SetProperty);

            Assert.AreEqual(existingSetupInfo, setupInfo);
        }

        [TestMethod]
        public void AddOrGetMethodReturnsSetupInfoWithNoArgs()
        {
            Expression<Action> expression = () => TestMethod();

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.Method);

            Assert.AreEqual("TestMethod", setupInfo.Name);
            Assert.AreEqual(0, setupInfo.Parameters.Length);
        }

        [TestMethod]
        public void AddOrGetMethodReturnsExistingSetupInfoWithNoArgs()
        {
            Expression<Action> expression = () => TestMethod();
            var existingSetupInfo = _setupInfoList.AddOrGet(expression, CallType.Method);

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.Method);

            Assert.AreEqual(existingSetupInfo, setupInfo);
        }

        [TestMethod]
        public void AddOrGetMethodReturnsSetupInfoWithArgs()
        {
            Expression<Action> expression = () => TestMethod(0, 2);

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.Method);

            Assert.AreEqual("TestMethod", setupInfo.Name);
            Assert.AreEqual(2, setupInfo.Parameters.Length);
            Assert.AreEqual(0, setupInfo.Parameters[0].Value);
            Assert.AreEqual(2, setupInfo.Parameters[1].Value);
        }

        [TestMethod]
        public void AddOrGetFunctionReturnsSetupInfoWithNoArgs()
        {
            Expression<Func<int>> expression = () => TestFunction();

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.Function);

            Assert.AreEqual("TestFunction", setupInfo.Name);
            Assert.AreEqual(0, setupInfo.Parameters.Length);
        }

        [TestMethod]
        public void AddOrGetFunctionReturnsExistingSetupInfoWithNoArgs()
        {
            Expression<Func<int>> expression = () => TestFunction();
            var existingSetupInfo = _setupInfoList.AddOrGet(expression, CallType.Function);

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.Function);

            Assert.AreEqual(existingSetupInfo, setupInfo);
        }

        [TestMethod]
        public void AddOrGetFunctionReturnsSetupInfoWithArgs()
        {
            Expression<Func<int>> expression = () => TestFunction(0, 2);

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.Function);

            Assert.AreEqual("TestFunction", setupInfo.Name);
            Assert.AreEqual(2, setupInfo.Parameters.Length);
            Assert.AreEqual(0, setupInfo.Parameters[0].Value);
            Assert.AreEqual(2, setupInfo.Parameters[1].Value);
        }

        [TestMethod]
        public void FindByReturnsGetPropertySetupInfo()
        {
            Expression<Func<int>> expression = () => TestProperty;
            var expectedSetupInfo = _setupInfoList.AddOrGet(expression, CallType.GetProperty);

            var setupInfo = _setupInfoList.FindBy("get_TestProperty", new object[0]);

            Assert.AreEqual(expectedSetupInfo, setupInfo);
        }

        [TestMethod]
        public void FindByReturnsNullSetupInfoForUnknownGetProperty()
        {
            Expression<Func<int>> expression = () => TestProperty;
            _setupInfoList.AddOrGet(expression, CallType.GetProperty);

            var setupInfo = _setupInfoList.FindBy("get_UnknownTestProperty", new object[0]);

            Assert.IsNull(setupInfo);
        }

        [TestMethod]
        public void FindByReturnsSetPropertySetupInfo()
        {
            Expression<Func<int>> expression = () => TestProperty2;
            var expectedSetupInfo = _setupInfoList.AddOrGet(expression, CallType.SetProperty);

            var setupInfo = _setupInfoList.FindBy("set_TestProperty2", new object[0]);

            Assert.AreEqual(expectedSetupInfo, setupInfo);
        }

        [TestMethod]
        public void FindByReturnsNullSetupInfoForUnknownSetProperty()
        {
            Expression<Func<int>> expression = () => TestProperty2;
            _setupInfoList.AddOrGet(expression, CallType.SetProperty);

            var setupInfo = _setupInfoList.FindBy("get_UnknownTestProperty", new object[0]);

            Assert.IsNull(setupInfo);
        }

        [TestMethod]
        public void FindByReturnsMethodSetupInfo()
        {
            Expression<Action> expression = () => TestMethod(10, 20);
            var expectedSetupInfo = _setupInfoList.AddOrGet(expression, CallType.Method);

            var setupInfo = _setupInfoList.FindBy("TestMethod", new object[] {10, 20});

            Assert.AreEqual(expectedSetupInfo, setupInfo);
        }

        [TestMethod]
        public void FindByReturnsNullSetupInfoForUnknownMethod()
        {
            Expression<Action> expression = () => TestMethod(10, 20);
            _setupInfoList.AddOrGet(expression, CallType.Method);

            var setupInfo = _setupInfoList.FindBy("UnknownTestMethod", new object[0]);

            Assert.IsNull(setupInfo);
        }

        [TestMethod]
        public void FindByReturnsFunctionSetupInfo()
        {
            Expression<Func<int>> expression = () => TestFunction(10, 20);
            var expectedSetupInfo = _setupInfoList.AddOrGet(expression, CallType.Function);

            var setupInfo = _setupInfoList.FindBy("TestFunction", new object[] { 10, 20 });

            Assert.AreEqual(expectedSetupInfo, setupInfo);
        }

        [TestMethod]
        public void FindByReturnsNullSetupInfoForUnknownFunction()
        {
            Expression<Func<int>> expression = () => TestFunction(10, 20);
            _setupInfoList.AddOrGet(expression, CallType.Function);

            var setupInfo = _setupInfoList.FindBy("UnknownTestFunction", new object[0]);

            Assert.IsNull(setupInfo);
        }

        [TestMethod]
        public void AddOrGetIndexGetterReturnsSetupInfoForMatch()
        {
            Expression<Func<int>> expression = () => this["Homer"];

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.GetProperty);

            Assert.IsNotNull(setupInfo);
        }

        [TestMethod]
        public void AddOrGetIndexGetterReturnsExistingSetupInfoForMatch()
        {
            Expression<Func<int>> expression = () => this["Homer"];
            var existingSetupInfo = _setupInfoList.AddOrGet(expression, CallType.GetProperty);

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.GetProperty);

            Assert.AreEqual(existingSetupInfo, setupInfo);
        }

        [TestMethod]
        public void AddOrGetIndexSetterReturnsSetupInfoForMatch()
        {
            Expression<Func<int>> expression = () => this["Homer"];

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.SetProperty);

            Assert.IsNotNull(setupInfo);
        }

        [TestMethod]
        public void AddOrGetIndexSetterReturnsExistingSetupInfoForMatch()
        {
            Expression<Func<int>> expression = () => this["Homer"];
            var existingSetupInfo = _setupInfoList.AddOrGet(expression, CallType.SetProperty);

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.SetProperty);

            Assert.AreEqual(existingSetupInfo, setupInfo);
        }

        // ReSharper disable once UnassignedGetOnlyAutoProperty
        private int TestProperty { get; }
        
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private int TestProperty2 { get; set; }

        private int this[string name]
        {
            get { return name != null ? TestProperty2 : 0; }
            // ReSharper disable once UnusedMember.Local
            set { TestProperty2 = value; }
        }

        private void TestMethod()
        {

        }

        // ReSharper disable UnusedParameter.Local
        private void TestMethod(int x, int y)
        // ReSharper restore UnusedParameter.Local
        {

        }

        private int TestFunction()
        {
            return 0;
        }

        // ReSharper disable UnusedParameter.Local
        private int TestFunction(int x, int y)
        // ReSharper restore UnusedParameter.Local
        {
            return 0;
        }
    }
}
