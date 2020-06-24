using System;
using System.Linq.Expressions;
using System.Reflection;
using HyperMock.Core;
using Shouldly;
using Xunit;

namespace HyperMock.Tests.Core
{
    public class VisitListTests
    {
        private readonly VisitList _visits;

        public VisitListTests()
        {
            _visits = new VisitList();
        }

        [Fact]
        public void RecordInsertsFirstTimeCallWithNoArgs()
        {
            var method = GetMethod("Save");

            var visit = _visits.Record(method, null);

            visit.VisitCount.ShouldBe(1);
        }

        [Fact]
        public void RecordUpdatesRepeatCallWithNoArgs()
        {
            var method = GetMethod("Save");
            _visits.Record(method, null);

            var visit = _visits.Record(method, null);

            visit.VisitCount.ShouldBe(2);
        }

        [Fact]
        public void RecordInsertsCallWithMismatchingArgs()
        {
            var method = GetMethod("Save");
            _visits.Record(method, new object[] { 10 });

            var visit = _visits.Record(method, new object[] { 20 });

            visit.VisitCount.ShouldBe(1);
        }

        [Fact]
        public void RecordInsertsCallWithNullMismatchingArgs()
        {
            var method = GetMethod("Save");
            _visits.Record(method, new object[] { 10 });

            var visit = _visits.Record(method, null);

            visit.VisitCount.ShouldBe(1);
        }

        [Fact]
        public void RecordInsertsRepeatCallWithSingleArg()
        {
            var method = GetMethod("Save");
            _visits.Record(method, new object[]{10});

            var visit = _visits.Record(method, new object[] {10});

            visit.VisitCount.ShouldBe(2);
        }

        [Fact]
        public void RecordInsertsRepeatCallWithMultipleArgs()
        {
            var method = GetMethod("Save2");
            _visits.Record(method, new object[] { 10, "Homer" });

            var visit = _visits.Record(method, new object[] { 10, "Homer" });

            visit.VisitCount.ShouldBe(2);
        }

        [Fact]
        public void FindByReturnsMethodVisitForExactMatch()
        {
            var method = GetMethod("Save");
            _visits.Record(method, new object[] { 10 });
            Expression<Action> expression = () => Save(10);

            var visits = _visits.FindBy(expression, CallType.Method);

            visits.Length.ShouldBe(1);
        }

        [Fact]
        public void FindByReturnsEmptyMethodVisitForArgsMismatch()
        {
            var method = GetMethod("Save");
            _visits.Record(method, new object[] { 10 });
            Expression<Action> expression = () => Save(20);

            var visits = _visits.FindBy(expression, CallType.Method);

            visits.Length.ShouldBe(0);
        }

        [Fact]
        public void FindByReturnsMethodVisitWithAnyArgsMatch()
        {
            var method = GetMethod("Save");
            _visits.Record(method, new object[] { 10 });
            Expression<Action> expression = () => Save(Param.IsAny<int>());

            var visits = _visits.FindBy(expression, CallType.Method);

            visits.Length.ShouldBe(1);
        }

        [Fact]
        public void FindByReturnsFunctionVisitForExactMatch()
        {
            var method = GetMethod("Load");
            _visits.Record(method, new object[] { "Homer" });
            Expression<Func<int>> expression = () => Load("Homer");

            var visits = _visits.FindBy(expression, CallType.Function);

            visits.Length.ShouldBe(1);
        }

        [Fact]
        public void FindByReturnsEmptyFunctionVisitForArgsMismatch()
        {
            var method = GetMethod("Load");
            _visits.Record(method, new object[] { "Homer" });
            Expression<Func<int>> expression = () => Load("Marge");

            var visits = _visits.FindBy(expression, CallType.Function);

            visits.Length.ShouldBe(0);
        }

        [Fact]
        public void FindByReturnsFunctionVisitWithAnyArgsMatch()
        {
            var method = GetMethod("Load");
            _visits.Record(method, new object[] { "Homer" });
            Expression<Func<int>> expression = () => Load(Param.IsAny<string>());

            var visits = _visits.FindBy(expression, CallType.Function);

            visits.Length.ShouldBe(1);
        }

        [Fact]
        public void FindByReturnsGetPropertyVisit()
        {
            var method = GetMethod("get_Age");
            _visits.Record(method, new object[0]);
            Expression<Func<int>> expression = () => Age;

            var visits = _visits.FindBy(expression, CallType.GetProperty);

            visits.Length.ShouldBe(1);
        }

        [Fact]
        public void FindByReturnsEmptyGetPropertyVisit()
        {
            var method = GetMethod("get_Age");
            _visits.Record(method, new object[0]);
            Expression<Func<string>> expression = () => Gender;

            var visits = _visits.FindBy(expression, CallType.GetProperty);

            visits.Length.ShouldBe(0);
        }

        [Fact]
        public void FindByReturnsSetPropertyVisit()
        {
            var method = GetMethod("set_Age");
            _visits.Record(method, new object[] {30});
            Expression<Func<int>> expression = () => Age;

            var visits = _visits.FindBy(expression, CallType.SetProperty);

            visits.Length.ShouldBe(1);
        }

        [Fact]
        public void FindByReturnsEmptySetPropertyVisit()
        {
            var method = GetMethod("set_Age");
            _visits.Record(method, new object[0]);
            Expression<Func<string>> expression = () => Gender;

            var visits = _visits.FindBy(expression, CallType.SetProperty);

            visits.Length.ShouldBe(0);
        }

        [Fact]
        public void FindByReturnsGetIndexerPropertyVisit()
        {
            var method = GetMethod("get_Item");
            _visits.Record(method, new object[] {"Homer"});
            Expression<Func<int>> expression = () => this["Homer"];

            var visits = _visits.FindBy(expression, CallType.GetProperty);

            visits.Length.ShouldBe(1);
        }

        [Fact]
        public void FindByReturnsSetIndexerPropertyVisit()
        {
            var method = GetMethod("set_Item");
            _visits.Record(method, new object[] { "Homer" });
            Expression<Func<int>> expression = () => this["Homer"];

            var visits = _visits.FindBy(expression, CallType.SetProperty, new object[] {"Homer"});

            visits.Length.ShouldBe(1);
        }

        [Fact]
        public void ResetCallsRemovesVisits()
        {
            var method = GetMethod("Save");
            _visits.Record(method, null);

            _visits.Reset();

            _visits.RecordedVisits.Count.ShouldBe(0);
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private int Age { get; set; }
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private string Gender { get; set; }

        private int this[string name]
        {
            get { return name != null ? Age : 0; }
            // ReSharper disable once UnusedMember.Local
            set { Age = value; }
        }

        private void Save(int x)
        {
            System.Diagnostics.Debug.WriteLine(x);
        }

        // ReSharper disable once UnusedMember.Local
        private void Save2(int x, string text)
        {
            System.Diagnostics.Debug.WriteLine(text + x);
        }

        private int Load(string name)
        {
            System.Diagnostics.Debug.WriteLine(name);
            return 0;
        }

        private MethodBase GetMethod(string name)
        {
            return GetType().GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance);
        }
    }
}
