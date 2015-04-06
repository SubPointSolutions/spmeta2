using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Utils;
using SPMeta2.Syntax.Default;
using SPMeta2.Containers.Utils;

namespace SPMeta2.Regression.Tests.Impl.ModelAPI
{
    [TestClass]
    public class SPMeta2ModelTests : SPMeta2RegresionTestBase
    {
        public SPMeta2ModelTests()
        {

        }

        #region common

        [ClassInitializeAttribute]
        public static void Init(TestContext context)
        {
            InternalInit();
        }

        [ClassCleanupAttribute]
        public static void Cleanup()
        {
            InternalCleanup();
        }

        [TestMethod]
        [TestCategory("Regression.SPMeta2Model")]
        public void CanDeploy_FarmModel()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var model = SPMeta2Model.NewFarmModel(m => { });

                TestModel(model);
            });
        }

        [TestMethod]
        [TestCategory("Regression.SPMeta2Model")]
        public void CanDeploy_WebApplicationModel()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var model = SPMeta2Model.NewWebApplicationModel(m => { });

                TestModel(model);
            });
        }

        [TestMethod]
        [TestCategory("Regression.SPMeta2Model")]
        public void CanDeploy_SiteModel()
        {
            var model = SPMeta2Model.NewSiteModel(m => { });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.SPMeta2Model")]
        public void CanDeploy_WebModel()
        {
            var model = SPMeta2Model.NewWebModel(m => { });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.SPMeta2Model")]
        public void CanDeploy_ListModel()
        {
            var model = SPMeta2Model.NewListModel(m => { });

            TestModel(model);
        }

        #endregion

        #region serialization

        [TestMethod]
        [TestCategory("Regression.SPMeta2Model.Serialization")]
        public void CanSerialize_SiteModelToXMLAndBack()
        {
            var orginalModel = SPMeta2Model.NewSiteModel(site =>
            {

            });

            TraceUtils.WithScope(trace =>
            {
                var modelString = SPMeta2Model.ToXML(orginalModel);
                Assert.IsFalse(string.IsNullOrEmpty(modelString));

                trace.WriteLine("XML");
                trace.WriteLine(modelString);

                var deserializedModel = SPMeta2Model.FromXML(modelString);
                Assert.IsNotNull(deserializedModel);

            });
        }


        [TestMethod]
        [TestCategory("Regression.SPMeta2Model.Serialization")]
        public void CanSerialize_SiteModelToJSONAndBack()
        {
            var orginalModel = SPMeta2Model.NewSiteModel(site =>
            {

            });

            TraceUtils.WithScope(trace =>
            {
                var modelString = SPMeta2Model.ToJSON(orginalModel);
                Assert.IsFalse(string.IsNullOrEmpty(modelString));

                trace.WriteLine("JSON");
                trace.WriteLine(modelString);

                var deserializedModel = SPMeta2Model.FromJSON(modelString);
                Assert.IsNotNull(deserializedModel);
            });
        }

        #endregion
    }
}
