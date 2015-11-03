using Microsoft.VisualStudio.TestTools.UnitTesting;
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
using SPMeta2.Containers;
using SPMeta2.Containers.Services;
using System.IO;
using SPMeta2.Containers.Consts;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Syntax;


namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class SearchSettingsScenariosTest : SPMeta2RegresionScenarioTestBase
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

        #region webs localization

        [TestMethod]
        [TestCategory("Regression.Scenarios.SearchSettings")]
        public void CanDeploy_SearchSettings_Under_Site()
        {
            var settings = ModelGeneratorService.GetRandomDefinition<SearchSettingsDefinition>();

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSearchSettings(settings);
            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.SearchSettings")]
        public void CanDeploy_SearchSettings_Under_RootWeb()
        {
            var settings = ModelGeneratorService.GetRandomDefinition<SearchSettingsDefinition>();

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddSearchSettings(settings);
            });

            TestModel(model);
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.SearchSettings")]
        public void CanDeploy_SearchSettings_Under_SubWeb()
        {

            var settings = ModelGeneratorService.GetRandomDefinition<SearchSettingsDefinition>();

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomWeb(c =>
                {
                    web.AddSearchSettings(settings);
                });
            });

            TestModel(model);
        }

        #endregion
    }
}
