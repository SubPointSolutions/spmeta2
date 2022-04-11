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
using SPMeta2.BuiltInDefinitions;

using SPMeta2.Containers.Extensions;

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

        #region subwebs

        [TestMethod]
        [TestCategory("Regression.Scenarios.MasterPageSettings.SubWebs")]
        public void CanDeploy_MasterPageSettings_SystemMasterPageUrl_Only_OnSubWeb()
        {
            var settings = BuiltInDefinitions.BuiltInMasterPageDefinitions.Minimal.Inherit<MasterPageSettingsDefinition>(def =>
            {
                def.SystemMasterPageUrl = BuiltInMasterPageDefinitions.Minimal.SiteMasterPageUrl;
                def.SiteMasterPageUrl = string.Empty;
            });

            var model = SPMeta2Model.NewWebModel(rootWeb =>
            {
                rootWeb.AddRandomWeb(web =>
                {
                    web.AddMasterPageSettings(settings);
                });

            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.MasterPageSettings.SubWebs")]
        public void CanDeploy_MasterPageSettings_SystemMasterPageUrl_Only_OnSubWeb_With_SiteCollectionToken()
        {
            var settings = BuiltInDefinitions.BuiltInMasterPageDefinitions.Minimal.Inherit<MasterPageSettingsDefinition>(def =>
            {
                def.SystemMasterPageUrl = "~sitecollection/_catalogs/masterpage/oslo.master";;
                def.SiteMasterPageUrl = string.Empty;
            });

            var model = SPMeta2Model.NewWebModel(rootWeb =>
            {
                rootWeb.AddRandomWeb(web =>
                {
                    web.AddMasterPageSettings(settings);
                });

            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.MasterPageSettings.SubWebs")]
        public void CanDeploy_MasterPageSettings_SystemMasterPageUrl_Only_OnSubWeb_With_SiteToken()
        {
            var masterPage = ModelGeneratorService.GetRandomDefinition<MasterPageDefinition>(def =>
            {

            });

            var settings = BuiltInDefinitions.BuiltInMasterPageDefinitions.Minimal.Inherit<MasterPageSettingsDefinition>(def =>
            {
                def.SystemMasterPageUrl = "~site/_catalogs/masterpage/" + masterPage.FileName;
                def.SiteMasterPageUrl = string.Empty;
            });

            var model = SPMeta2Model.NewWebModel(rootWeb =>
            {
                rootWeb.AddRandomWeb(web =>
                {
                    web.AddHostList(BuiltInListDefinitions.Catalogs.MasterPage, list =>
                    {
                        list.RegExcludeFromValidation();
                        list.AddMasterPage(masterPage, page =>
                        {
                            page.RegExcludeFromValidation();
                        });
                    });

                    web.AddMasterPageSettings(settings);
                });

            });

            TestModel(model);
        }

        #region SiteMasterPageUrl

        [TestMethod]
        [TestCategory("Regression.Scenarios.MasterPageSettings.SubWebs")]
        public void CanDeploy_MasterPageSettings_SiteMasterPageUrl_Only_OnSubWeb()
        {
            var settings = BuiltInDefinitions.BuiltInMasterPageDefinitions.Minimal.Inherit<MasterPageSettingsDefinition>(def =>
            {
                def.SystemMasterPageUrl = string.Empty;
                def.SiteMasterPageUrl = BuiltInMasterPageDefinitions.Minimal.SiteMasterPageUrl;
            });

            var model = SPMeta2Model.NewWebModel(rootWeb =>
            {
                rootWeb.AddRandomWeb(web =>
                {
                    web.AddMasterPageSettings(settings);
                });

            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.MasterPageSettings.SubWebs")]
        public void CanDeploy_MasterPageSettings_SiteMasterPageUrl_Only_OnSubWeb_With_SiteCollectionToken()
        {
            var settings = BuiltInDefinitions.BuiltInMasterPageDefinitions.Minimal.Inherit<MasterPageSettingsDefinition>(def =>
            {
                def.SystemMasterPageUrl = string.Empty;
                def.SiteMasterPageUrl = "~sitecollection/_catalogs/masterpage/oslo.master";
            });

            var model = SPMeta2Model.NewWebModel(rootWeb =>
            {
                rootWeb.AddRandomWeb(web =>
                {
                    web.AddMasterPageSettings(settings);
                });

            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.MasterPageSettings.SubWebs")]
        public void CanDeploy_MasterPageSettings_SiteMasterPageUrl_Only_OnSubWeb_With_SiteToken()
        {
            var masterPage = ModelGeneratorService.GetRandomDefinition<MasterPageDefinition>(def =>
            {

            });

            var settings = BuiltInDefinitions.BuiltInMasterPageDefinitions.Minimal.Inherit<MasterPageSettingsDefinition>(def =>
            {
                def.SystemMasterPageUrl = string.Empty;
                def.SiteMasterPageUrl = "~site/_catalogs/masterpage/" + masterPage.FileName;
            });

            var model = SPMeta2Model.NewWebModel(rootWeb =>
            {
                rootWeb.AddRandomWeb(web =>
                {
                    web.AddHostList(BuiltInListDefinitions.Catalogs.MasterPage, list =>
                    {
                        list.RegExcludeFromValidation();
                        list.AddMasterPage(masterPage, page =>
                        {
                            page.RegExcludeFromValidation();
                        });
                    });

                    web.AddMasterPageSettings(settings);
                });

            });

            TestModel(model);
        }

        #endregion

        #endregion
    }
}
