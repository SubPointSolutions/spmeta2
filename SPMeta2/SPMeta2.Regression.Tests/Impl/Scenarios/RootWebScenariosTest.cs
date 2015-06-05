using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Containers.Standard;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Definitions;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Syntax.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Syntax.Default.Modern;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class RootWebScenariosTest : SPMeta2RegresionScenarioTestBase
    {
        #region internal

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

        #endregion

        #region group link options

        [TestMethod]
        [TestCategory("Regression.Scenarios.RootWeb")]
        public void CanDeploy_RootWeb_UnderSite()
        {
            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddRootWeb(new RootWebDefinition());
            });


            TestModels(new[] { siteModel });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.RootWeb")]
        public void CanDeploy_RootWeb_UnderRootWeb()
        {
            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRootWeb(new RootWebDefinition());
            });


            TestModels(new[] { webModel });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.RootWeb")]
        public void CanDeploy_RootWeb_UnderSubWeb()
        {
            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomWeb(subWeb =>
                {
                    subWeb.AddRootWeb(new RootWebDefinition());
                });
            });


            TestModels(new[] { webModel });
        }

        #endregion
    }
}
