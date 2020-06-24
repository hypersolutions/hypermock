using System;
using System.Linq.Expressions;
using HyperMock.Core;
using HyperMock.Setups;
using Shouldly;
using Xunit;

namespace HyperMock.Tests.Setups
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

            Should.Throw<ArgumentException>(() => _setupInfoList.AddOrGet(expression, CallType.GetProperty));
        }

        [Fact]
        public void AddOrGetGetterReturnsSetupInfo()
        {
            Expression<Func<int>> expression = () => TestProperty;

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.GetProperty);

            setupInfo.Name.ShouldBe("get_TestProperty");
        }

        [Fact]
        public void AddOrGetGetterReturnsExistingSetupInfoForMatch()
        {
            Expression<Func<int>> expression = () => TestProperty;
            var existingSetupInfo = _setupInfoList.AddOrGet(expression, CallType.GetProperty);

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.GetProperty);

            setupInfo.ShouldBe(existingSetupInfo);
        }

        [Fact]
        public void AddOrGetPropertyWithNoSetterThrowsException()
        {
            Expression<Func<int>> expression = () => 1;

            Should.Throw<ArgumentException>(() => _setupInfoList.AddOrGet(expression, CallType.SetProperty));
        }

        [Fact]
        public void AddOrGetSetterReadOnlyPropertyThrowsException()
        {
            Expression<Func<int>> expression = () => TestProperty;

            Should.Throw<ArgumentException>(() => _setupInfoList.AddOrGet(expression, CallType.SetProperty));
        }

        [Fact]
        public void AddOrGetSetterReturnsSetupInfo()
        {
            Expression<Func<int>> expression = () => TestProperty2;

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.SetProperty);

            setupInfo.Name.ShouldBe("set_TestProperty2");
        }

        [Fact]
        public void AddOrGetSetterReturnsExistingSetupInfoForMatch()
        {
            Expression<Func<int>> expression = () => TestProperty2;
            var existingSetupInfo = _setupInfoList.AddOrGet(expression, CallType.SetProperty);

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.SetProperty);

            setupInfo.ShouldBe(existingSetupInfo);
        }

        [Fact]
        public void AddOrGetMethodReturnsSetupInfoWithNoArgs()
        {
            Expression<Action> expression = () => TestMethod();

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.Method);

            setupInfo.Name.ShouldBe("TestMethod");
            setupInfo.Parameters.Length.ShouldBe(0);
        }

        [Fact]
        public void AddOrGetMethodReturnsExistingSetupInfoWithNoArgs()
        {
            Expression<Action> expression = () => TestMethod();
            var existingSetupInfo = _setupInfoList.AddOrGet(expression, CallType.Method);

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.Method);

            setupInfo.ShouldBe(existingSetupInfo);
        }

        [Fact]
        public void AddOrGetMethodReturnsSetupInfoWithArgs()
        {
            Expression<Action> expression = () => TestMethod(0, 2);

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.Method);

            setupInfo.Name.ShouldBe("TestMethod");
            setupInfo.Parameters.Length.ShouldBe(2);
            setupInfo.Parameters[0].Value.ShouldBe(0);
            setupInfo.Parameters[1].Value.ShouldBe(2);
        }

        [Fact]
        public void AddOrGetFunctionReturnsSetupInfoWithNoArgs()
        {
            Expression<Func<int>> expression = () => TestFunction();

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.Function);

            setupInfo.Name.ShouldBe("TestFunction");
            setupInfo.Parameters.Length.ShouldBe(0);
        }

        [Fact]
        public void AddOrGetFunctionReturnsExistingSetupInfoWithNoArgs()
        {
            Expression<Func<int>> expression = () => TestFunction();
            var existingSetupInfo = _setupInfoList.AddOrGet(expression, CallType.Function);

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.Function);

            setupInfo.ShouldBe(existingSetupInfo);
        }

        [Fact]
        public void AddOrGetFunctionReturnsSetupInfoWithArgs()
        {
            Expression<Func<int>> expression = () => TestFunction(0, 2);

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.Function);

            setupInfo.Name.ShouldBe("TestFunction");
            setupInfo.Parameters.Length.ShouldBe(2);
            setupInfo.Parameters[0].Value.ShouldBe(0);
            setupInfo.Parameters[1].Value.ShouldBe(2);
        }

        [Fact]
        public void FindByReturnsGetPropertySetupInfo()
        {
            Expression<Func<int>> expression = () => TestProperty;
            var expectedSetupInfo = _setupInfoList.AddOrGet(expression, CallType.GetProperty);

            var setupInfo = _setupInfoList.FindBy("get_TestProperty", new object[0]);

            setupInfo.ShouldBe(expectedSetupInfo);
        }

        [Fact]
        public void FindByReturnsNullSetupInfoForUnknownGetProperty()
        {
            Expression<Func<int>> expression = () => TestProperty;
            _setupInfoList.AddOrGet(expression, CallType.GetProperty);

            var setupInfo = _setupInfoList.FindBy("get_UnknownTestProperty", new object[0]);

            setupInfo.ShouldBeNull();
        }

        [Fact]
        public void FindByReturnsSetPropertySetupInfo()
        {
            Expression<Func<int>> expression = () => TestProperty2;
            var expectedSetupInfo = _setupInfoList.AddOrGet(expression, CallType.SetProperty);

            var setupInfo = _setupInfoList.FindBy("set_TestProperty2", new object[0]);

            setupInfo.ShouldBe(expectedSetupInfo);
        }

        [Fact]
        public void FindByReturnsNullSetupInfoForUnknownSetProperty()
        {
            Expression<Func<int>> expression = () => TestProperty2;
            _setupInfoList.AddOrGet(expression, CallType.SetProperty);

            var setupInfo = _setupInfoList.FindBy("get_UnknownTestProperty", new object[0]);

            setupInfo.ShouldBeNull();
        }

        [Fact]
        public void FindByReturnsMethodSetupInfo()
        {
            Expression<Action> expression = () => TestMethod(10, 20);
            var expectedSetupInfo = _setupInfoList.AddOrGet(expression, CallType.Method);

            var setupInfo = _setupInfoList.FindBy("TestMethod", new object[] {10, 20});

            setupInfo.ShouldBe(expectedSetupInfo);
        }

        [Fact]
        public void FindByReturnsNullSetupInfoForUnknownMethod()
        {
            Expression<Action> expression = () => TestMethod(10, 20);
            _setupInfoList.AddOrGet(expression, CallType.Method);

            var setupInfo = _setupInfoList.FindBy("UnknownTestMethod", new object[0]);

            setupInfo.ShouldBeNull();
        }

        [Fact]
        public void FindByReturnsFunctionSetupInfo()
        {
            Expression<Func<int>> expression = () => TestFunction(10, 20);
            var expectedSetupInfo = _setupInfoList.AddOrGet(expression, CallType.Function);

            var setupInfo = _setupInfoList.FindBy("TestFunction", new object[] { 10, 20 });

            setupInfo.ShouldBe(expectedSetupInfo);
        }

        [Fact]
        public void FindByReturnsNullSetupInfoForUnknownFunction()
        {
            Expression<Func<int>> expression = () => TestFunction(10, 20);
            _setupInfoList.AddOrGet(expression, CallType.Function);

            var setupInfo = _setupInfoList.FindBy("UnknownTestFunction", new object[0]);

            setupInfo.ShouldBeNull();
        }

        [Fact]
        public void AddOrGetIndexGetterReturnsSetupInfoForMatch()
        {
            Expression<Func<int>> expression = () => this["Homer"];

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.GetProperty);

            setupInfo.ShouldNotBeNull();
        }

        [Fact]
        public void AddOrGetIndexGetterReturnsExistingSetupInfoForMatch()
        {
            Expression<Func<int>> expression = () => this["Homer"];
            var existingSetupInfo = _setupInfoList.AddOrGet(expression, CallType.GetProperty);

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.GetProperty);

            setupInfo.ShouldBe(existingSetupInfo);
        }

        [Fact]
        public void AddOrGetIndexSetterReturnsSetupInfoForMatch()
        {
            Expression<Func<int>> expression = () => this["Homer"];

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.SetProperty);

            setupInfo.ShouldNotBeNull();
        }

        [Fact]
        public void AddOrGetIndexSetterReturnsExistingSetupInfoForMatch()
        {
            Expression<Func<int>> expression = () => this["Homer"];
            var existingSetupInfo = _setupInfoList.AddOrGet(expression, CallType.SetProperty);

            var setupInfo = _setupInfoList.AddOrGet(expression, CallType.SetProperty);

            setupInfo.ShouldBe(existingSetupInfo);
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
