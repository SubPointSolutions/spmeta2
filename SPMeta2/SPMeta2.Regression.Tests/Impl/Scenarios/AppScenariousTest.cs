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
using SPMeta2.Regression.Utils;


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
            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSiteFeature(BuiltInSiteFeatures.EnableAppSideLoading.Inherit(def =>
                {
                    def.Enable();
                }));
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomWeb(subWeb =>
                {
                    web.AddRandomApp();
                });
            });

            TestModel(siteModel, model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Apps")]
        public void CanDeploy_App_ToWebHierarchy()
        {
            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSiteFeature(BuiltInSiteFeatures.EnableAppSideLoading.Inherit(def =>
                {
                    def.Enable();
                }));
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                //web.AddRandomApp();

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

            TestModel(siteModel, model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Apps.ClientWebPart")]
        public void CanDeploy_AppClientWebPart_ToWebAndSubWebPages()
        {
            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSiteFeature(BuiltInSiteFeatures.EnableAppSideLoading.Inherit(def =>
                {
                    def.Enable();
                }));

                site.AddSiteFeature(BuiltInSiteFeatures.SharePointServerPublishingInfrastructure.Inherit(def =>
                {
                    def.Enable = true;
                }));
            });

            var webPublishing = BuiltInWebFeatures.SharePointServerPublishing.Inherit(def =>
            {
                def.Enable = true;
            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWebFeature(webPublishing.Inherit());

                web.AddRandomApp();
                FillClientWebPartPages(web);

                // subwebs

                web.AddRandomWeb(subWeb1 =>
                {
                    subWeb1.AddWebFeature(webPublishing.Inherit());
                    subWeb1.AddRandomApp();

                    FillClientWebPartPages(subWeb1);

                    subWeb1.AddRandomWeb(subWeb12 =>
                    {
                        subWeb12.AddWebFeature(webPublishing.Inherit());
                        subWeb12.AddRandomApp();

                        FillClientWebPartPages(subWeb12);

                        subWeb12.AddRandomWeb(subWeb123 =>
                        {
                            subWeb123.AddWebFeature(webPublishing.Inherit());
                            subWeb123.AddRandomApp();

                            FillClientWebPartPages(subWeb123);
                        });
                    });
                });
            });

            TestModel(siteModel, webModel);
        }


        #endregion

        #region upgradability

        [TestMethod]
        [TestCategory("Regression.Scenarios.Apps.Upgradability")]
        public void CanDeploy_App_ToWeb_Upgrade_0123_Versions()
        {
            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSiteFeature(BuiltInSiteFeatures.EnableAppSideLoading.Inherit(def =>
                {
                    def.Enable();
                }));
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomWeb(appWeb =>
                {
                    // v0
                    appWeb.AddRandomApp();

                    // v1
                    AddAndCheckRandomAppWithVersion(appWeb, new Version(1, 0, 0, 1));

                    // v2
                    AddAndCheckRandomAppWithVersion(appWeb, new Version(1, 0, 0, 2));

                    // v3
                    AddAndCheckRandomAppWithVersion(appWeb, new Version(1, 0, 0, 3));
                });


            });

            TestModel(siteModel, model);
        }

        private void AddAndCheckRandomAppWithVersion(WebModelNode web, Version version)
        {
            var def = new AppDefinition();

            def.ProductId = DefaultContainers.Apps.ProductId;
            def.Version = version.ToString();

            def.Content = File.ReadAllBytes(string.Format(DefaultContainers.Apps.GenericVersionableAppFilePath, version));

            web.AddApp(def, app =>
            {
                app.OnProvisioned<object, AppDefinition>(context =>
                {
                    RegressionUtils.WriteLine(context.ObjectDefinition.ToString());
                    RegressionUtils.WriteLine(context.Object.ToString());

                    var expectedAppVersion = new Version(context.ObjectDefinition.Version);

                    var obj = context.Object;
                    var objType = context.Object.GetType();

                    if (objType.ToString().Contains("Microsoft.SharePoint.Client.AppInstance"))
                    {
                        // with CSOM there is no API to know current app installed version
                        // checking if app is Installed after every single update
                        var appStatus = obj.GetPropertyValue("Status").ToString();

                        Assert.IsTrue(appStatus == "Installed",
                            string.Format("App should be installed after every update"));
                    }
                    else if (objType.ToString().Contains("Microsoft.SharePoint.Administration.SPAppInstance"))
                    {
                        var appObjet = obj.GetPropertyValue("App");

                        var versionString = appObjet.GetPropertyValue("VersionString") as string;
                        var spAppVersion = new Version(versionString);

                        // either equal (update) or SharePoint version greater than local (second update)
                        // the test is run several times, so only once we have =, and then we have <
                        Assert.IsTrue(expectedAppVersion <= spAppVersion,
                            string.Format("Expecting app version:[{0}] SharePoint app version:[{1}]", expectedAppVersion, spAppVersion));
                    }
                    else
                    {
                        throw new SPMeta2NotImplementedException(string.Format("ID property extraction is not implemented for type: [{0}]", objType));
                    }
                });
            });
        }

        #endregion

        #region utils

        protected void FillClientWebPartPages(WebModelNode web)
        {
            web.AddHostPagesList(list =>
            {
                list.AddRandomPublishingPage(page =>
                {
                    page.AddRandomWebpart();
                });
            });

            web.AddHostSitePagesList(list =>
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
        }

        #endregion
    }
}
