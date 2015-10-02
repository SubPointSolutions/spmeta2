using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Modern;
using SPMeta2.Syntax.Extended;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers.Standard;
using SPMeta2.Models;
using SPMeta2.Containers.Consts;
using System.Diagnostics;
using SPMeta2.Exceptions;
using SPMeta2.Regression.Tests.Utils;


namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class SupportedUICultureScenariousTest : SPMeta2RegresionScenarioTestBase
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

        #region site and list


        [TestMethod]
        [TestCategory("Regression.Scenarios.Localization")]
        public void CanDeploy_SupportedUICulture_ToRootWeb()
        {
            var language1 = ModelGeneratorService.GetRandomDefinition<SupportedUICultureDefinition>();
            var language2 = ModelGeneratorService.GetRandomDefinition<SupportedUICultureDefinition>();
            var language3 = ModelGeneratorService.GetRandomDefinition<SupportedUICultureDefinition>();

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddRootWeb(new RootWebDefinition(), web =>
                {
                    web.AddSupportedUICulture(language1);
                    web.AddSupportedUICulture(language2);
                    web.AddSupportedUICulture(language3);
                });
            });

            TestModel(siteModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Localization")]
        public void CanDeploy_SupportedUICulture_ToSubWeb()
        {
            var language1 = ModelGeneratorService.GetRandomDefinition<SupportedUICultureDefinition>();
            var language2 = ModelGeneratorService.GetRandomDefinition<SupportedUICultureDefinition>();
            var language3 = ModelGeneratorService.GetRandomDefinition<SupportedUICultureDefinition>();

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomWeb(subWeb =>
                {
                    subWeb.AddSupportedUICulture(language1);
                    subWeb.AddSupportedUICulture(language2);
                    subWeb.AddSupportedUICulture(language3);
                });
            });

            TestModel(webModel);
        }

        #endregion
    }
}
