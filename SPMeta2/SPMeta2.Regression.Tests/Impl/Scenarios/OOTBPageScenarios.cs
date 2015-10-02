using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Containers.Exceptions;
using SPMeta2.Definitions;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;
using SPMeta2.Models;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Enumerations;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Modern;
using SPMeta2.Definitions.Fields;
using SPMeta2.Standard.BuiltInDefinitions;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class OOTBPageScenarios : SPMeta2RegresionScenarioTestBase
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

        #region wiki pages

        [TestMethod]
        [TestCategory("Regression.Scenarios.OOTBPages")]
        public void CanDeploy_WebParts_To_OOTB_PublishingPages()
        {
            var sitePublishingFeature = BuiltInSiteFeatures.SharePointServerPublishingInfrastructure.Inherit(f => f.Enable());
            var webPublishingFeature = BuiltInWebFeatures.SharePointServerPublishing.Inherit(f => f.Enable());

            var siteModel = SPMeta2Model.NewSiteModel(site => site.AddSiteFeature(sitePublishingFeature));
            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomWeb(subWeb =>
                {
                    var webDef = subWeb.Value as WebDefinition;
                    webDef.WebTemplate = BuiltInWebTemplates.Publishing.PublishingSite_Intranet;

                    subWeb.AddWebFeature(webPublishingFeature);

                    subWeb.AddHostList(BuiltInListDefinitions.Pages.Inherit(), list =>
                    {
                        list.AddHostPublishingPage(BuiltInPublishingPages.Default, page =>
                        {
                            page.AddRandomWebpart();
                        });
                    });
                });
            });

            TestModels(new ModelNode[] { siteModel, webModel });
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.OOTBPages")]
        public void CanDeploy_WebParts_To_OOTB_WikiPages()
        {
            var sitePublishingFeature = BuiltInSiteFeatures.SharePointServerPublishingInfrastructure.Inherit(f => f.Enable());

            var webPublishingFeature = BuiltInWebFeatures.SharePointServerPublishing.Inherit(f => f.Enable());
            var wikiPages = BuiltInWebFeatures.WikiPageHomePage.Inherit(f => f.Enable());

            var siteModel = SPMeta2Model.NewSiteModel(site => site.AddSiteFeature(sitePublishingFeature));
            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomWeb(subWeb =>
                {
                    subWeb.AddWebFeature(webPublishingFeature);
                    subWeb.AddWebFeature(wikiPages);

                    subWeb.AddHostList(BuiltInListDefinitions.SitePages, list =>
                    {
                        list.AddHostWikiPage(BuiltInWikiPages.Home, page =>
                        {
                            page.AddRandomWebpart();
                        });

                        list.AddHostWikiPage(BuiltInWikiPages.HowToUserThisLibrary, page =>
                        {
                            page.AddRandomWebpart();
                        });
                    });
                });
            });

            TestModels(new ModelNode[] { siteModel, webModel });
        }

        #endregion


    }
}
