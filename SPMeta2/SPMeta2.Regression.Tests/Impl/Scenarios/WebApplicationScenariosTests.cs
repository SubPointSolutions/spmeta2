using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers.Consts;
using SPMeta2.Containers.Services;
using SPMeta2.Containers.Utils;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Definitions;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Syntax.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class WebApplicationScenariosTests : SPMeta2RegresionScenarioTestBase
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

        protected virtual int GetWebApplicationPort()
        {
            var webUrls = RunnerEnvironmentUtils.GetEnvironmentVariables(EnvironmentConsts.SSOM_WebApplicationUrls);
            var webUrl = webUrls.First();

            var webPort = webUrl.Split(':').Last();

            return int.Parse(webPort);
        }

        #region default

        [TestMethod]
        [TestCategory("Regression.Scenarios.WebApplicationScenariosTests")]
        public void CanDeploy_Existing_WebApplication()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var webAppDefinition = ModelGeneratorService.GetRandomDefinition<WebApplicationDefinition>(def =>
                {
                    def.Port = GetWebApplicationPort();
                    def.CreateNewDatabase = false;
                });

                var model = SPMeta2Model.NewFarmModel(farm =>
                {
                    farm.AddWebApplication(webAppDefinition);
                });

                TestModel(model);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.WebApplicationScenariosTests")]
        public void CanDeploy_Existing_WebApplication_With_AllowedInlineDownloadedMimeTypes()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var webAppDefinition = ModelGeneratorService.GetRandomDefinition<WebApplicationDefinition>(def =>
                {
                    def.Port = GetWebApplicationPort();

                    def.CreateNewDatabase = false;

                    def.ShouldOverrideAllowedInlineDownloadedMimeTypes = false;

                    def.AllowedInlineDownloadedMimeTypes.Add("text/html-" + Rnd.Int());
                    def.AllowedInlineDownloadedMimeTypes.Add("text/javascript-" + +Rnd.Int());
                });

                var model = SPMeta2Model.NewFarmModel(farm =>
                {
                    farm.AddWebApplication(webAppDefinition);
                });

                TestModel(model);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.WebApplicationScenariosTests")]
        public void CanDeploy_Existing_WebApplication_With_AllowedInlineDownloadedMimeTypes_Reset()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                // reset everyting
                var webAppDefinitionResetValues = ModelGeneratorService.GetRandomDefinition<WebApplicationDefinition>(def =>
                {
                    def.Port = GetWebApplicationPort();

                    def.CreateNewDatabase = false;

                    def.ShouldOverrideAllowedInlineDownloadedMimeTypes = true;

                    def.AllowedInlineDownloadedMimeTypes.Add("text/reset-" + Rnd.Int());
                });

                var resetValuesModel = SPMeta2Model.NewFarmModel(farm =>
                {
                    farm.AddWebApplication(webAppDefinitionResetValues);
                });

                // bring everyting to 1
                TestModel(resetValuesModel);

                var webAppDefinitionInitialValues = ModelGeneratorService.GetRandomDefinition<WebApplicationDefinition>(def =>
                {
                    def.Port = GetWebApplicationPort();

                    def.CreateNewDatabase = false;

                    def.ShouldOverrideAllowedInlineDownloadedMimeTypes = false;

                    def.AllowedInlineDownloadedMimeTypes.Add("text/html-" + Rnd.Int());
                    def.AllowedInlineDownloadedMimeTypes.Add("text/javascript-" + +Rnd.Int());
                });

                var initialValuesModel = SPMeta2Model.NewFarmModel(farm =>
                {
                    farm.AddWebApplication(webAppDefinitionInitialValues);
                });

                // add more
                TestModel(initialValuesModel);

                var webAppDefinitionOverwrittemlValues = ModelGeneratorService.GetRandomDefinition<WebApplicationDefinition>(def =>
                {
                    def.Port = GetWebApplicationPort();

                    def.CreateNewDatabase = false;

                    def.ShouldOverrideAllowedInlineDownloadedMimeTypes = true;

                    def.AllowedInlineDownloadedMimeTypes.Add("text/html-" + Rnd.Int());
                    def.AllowedInlineDownloadedMimeTypes.Add("text/javascript-" + +Rnd.Int());
                    def.AllowedInlineDownloadedMimeTypes.Add("text/overwrite-" + +Rnd.Int());
                });

                var overwrittenValuesModel = SPMeta2Model.NewFarmModel(farm =>
                {
                    farm.AddWebApplication(webAppDefinitionOverwrittemlValues);
                });

                // overwrite again
                TestModel(overwrittenValuesModel);
            });
        }

        #endregion
    }
}
