using System;
using System.Linq.Expressions;
using HyperMock.Core;
using HyperMock.Setups;
using Xunit;

namespace Tests.HyperMock.Setups
{
    public class SetupInfoListTests
    {
        private readonly SetupInfoList _setupInfoList;

        public SetupInfoListTests()
        {
            _setupInfoList = new SetupInfoList();
        }

        [Fact]
        public void AddOrGetPropertyWithNoGetterThrowsException()
        {
            Expression<Func<int>> expression = () => 1;

            Assert.Throws<ArgumentException>(() => _setupInfoList.AddOrGet(expression, CallType.GetProperty));
        }

        [Fact]
        public void AddOrGetGetterReturnsSetupInfo()
        {
            Expression<Func<int>> expression = () => TestProperty;

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.GetProperty);

            Assert.Equal("get_TestProperty", setupInfo.Name);
        }

        [Fact]
        public void AddOrGetGetterReturnsExistingSetupInfoForMatch()
        {
            Expression<Func<int>> expression = () => TestProperty;
            var existingSetupInfo = _setupInfoList.AddOrGet(expression, CallType.GetProperty);

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.GetProperty);

            Assert.Equal(existingSetupInfo, setupInfo);
        }

        [Fact]
        public void AddOrGetPropertyWithNoSetterThrowsException()
        {
            Expression<Func<int>> expression = () => 1;

            Assert.Throws<ArgumentException>(() => _setupInfoList.AddOrGet(expression, CallType.SetProperty));
        }

        [Fact]
        public void AddOrGetSetterReadOnlyPropertyThrowsException()
        {
            Expression<Func<int>> expression = () => TestProperty;

            Assert.Throws<ArgumentException>(() => _setupInfoList.AddOrGet(expression, CallType.SetProperty));
        }

        [Fact]
        public void AddOrGetSetterReturnsSetupInfo()
        {
            Expression<Func<int>> expression = () => TestProperty2;

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.SetProperty);

            Assert.Equal("set_TestProperty2", setupInfo.Name);
        }

        [Fact]
        public void AddOrGetSetterReturnsExistingSetupInfoForMatch()
        {
            Expression<Func<int>> expression = () => TestProperty2;
            var existingSetupInfo = _setupInfoList.AddOrGet(expression, CallType.SetProperty);

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.SetProperty);

            Assert.Equal(existingSetupInfo, setupInfo);
        }

        [Fact]
        public void AddOrGetMethodReturnsSetupInfoWithNoArgs()
        {
            Expression<Action> expression = () => TestMethod();

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.Method);

            Assert.Equal("TestMethod", setupInfo.Name);
            Assert.Equal(0, setupInfo.Parameters.Length);
        }

        [Fact]
        public void AddOrGetMethodReturnsExistingSetupInfoWithNoArgs()
        {
            Expression<Action> expression = () => TestMethod();
            var existingSetupInfo = _setupInfoList.AddOrGet(expression, CallType.Method);

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.Method);

            Assert.Equal(existingSetupInfo, setupInfo);
        }

        [Fact]
        public void AddOrGetMethodReturnsSetupInfoWithArgs()
        {
            Expression<Action> expression = () => TestMethod(0, 2);

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.Method);

            Assert.Equal("TestMethod", setupInfo.Name);
            Assert.Equal(2, setupInfo.Parameters.Length);
            Assert.Equal(0, setupInfo.Parameters[0].Value);
            Assert.Equal(2, setupInfo.Parameters[1].Value);
        }

        [Fact]
        public void AddOrGetFunctionReturnsSetupInfoWithNoArgs()
        {
            Expression<Func<int>> expression = () => TestFunction();

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.Function);

            Assert.Equal("TestFunction", setupInfo.Name);
            Assert.Equal(0, setupInfo.Parameters.Length);
        }

        [Fact]
        public void AddOrGetFunctionReturnsExistingSetupInfoWithNoArgs()
        {
            Expression<Func<int>> expression = () => TestFunction();
            var existingSetupInfo = _setupInfoList.AddOrGet(expression, CallType.Function);

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.Function);

            Assert.Equal(existingSetupInfo, setupInfo);
        }

        [Fact]
        public void AddOrGetFunctionReturnsSetupInfoWithArgs()
        {
            Expression<Func<int>> expression = () => TestFunction(0, 2);

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.Function);

            Assert.Equal("TestFunction", setupInfo.Name);
            Assert.Equal(2, setupInfo.Parameters.Length);
            Assert.Equal(0, setupInfo.Parameters[0].Value);
            Assert.Equal(2, setupInfo.Parameters[1].Value);
        }

        [Fact]
        public void FindByReturnsGetPropertySetupInfo()
        {
            Expression<Func<int>> expression = () => TestProperty;
            var expectedSetupInfo = _setupInfoList.AddOrGet(expression, CallType.GetProperty);

            var setupInfo = _setupInfoList.FindBy("get_TestProperty", new object[0]);

            Assert.Equal(expectedSetupInfo, setupInfo);
        }

        [Fact]
        public void FindByReturnsNullSetupInfoForUnknownGetProperty()
        {
            Expression<Func<int>> expression = () => TestProperty;
            _setupInfoList.AddOrGet(expression, CallType.GetProperty);

            var setupInfo = _setupInfoList.FindBy("get_UnknownTestProperty", new object[0]);

            Assert.Null(setupInfo);
        }

        [Fact]
        public void FindByReturnsSetPropertySetupInfo()
        {
            Expression<Func<int>> expression = () => TestProperty2;
            var expectedSetupInfo = _setupInfoList.AddOrGet(expression, CallType.SetProperty);

            var setupInfo = _setupInfoList.FindBy("set_TestProperty2", new object[0]);

            Assert.Equal(expectedSetupInfo, setupInfo);
        }

        [Fact]
        public void FindByReturnsNullSetupInfoForUnknownSetProperty()
        {
            Expression<Func<int>> expression = () => TestProperty2;
            _setupInfoList.AddOrGet(expression, CallType.SetProperty);

            var setupInfo = _setupInfoList.FindBy("get_UnknownTestProperty", new object[0]);

            Assert.Null(setupInfo);
        }

        [Fact]
        public void FindByReturnsMethodSetupInfo()
        {
            Expression<Action> expression = () => TestMethod(10, 20);
            var expectedSetupInfo = _setupInfoList.AddOrGet(expression, CallType.Method);

            var setupInfo = _setupInfoList.FindBy("TestMethod", new object[] {10, 20});

            Assert.Equal(expectedSetupInfo, setupInfo);
        }

        [Fact]
        public void FindByReturnsNullSetupInfoForUnknownMethod()
        {
            Expression<Action> expression = () => TestMethod(10, 20);
            _setupInfoList.AddOrGet(expression, CallType.Method);

            var setupInfo = _setupInfoList.FindBy("UnknownTestMethod", new object[0]);

            Assert.Null(setupInfo);
        }

        [Fact]
        public void FindByReturnsFunctionSetupInfo()
        {
            Expression<Func<int>> expression = () => TestFunction(10, 20);
            var expectedSetupInfo = _setupInfoList.AddOrGet(expression, CallType.Function);

            var setupInfo = _setupInfoList.FindBy("TestFunction", new object[] { 10, 20 });

            Assert.Equal(expectedSetupInfo, setupInfo);
        }

        [Fact]
        public void FindByReturnsNullSetupInfoForUnknownFunction()
        {
            Expression<Func<int>> expression = () => TestFunction(10, 20);
            _setupInfoList.AddOrGet(expression, CallType.Function);

            var setupInfo = _setupInfoList.FindBy("UnknownTestFunction", new object[0]);

            Assert.Null(setupInfo);
        }

        [Fact]
        public void AddOrGetIndexGetterReturnsSetupInfoForMatch()
        {
            Expression<Func<int>> expression = () => this["Homer"];

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.GetProperty);

            Assert.NotNull(setupInfo);
        }

        [Fact]
        public void AddOrGetIndexGetterReturnsExistingSetupInfoForMatch()
        {
            Expression<Func<int>> expression = () => this["Homer"];
            var existingSetupInfo = _setupInfoList.AddOrGet(expression, CallType.GetProperty);

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.GetProperty);

            Assert.Equal(existingSetupInfo, setupInfo);
        }

        [Fact]
        public void AddOrGetIndexSetterReturnsSetupInfoForMatch()
        {
            Expression<Func<int>> expression = () => this["Homer"];

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.SetProperty);

            Assert.NotNull(setupInfo);
        }

        [Fact]
        public void AddOrGetIndexSetterReturnsExistingSetupInfoForMatch()
        {
            Expression<Func<int>> expression = () => this["Homer"];
            var existingSetupInfo = _setupInfoList.AddOrGet(expression, CallType.SetProperty);

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.SetProperty);

            Assert.Equal(existingSetupInfo, setupInfo);
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
