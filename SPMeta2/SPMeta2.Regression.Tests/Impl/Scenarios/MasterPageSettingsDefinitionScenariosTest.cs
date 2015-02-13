using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class MasterPageSettingsDefinitionScenariosTest : SPMeta2RegresionScenarioTestBase
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

        #region default

        [TestMethod]
        [TestCategory("Regression.Scenarios.MasterPageSettings")]
        public void CanDeploy_MasterPageSettings_SiteMasterPageUrl_Only()
        {
            var settings = BuiltInDefinitions.BuiltInMasterPageDefinitions.Minimal.Inherit<MasterPageSettingsDefinition>(def =>
            {
                def.SystemMasterPageUrl = string.Empty;
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddRandomWeb(newWeb =>
                    {
                        newWeb.AddMasterPageSettings(settings);
                    });

                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.MasterPageSettings")]
        public void CanDeploy_MasterPageSettings_SystemMasterPageUrl_Only()
        {
            var settings = BuiltInDefinitions.BuiltInMasterPageDefinitions.Minimal.Inherit<MasterPageSettingsDefinition>(def =>
            {
                def.SiteMasterPageUrl = string.Empty;
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddRandomWeb(newWeb =>
                    {
                        newWeb.AddMasterPageSettings(settings);
                    });

                });

            TestModel(model);
        }

        #endregion
    }
}
