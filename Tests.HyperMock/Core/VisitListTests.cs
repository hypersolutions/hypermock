using System;
using System.Linq.Expressions;
using HyperMock;
using HyperMock.Core;
#if WINDOWS_UWP
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace Tests.HyperMock.Core
{
    [TestClass]
    public class VisitListTests
    {
        private VisitList _visits;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _visits = new VisitList();
        }

        [TestMethod]
        public void RecordInsertsFirstTimeCallWithNoArgs()
        {
            var visit = _visits.Record("Save", null);

            Assert.AreEqual(1, visit.VisitCount);
        }

        [TestMethod]
        public void RecordUpdatesRepeatCallWithNoArgs()
        {
            _visits.Record("Save", null);

            var visit = _visits.Record("Save", null);

            Assert.AreEqual(2, visit.VisitCount);
        }

        [TestMethod]
        public void RecordInsertsCallWithMismatchingArgs()
        {
            _visits.Record("Save", new object[] { 10 });

            var visit = _visits.Record("Save", new object[] { 20 });

            Assert.AreEqual(1, visit.VisitCount);
        }

        [TestMethod]
        public void RecordInsertsCallWithNullMismatchingArgs()
        {
            _visits.Record("Save", new object[] { 10 });

            var visit = _visits.Record("Save", null);

            Assert.AreEqual(1, visit.VisitCount);
        }

        [TestMethod]
        public void RecordInsertsRepeatCallWithSingleArg()
        {
            _visits.Record("Save", new object[]{10});

            var visit = _visits.Record("Save", new object[] {10});

            Assert.AreEqual(2, visit.VisitCount);
        }

        [TestMethod]
        public void RecordInsertsRepeatCallWithMultipleArgs()
        {
            _visits.Record("Save", new object[] { 10, "Homer" });

            var visit = _visits.Record("Save", new object[] { 10, "Homer" });

            Assert.AreEqual(2, visit.VisitCount);
        }

        [TestMethod]
        public void FindByReturnsMethodVisitForExactMatch()
        {
            _visits.Record("Save", new object[] { 10 });
            Expression<Action> expression = () => Save(10);

            var visit = _visits.FindBy(expression, CallType.Method);

            Assert.IsNotNull(visit);
        }

        [TestMethod]
        public void FindByReturnsNullMethodVisitForArgsMismatch()
        {
            _visits.Record("Save", new object[] { 10 });
            Expression<Action> expression = () => Save(20);

            var visit = _visits.FindBy(expression, CallType.Method);

            Assert.IsNull(visit);
        }

        [TestMethod]
        public void FindByReturnsMethodVisitWithAnyArgsMatch()
        {
            _visits.Record("Save", new object[] { 10 });
            Expression<Action> expression = () => Save(Param.IsAny<int>());

            var visit = _visits.FindBy(expression, CallType.Method);

            Assert.IsNotNull(visit);
        }

        [TestMethod]
        public void FindByReturnsFunctionVisitForExactMatch()
        {
            _visits.Record("Load", new object[] { "Homer" });
            Expression<Func<int>> expression = () => Load("Homer");

            var visit = _visits.FindBy(expression, CallType.Function);

            Assert.IsNotNull(visit);
        }

        [TestMethod]
        public void FindByReturnsNullFunctionVisitForArgsMismatch()
        {
            _visits.Record("Load", new object[] { "Homer" });
            Expression<Func<int>> expression = () => Load("Marge");

            var visit = _visits.FindBy(expression, CallType.Function);

            Assert.IsNull(visit);
        }

        [TestMethod]
        public void FindByReturnsFunctionVisitWithAnyArgsMatch()
        {
            _visits.Record("Load", new object[] { "Homer" });
            Expression<Func<int>> expression = () => Load(Param.IsAny<string>());

            var visit = _visits.FindBy(expression, CallType.Function);

            Assert.IsNotNull(visit);
        }

        [TestMethod]
        public void FindByReturnsGetPropertyVisit()
        {
            _visits.Record("get_Age", new object[0]);
            Expression<Func<int>> expression = () => Age;

            var visit = _visits.FindBy(expression, CallType.GetProperty);

            Assert.IsNotNull(visit);
        }

        [TestMethod]
        public void FindByReturnsNullGetPropertyVisit()
        {
            _visits.Record("get_Age", new object[0]);
            Expression<Func<string>> expression = () => Gender;

            var visit = _visits.FindBy(expression, CallType.GetProperty);

            Assert.IsNull(visit);
        }

        [TestMethod]
        public void FindByReturnsSetPropertyVisit()
        {
            _visits.Record("set_Age", new object[] {30});
            Expression<Func<int>> expression = () => Age;

            var visit = _visits.FindBy(expression, CallType.SetProperty);

            Assert.IsNotNull(visit);
        }

        [TestMethod]
        public void FindByReturnsNullSetPropertyVisit()
        {
            _visits.Record("set_Age", new object[0]);
            Expression<Func<string>> expression = () => Gender;

            var visit = _visits.FindBy(expression, CallType.SetProperty);

            Assert.IsNull(visit);
        }

        [TestMethod]
        public void FindByReturnsGetIndexerPropertyVisit()
        {
            _visits.Record("get_Item", new object[] {"Homer"});
            Expression<Func<int>> expression = () => this["Homer"];

            var visit = _visits.FindBy(expression, CallType.GetProperty);

            Assert.IsNotNull(visit);
        }

        [TestMethod]
        public void FindByReturnsSetIndexerPropertyVisit()
        {
            _visits.Record("set_Item", new object[] { "Homer" });
            Expression<Func<int>> expression = () => this["Homer"];

            var visit = _visits.FindBy(expression, CallType.SetProperty, new object[] {"Homer"});

            Assert.IsNotNull(visit);
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

        private int Load(string name)
        {
            System.Diagnostics.Debug.WriteLine(name);
            return 0;
        }
    }
}
