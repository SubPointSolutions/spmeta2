using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Containers.Services;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Syntax.Default;
using System.IO;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class SiteScenariousTest : SPMeta2RegresionScenarioTestBase
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
        [TestCategory("Regression.Scenarios.Sites")]
        public void CanDeploy_Simple_Site_WithNullableSecondaryContactLogin()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var site = ModelGeneratorService.GetRandomDefinition<SiteDefinition>();

                //site.PrefixName = string.Empty;

                site.SecondaryContactEmail = null;
                site.SecondaryContactLogin = null;
                site.SecondaryContactName = null;

                var model = SPMeta2Model.NewWebApplicationModel(webApplication =>
                {
                    webApplication.AddSite(site);
                });

                TestModel(model);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Sites")]
        public void CanDeploy_Simple_Site()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var site = ModelGeneratorService.GetRandomDefinition<SiteDefinition>();

                site.PrefixName = string.Empty;

                var model = SPMeta2Model.NewWebApplicationModel(webApplication =>
                {
                    webApplication.AddSite(site);
                });

                TestModel(model);
            });
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.Sites")]
        public void CanDeploy_Simple_Site_UnderSitesManagedPath()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var site = ModelGeneratorService.GetRandomDefinition<SiteDefinition>(def =>
                {
                    def.PrefixName = "sites";
                });

                var model = SPMeta2Model.NewWebApplicationModel(webApplication =>
                {
                    webApplication.AddSite(site);
                });

                TestModel(model);
            });
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.Sites")]
        public void CanDeploy_Simple_Site_UnderRandomManagedPath()
        {
            // /some-path/some-site

            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var managedPath = ModelGeneratorService.GetRandomDefinition<PrefixDefinition>(def =>
                {
                    def.PrefixType = "WildcardInclusion";
                });

                var site = ModelGeneratorService.GetRandomDefinition<SiteDefinition>(def =>
                {
                    def.PrefixName = managedPath.Path;
                });

                var model = SPMeta2Model.NewWebApplicationModel(webApplication =>
                {
                    webApplication.AddPrefix(managedPath);
                    webApplication.AddSite(site);
                });

                TestModel(model);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Sites")]
        public void CanDeploy_Simple_Site_UnderRandomManagedPath_AsRoot()
        {
            // /some-path/

            // Enhance SiteDefinition provision - enable provision under the managed path  #853 
            // https://github.com/SubPointSolutions/spmeta2/issues/853

            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var managedPath = ModelGeneratorService.GetRandomDefinition<PrefixDefinition>(def =>
                {
                    def.PrefixType = BuiltInPrefixTypes.ExplicitInclusion;
                });

                var site = ModelGeneratorService.GetRandomDefinition<SiteDefinition>(def =>
                {
                    def.Url = "/";
                    def.PrefixName = managedPath.Path;
                });

                var model = SPMeta2Model.NewWebApplicationModel(webApplication =>
                {
                    webApplication.AddPrefix(managedPath);
                    webApplication.AddSite(site);
                });

                TestModel(model);
            });
        }

        #endregion
    }
}
