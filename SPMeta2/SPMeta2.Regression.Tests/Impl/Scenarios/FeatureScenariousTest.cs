using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Syntax.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Containers.DefinitionGenerators;
using SPMeta2.Syntax.Default.Utils;
using SPMeta2.Containers.Consts;


namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class FeatureScenariousTest : SPMeta2RegresionScenarioTestBase
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

        #region sandbox features

        [TestMethod]
        [TestCategory("Regression.Scenarios.Features.Sandbox")]
        public void CanDeploy_SandboxSiteFeature()
        {
            var sandboxSolution = new SandboxSolutionDefinitionGenerator().GenerateRandomDefinition() as SandboxSolutionDefinition;

            var model = SPMeta2Model
                .NewSiteModel(site =>
                {
                    site.AddSandboxSolution(sandboxSolution);

                    site.AddFeature(new FeatureDefinition
                    {
                        Enable = true,
                        ForceActivate = true,
                        Id = DefaultContainers.Sandbox.SiteFeatureId,
                        Scope = FeatureDefinitionScope.Site
                    });
                });

            TestModel(model);
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.Features.Sandbox")]
        public void CanDeploy_SandboxWebFeature()
        {
            var sandboxSolution = new SandboxSolutionDefinitionGenerator().GenerateRandomDefinition() as SandboxSolutionDefinition;

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSandboxSolution(sandboxSolution);
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddFeature(new FeatureDefinition
                    {
                        Enable = true,
                        ForceActivate = true,
                        Id = DefaultContainers.Sandbox.WebFeatureId,
                        Scope = FeatureDefinitionScope.Web
                    });
                });

            TestModel(siteModel, model);
        }


        #endregion

        #region default

        [TestMethod]
        [TestCategory("Regression.Scenarios.Features")]
        public void CanDeploy_FarmFeature()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var model = SPMeta2Model
                   .NewFarmModel(farm =>
                   {
                       farm.AddFeature(new FeatureDefinition
                       {
                           Enable = true,
                           ForceActivate = true,
                           Id = BuiltInFarmFeatures.DataConnectionLibrary.Id,
                           Scope = FeatureDefinitionScope.Farm
                       });
                   });

                TestModel(model);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Features")]
        public void CanDeploy_WebApplicationFeature()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var model = SPMeta2Model
                    .NewWebApplicationModel(webApp =>
                    {
                        webApp.AddFeature(new FeatureDefinition
                        {
                            Enable = true,
                            ForceActivate = true,
                            Id = BuiltInWebApplicationFeatures.DocumentSetsMetadataSynchronization.Id,
                            Scope = FeatureDefinitionScope.WebApplication
                        });
                    });

                TestModel(model);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Features")]
        public void CanDeploy_SiteFeature()
        {
            var model = SPMeta2Model
                .NewSiteModel(site =>
                {
                    site.AddFeature(new FeatureDefinition
                    {
                        Enable = true,
                        ForceActivate = true,
                        Id = BuiltInSiteFeatures.DocumentIDService.Id,
                        Scope = FeatureDefinitionScope.Site
                    });
                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Features")]
        public void CanDeploy_WebFeature()
        {
            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddFeature(new FeatureDefinition
                    {
                        Enable = true,
                        ForceActivate = true,
                        Id = BuiltInWebFeatures.MinimalDownloadStrategy.Id,
                        Scope = FeatureDefinitionScope.Web
                    });
                });

            TestModel(model);
        }


        #endregion
    }
}
