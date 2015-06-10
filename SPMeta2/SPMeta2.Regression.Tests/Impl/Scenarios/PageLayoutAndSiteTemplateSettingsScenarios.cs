using System.Collections.ObjectModel;
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

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class PageLayoutAndSiteTemplateSettingsScenarios : SPMeta2RegresionScenarioTestBase
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

        #region tests

        protected WebDefinition GetPublishingWeb()
        {
            var def = ModelGeneratorService.GetRandomDefinition<WebDefinition>();

            //def.WebTemplate = BuiltInWebTemplates.Publishing.PublishingSite_Intranet;
            def.WebTemplate = BuiltInWebTemplates.Collaboration.TeamSite;

            return def;
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.PageLayoutAndSiteTemplateSettings")]
        public void CanDeploy_PageLayoutAndSiteTemplates_InheritEverything()
        {
            var siteFeature = BuiltInSiteFeatures.SharePointServerPublishingInfrastructure.Inherit(f => f.Enable());

            var webFeature = BuiltInWebFeatures.SharePointServerPublishing.Inherit(f => f.Enable());
            var subWebFeature = BuiltInWebFeatures.SharePointServerPublishing.Inherit(f => f.Enable());

            var settings = new PageLayoutAndSiteTemplateSettingsDefinition
            {
                InheritWebTemplates = true,
                InheritPageLayouts = true,
                InheritDefaultPageLayout = true,
                ConverBlankSpacesIntoHyphen = Rnd.Bool()
            };

            var siteModel = SPMeta2Model.NewSiteModel(site => site.AddSiteFeature(siteFeature));
            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWebFeature(webFeature);
                web.AddWeb(GetPublishingWeb(), subWeb =>
                {
                    subWeb.AddFeature(subWebFeature);
                    subWeb.AddPageLayoutAndSiteTemplateSettings(settings);
                });
            });

            TestModels(new[] { siteModel, webModel });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.PageLayoutAndSiteTemplateSettings")]
        public void CanDeploy_PageLayoutAndSiteTemplates_UseAny()
        {
            var siteFeature = BuiltInSiteFeatures.SharePointServerPublishingInfrastructure.Inherit(f => f.Enable());

            var webFeature = BuiltInWebFeatures.SharePointServerPublishing.Inherit(f => f.Enable());
            var subWebFeature = BuiltInWebFeatures.SharePointServerPublishing.Inherit(f => f.Enable());

            var settings = new PageLayoutAndSiteTemplateSettingsDefinition
            {
                UseAnyWebTemplate = true,
                UseAnyPageLayout = true,
                ConverBlankSpacesIntoHyphen = Rnd.Bool()
            };

            var siteModel = SPMeta2Model.NewSiteModel(site => site.AddSiteFeature(siteFeature));
            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWebFeature(webFeature);
                web.AddWeb(GetPublishingWeb(), subWeb =>
                {
                    subWeb.AddFeature(subWebFeature);
                    subWeb.AddPageLayoutAndSiteTemplateSettings(settings);
                });
            });

            TestModels(new[] { siteModel, webModel });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.PageLayoutAndSiteTemplateSettings")]
        public void CanDeploy_PageLayoutAndSiteTemplates_UseDefined()
        {
            var siteFeature = BuiltInSiteFeatures.SharePointServerPublishingInfrastructure.Inherit(f => f.Enable());

            var webFeature = BuiltInWebFeatures.SharePointServerPublishing.Inherit(f => f.Enable());
            var subWebFeature = BuiltInWebFeatures.SharePointServerPublishing.Inherit(f => f.Enable());

            var settings = new PageLayoutAndSiteTemplateSettingsDefinition
            {
                UseDefinedWebTemplates = true,
                DefinedWebTemplates = new Collection<string>
                {
                    BuiltInWebTemplates.Collaboration.BlankSite,
                    BuiltInWebTemplates.Collaboration.Blog,
                    BuiltInWebTemplates.Collaboration.TeamSite
                },

                UseDefinedPageLayouts = true,
                DefinedPageLayouts = new Collection<string>
                {
                    BuiltInPublishingPageLayoutNames.ArticleLeft,
                    BuiltInPublishingPageLayoutNames.ArticleRight,
                    BuiltInPublishingPageLayoutNames.ArticleLinks
                },

                UseDefinedDefaultPageLayout = true,
                DefinedDefaultPageLayout = BuiltInPublishingPageLayoutNames.ArticleRight,

                ConverBlankSpacesIntoHyphen = Rnd.Bool()
            };

            var siteModel = SPMeta2Model.NewSiteModel(site => site.AddSiteFeature(siteFeature));
            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWebFeature(webFeature);
                web.AddWeb(GetPublishingWeb(), subWeb =>
                {
                    subWeb.AddFeature(subWebFeature);
                    subWeb.AddPageLayoutAndSiteTemplateSettings(settings);
                });
            });

            TestModels(new[] { siteModel, webModel });
        }

        #endregion
    }
}
