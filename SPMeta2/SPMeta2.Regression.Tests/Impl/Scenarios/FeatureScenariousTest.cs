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
using SPMeta2.Syntax.Default.Utils;


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
            var sandboxSolution = new SandboxSolutionDefinition()
            {
                FileName = string.Format("{0}.wsp", Rnd.String()),
                Activate = true,

                SolutionId = new Guid("9f581901-7ed8-4293-9d48-c526ad5a3d7d"),
                Content = ModuleFileUtils.FromResource(GetType().Assembly, "SPMeta2.Regression.Tests.Templates.SandboxSolutions.SPMeta2.Containers.SandboxSolutionContainer.wsp")
            };

            var model = SPMeta2Model
                .NewSiteModel(site =>
                {
                    site.AddSandboxSolution(sandboxSolution);

                    site.AddFeature(new FeatureDefinition
                    {
                        Enable = true,
                        ForceActivate = true,
                        Id = new Guid("167dc650-68d2-4784-bd5b-9c6709db4ac6"),
                        Scope = FeatureDefinitionScope.Site
                    });
                });

            TestModel(model);
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.Features.Sandbox")]
        public void CanDeploy_SandboxWebFeature()
        {
            var sandboxSolution = new SandboxSolutionDefinition()
            {
                FileName = string.Format("{0}.wsp", Rnd.String()),
                Activate = true,

                SolutionId = new Guid("9f581901-7ed8-4293-9d48-c526ad5a3d7d"),
                Content = ModuleFileUtils.FromResource(GetType().Assembly, "SPMeta2.Regression.Tests.Templates.SandboxSolutions.SPMeta2.Containers.SandboxSolutionContainer.wsp")
            };

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
                        Id = new Guid("e4a8e1de-6688-493b-ad7a-d35c0268550c"),
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
