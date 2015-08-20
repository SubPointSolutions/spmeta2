using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Syntax.Default;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers.Standard;
using SPMeta2.Models;


namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class AppScenariousTest : SPMeta2RegresionScenarioTestBase
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
        [TestCategory("Regression.Scenarios.Apps")]
        public void CanDeploy_App_ToWeb()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomApp();
            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Apps")]
        public void CanDeploy_App_ToWebHierarchy()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomApp();

                web.AddRandomWeb(subWeb1 =>
                {
                    subWeb1.AddRandomApp();

                    subWeb1.AddRandomWeb(subWeb12 =>
                    {
                        subWeb12.AddRandomApp();

                        subWeb12.AddRandomWeb(subWeb123 =>
                        {
                            subWeb123.AddRandomApp();
                        });
                    });
                });
            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Apps.ClientWebPart")]
        public void CanDeploy_AppClientWebPart_ToPages()
        {
            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSiteFeature(BuiltInSiteFeatures.SharePointServerPublishingInfrastructure.Inherit(def =>
                {
                    def.Enable = true;
                }));
            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWebFeature(BuiltInWebFeatures.SharePointServerPublishing.Inherit(def =>
                {
                    def.Enable = true;
                }));

                web.AddRandomApp();

                web.AddList(BuiltInListDefinitions.Pages, list =>
                {
                    list.AddRandomPublishingPage(page =>
                    {
                        page.AddRandomWebpart();
                    });
                });

                web.AddList(BuiltInListDefinitions.SitePages, list =>
                {
                    list.AddRandomWikiPage(page =>
                    {
                        page.AddRandomWebpart();
                    });

                    list.AddRandomWebPartPage(page =>
                    {
                        page.AddRandomWebpart();
                    });
                });
            });

            TestModel(siteModel, webModel);
        }


        #endregion
    }
}
