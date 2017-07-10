using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Containers.Standard;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Regression.Tests.Definitions;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Standard.Definitions;
using SPMeta2.Syntax.Default;
using SPMeta2.Validation.Validators.Relationships;
using SPMeta2.Containers.Consts;
using System.IO;

using SPMeta2.Containers.Extensions;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class FarmSolutionScenariosTest : SPMeta2RegresionScenarioTestBase
    {
        #region constructors

        public FarmSolutionScenariosTest()
        {
            RegressionService.ProvisionGenerationCount = 1;
            RegressionService.ShowOnlyFalseResults = true;
        }

        #endregion

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
        [TestCategory("Regression.Scenarios.FarmSolution.Farm")]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm.Default")]
        public void CanDeploy_FarmSolution_As_Default()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = GetFarmSolutionDefinition(false, def =>
                {

                });

                TestFarmSolutionModel(solutionDef, false);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication")]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication.Default")]
        public void CanDeploy_FarmSolution_As_Default_UnderWebApplication()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = GetFarmSolutionDefinition(true, def =>
                {

                });

                TestFarmSolutionModel(solutionDef, true);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication")]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication.Default")]
        public void CanDeploy_FarmSolution_As_Default_UnderTwoWebApplication()
        {
            // the same wsp package is to be deployed under different web app
            // checking that the same *.wsp package can be sfaely deployed under two web app
            // such deployment and retraction should only be scoped to a particular web app not affecting other web app deployments

            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var webApp1 = ModelGeneratorService.GetRandomDefinition<WebApplicationDefinition>(def =>
                {
                    def.Port = 31401;
                    def.UseSecureSocketsLayer = false;
                });

                var webApp2 = ModelGeneratorService.GetRandomDefinition<WebApplicationDefinition>(def =>
                {
                    def.Port = 31402;
                    def.UseSecureSocketsLayer = false;
                });

                var solutionDef1 = GetFarmSolutionDefinition(true, def =>
                {
                    //def.ShouldRetract = true;
                    def.ShouldDeploy = true;
                });

                var solutionDef2 = GetFarmSolutionDefinition(true, def =>
                {
                    // the same wsp package is to be deployed under different web app
                    def.FileName = solutionDef1.FileName;

                    //def.ShouldRetract = true;
                    def.ShouldDeploy = true;
                });

                var farmModel = SPMeta2Model.NewFarmModel(farm =>
                {
                    farm.AddWebApplication(webApp1, webApp =>
                    {
                        webApp.RegExcludeFromValidation();

                        webApp.AddFarmSolution(solutionDef1, solution =>
                        {

                        });
                    });


                    farm.AddWebApplication(webApp2, webApp =>
                    {
                        webApp.RegExcludeFromValidation();

                        webApp.AddFarmSolution(solutionDef2, solution =>
                        {

                        });
                    });

                });

                TestModel(farmModel);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication")]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication.Default")]
        public void CanDeploy_FarmSolution_As_Upgrade_UnderTwoWebApplication()
        {
            // the same wsp package is to be deployed under different web app
            // checking that the same *.wsp package can be sfaely deployed under two web app
            // such deployment and retraction should only be scoped to a particular web app not affecting other web app deployments

            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var webApp1 = ModelGeneratorService.GetRandomDefinition<WebApplicationDefinition>(def =>
                {
                    def.Port = 31401;
                    def.UseSecureSocketsLayer = false;
                });

                var webApp2 = ModelGeneratorService.GetRandomDefinition<WebApplicationDefinition>(def =>
                {
                    def.Port = 31402;
                    def.UseSecureSocketsLayer = false;
                });

                var solutionDef1 = GetFarmSolutionDefinition(true, def =>
                {
                    def.ShouldUpgrade = true;
                    def.ShouldDeploy = true;
                });

                var solutionDef2 = GetFarmSolutionDefinition(true, def =>
                {
                    // the same wsp package is to be deployed under different web app
                    def.FileName = solutionDef1.FileName;

                    def.ShouldUpgrade = true;
                    def.ShouldDeploy = true;
                });

                var farmModel = SPMeta2Model.NewFarmModel(farm =>
                {
                    farm.AddWebApplication(webApp1, webApp =>
                    {
                        webApp.RegExcludeFromValidation();

                        webApp.AddFarmSolution(solutionDef1, solution =>
                        {

                        });
                    });


                    farm.AddWebApplication(webApp2, webApp =>
                    {
                        webApp.RegExcludeFromValidation();

                        webApp.AddFarmSolution(solutionDef2, solution =>
                        {

                        });
                    });

                });

                TestModel(farmModel);
            });
        }

        #endregion

        #region add operations

        public void CanDeploy_FarmSolution_As_Add_FromDeleted_State_Internal(bool isWebApplication)
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = GetFarmSolutionDefinition(isWebApplication, def =>
                {

                });

                PrepareDeletedState(solutionDef, isWebApplication);
                TestFarmSolutionModel(solutionDef, isWebApplication);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm")]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm.Add")]
        public void CanDeploy_FarmSolution_As_Add_FromDeleted_State()
        {
            CanDeploy_FarmSolution_As_Add_FromDeleted_State_Internal(false);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication")]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication.Add")]
        public void CanDeploy_FarmSolution_As_Add_FromDeleted_State_UnderWebApplication()
        {
            CanDeploy_FarmSolution_As_Add_FromDeleted_State_Internal(true);
        }

        protected FarmSolutionDefinition GetFarmSolutionDefinition(bool isWebApplication, Action<FarmSolutionDefinition> action)
        {
            var solutionDef = ModelGeneratorService.GetRandomDefinition<FarmSolutionDefinition>(def =>
            {
                action(def);
            });

            if (isWebApplication)
            {
                solutionDef.SolutionId = DefaultContainers.FarmSolutionWebScope.SolutionId;
                solutionDef.Content = File.ReadAllBytes(DefaultContainers.FarmSolutionWebScope.FilePath);
            }

            return solutionDef;
        }

        public void CanDeploy_FarmSolution_As_Add_FromRetracted_State_Internal(bool isWebApplication)
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = GetFarmSolutionDefinition(isWebApplication, def =>
                {

                });

                PrepareRetractedState(solutionDef, isWebApplication);
                TestFarmSolutionModel(solutionDef, isWebApplication);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm")]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm.Add")]
        public void CanDeploy_FarmSolution_As_Add_FromRetracted_State()
        {
            CanDeploy_FarmSolution_As_Add_FromRetracted_State_Internal(false);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication")]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication.Add")]
        public void CanDeploy_FarmSolution_As_Add_FromRetracted_State_UnderWebApplication()
        {
            CanDeploy_FarmSolution_As_Add_FromRetracted_State_Internal(true);
        }

        public void CanDeploy_FarmSolution_As_Add_FromAdded_State_Internal(bool isWebApplication)
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = GetFarmSolutionDefinition(isWebApplication, def =>
                {

                });

                PrepareAddedState(solutionDef, isWebApplication);
                TestFarmSolutionModel(solutionDef, isWebApplication);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm")]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm.Add")]
        public void CanDeploy_FarmSolution_As_Add_FromAdded_State()
        {
            CanDeploy_FarmSolution_As_Add_FromAdded_State_Internal(false);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication")]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication.Add")]
        public void CanDeploy_FarmSolution_As_Add_FromAdded_State_UnderWebApplication()
        {
            CanDeploy_FarmSolution_As_Add_FromAdded_State_Internal(true);
        }

        public void CanDeploy_FarmSolution_As_Add_FromDeployed_State_Internal(bool isWebApplication)
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = GetFarmSolutionDefinition(isWebApplication, def =>
                {

                });

                PrepareDeployedState(solutionDef, isWebApplication);
                TestFarmSolutionModel(solutionDef, isWebApplication);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm")]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm.Add")]
        public void CanDeploy_FarmSolution_As_Add_FromDeployed_State()
        {
            CanDeploy_FarmSolution_As_Add_FromDeployed_State_Internal(false);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication")]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication.Add")]
        public void CanDeploy_FarmSolution_As_Add_FromDeployed_State_UnderWebApplication()
        {
            CanDeploy_FarmSolution_As_Add_FromDeployed_State_Internal(true);
        }

        #endregion

        #region delete operations

        public void CanDeploy_FarmSolution_As_Delete_FromDeleted_State_Internal(bool isWebApplication)
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = GetFarmSolutionDefinition(isWebApplication, def =>
                {
                    def.ShouldDelete = true;
                    def.ShouldAdd = false;
                });

                PrepareDeletedState(solutionDef, isWebApplication);
                TestFarmSolutionModel(solutionDef, isWebApplication);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm")]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm.Delete")]
        public void CanDeploy_FarmSolution_As_Delete_FromDeleted_State()
        {
            CanDeploy_FarmSolution_As_Delete_FromDeleted_State_Internal(false);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication")]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication.Delete")]
        public void CanDeploy_FarmSolution_As_Delete_FromDeleted_State_UnderWebApplication()
        {
            CanDeploy_FarmSolution_As_Delete_FromDeleted_State_Internal(true);
        }

        public void CanDeploy_FarmSolution_As_Delete_FromRetracted_State_Internal(bool isWebApplication)
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = GetFarmSolutionDefinition(isWebApplication, def =>
                {
                    def.ShouldDelete = true;
                    def.ShouldAdd = false;
                });

                PrepareRetractedState(solutionDef, isWebApplication);
                TestFarmSolutionModel(solutionDef, isWebApplication);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm")]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm.Delete")]
        public void CanDeploy_FarmSolution_As_Delete_FromRetracted_State()
        {
            CanDeploy_FarmSolution_As_Delete_FromRetracted_State_Internal(false);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication")]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication.Delete")]
        public void CanDeploy_FarmSolution_As_Delete_FromRetracted_State_UnderWebApplication()
        {
            CanDeploy_FarmSolution_As_Delete_FromRetracted_State_Internal(true);
        }

        public void CanDeploy_FarmSolution_As_Delete_FromAdded_State_Internal(bool isWebApplication)
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = GetFarmSolutionDefinition(isWebApplication, def =>
                {
                    def.ShouldDelete = true;
                    def.ShouldAdd = false;
                });

                PrepareAddedState(solutionDef, isWebApplication);
                TestFarmSolutionModel(solutionDef, isWebApplication);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm")]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm.Delete")]
        public void CanDeploy_FarmSolution_As_Delete_FromAdded_State()
        {
            CanDeploy_FarmSolution_As_Delete_FromAdded_State_Internal(false);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication")]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication.Delete")]
        public void CanDeploy_FarmSolution_As_Delete_FromAdded_State_UnderWebApplication()
        {
            CanDeploy_FarmSolution_As_Delete_FromAdded_State_Internal(true);
        }

        public void CanDeploy_FarmSolution_As_Delete_FromDeployed_State_Internal(bool isWebApplication)
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = GetFarmSolutionDefinition(isWebApplication, def =>
                {
                    def.ShouldDelete = true;
                    def.ShouldAdd = false;
                });

                PrepareDeployedState(solutionDef, isWebApplication);
                TestFarmSolutionModel(solutionDef, isWebApplication);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm")]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm.Delete")]
        public void CanDeploy_FarmSolution_As_Delete_FromDeployed_State()
        {
            CanDeploy_FarmSolution_As_Delete_FromDeployed_State_Internal(false);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication")]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication.Delete")]
        public void CanDeploy_FarmSolution_As_Delete_FromDeployed_State_UnderWebApplication()
        {
            CanDeploy_FarmSolution_As_Delete_FromDeployed_State_Internal(true);
        }

        #endregion

        #region retract operations

        public void CanDeploy_FarmSolution_As_Retract_FromDeleted_State_Internal(bool isWebApplication)
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = GetFarmSolutionDefinition(isWebApplication, def =>
                {
                    def.ShouldRetract = true;
                });

                PrepareDeletedState(solutionDef, isWebApplication);
                TestFarmSolutionModel(solutionDef, isWebApplication);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm")]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm.Retract")]
        public void CanDeploy_FarmSolution_As_Retract_FromDeleted_State()
        {
            CanDeploy_FarmSolution_As_Retract_FromDeleted_State_Internal(false);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication")]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication.Retract")]
        public void CanDeploy_FarmSolution_As_Retract_FromDeleted_State_UnderWebApplication()
        {
            CanDeploy_FarmSolution_As_Retract_FromDeleted_State_Internal(true);
        }

        public void CanDeploy_FarmSolution_As_Retract_FromRetracted_State_Internal(bool isWebApplication)
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = GetFarmSolutionDefinition(isWebApplication, def =>
                {
                    def.ShouldRetract = true;
                });

                PrepareRetractedState(solutionDef, isWebApplication);
                TestFarmSolutionModel(solutionDef, isWebApplication);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm")]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm.Retract")]
        public void CanDeploy_FarmSolution_As_Retract_FromRetracted_State()
        {
            CanDeploy_FarmSolution_As_Retract_FromRetracted_State_Internal(false);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication")]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication.Retract")]
        public void CanDeploy_FarmSolution_As_Retract_FromRetracted_State_UnderWbApplication()
        {
            CanDeploy_FarmSolution_As_Retract_FromRetracted_State_Internal(true);
        }

        public void CanDeploy_FarmSolution_As_Retract_FromAdded_State_Internal(bool isWebApplication)
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = GetFarmSolutionDefinition(isWebApplication, def =>
                {
                    def.ShouldRetract = true;
                });

                PrepareAddedState(solutionDef, isWebApplication);
                TestFarmSolutionModel(solutionDef, isWebApplication);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm")]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm.Retract")]
        public void CanDeploy_FarmSolution_As_Retract_FromAdded_State()
        {
            CanDeploy_FarmSolution_As_Retract_FromAdded_State_Internal(false);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication")]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication.Retract")]
        public void CanDeploy_FarmSolution_As_Retract_FromAdded_State_UnderWebApplication()
        {
            CanDeploy_FarmSolution_As_Retract_FromAdded_State_Internal(true);
        }

        public void CanDeploy_FarmSolution_As_Retract_FromDeployed_State_Internal(bool isWebApplication)
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = GetFarmSolutionDefinition(isWebApplication, def =>
                {
                    def.ShouldRetract = true;
                });

                PrepareDeployedState(solutionDef, isWebApplication);
                TestFarmSolutionModel(solutionDef, isWebApplication);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm")]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm.Retract")]
        public void CanDeploy_FarmSolution_As_Retract_FromDeployed_State()
        {
            CanDeploy_FarmSolution_As_Retract_FromDeployed_State_Internal(false);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication")]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication.Retract")]
        public void CanDeploy_FarmSolution_As_Retract_FromDeployed_State_UnderWebApplication()
        {
            CanDeploy_FarmSolution_As_Retract_FromDeployed_State_Internal(true);
        }

        #endregion


        #region deploy operations

        public void CanDeploy_FarmSolution_As_Deploy_FromDeleted_State_Internal(bool isWebApplication)
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = GetFarmSolutionDefinition(isWebApplication, def =>
                {
                    def.ShouldDeploy = true;
                });

                PrepareDeletedState(solutionDef, isWebApplication);
                TestFarmSolutionModel(solutionDef, isWebApplication);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm")]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm.Deploy")]
        public void CanDeploy_FarmSolution_As_Deploy_FromDeleted_State()
        {
            CanDeploy_FarmSolution_As_Deploy_FromDeleted_State_Internal(false);
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication")]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication.Deploy")]
        public void CanDeploy_FarmSolution_As_Deploy_FromDeleted_State_UnderWebApplication()
        {
            CanDeploy_FarmSolution_As_Deploy_FromDeleted_State_Internal(true);
        }

        public void CanDeploy_FarmSolution_As_Deploy_FromRetracted_State_Internal(bool isWebApplication)
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = GetFarmSolutionDefinition(isWebApplication, def =>
                {
                    def.ShouldDeploy = true;
                });

                PrepareRetractedState(solutionDef, isWebApplication);
                TestFarmSolutionModel(solutionDef, isWebApplication);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm")]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm.Deploy")]
        public void CanDeploy_FarmSolution_As_Deploy_FromRetracted_State()
        {
            CanDeploy_FarmSolution_As_Deploy_FromRetracted_State_Internal(false);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication")]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication.Deploy")]
        public void CanDeploy_FarmSolution_As_Deploy_FromRetracted_State_UnderWebApplication()
        {
            CanDeploy_FarmSolution_As_Deploy_FromRetracted_State_Internal(true);
        }

        public void CanDeploy_FarmSolution_As_Deploy_FromAdded_State_Internal(bool isWebApplication)
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = GetFarmSolutionDefinition(isWebApplication, def =>
                {
                    def.ShouldDeploy = true;
                });

                PrepareAddedState(solutionDef, isWebApplication);
                TestFarmSolutionModel(solutionDef, isWebApplication);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm")]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm.Deploy")]
        public void CanDeploy_FarmSolution_As_Deploy_FromAdded_State()
        {
            CanDeploy_FarmSolution_As_Deploy_FromAdded_State_Internal(false);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication")]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication.Deploy")]
        public void CanDeploy_FarmSolution_As_Deploy_FromAdded_State_UnderWebApplication()
        {
            CanDeploy_FarmSolution_As_Deploy_FromAdded_State_Internal(true);
        }

        public void CanDeploy_FarmSolution_As_Deploy_FromDeployed_State_Internal(bool isWebApplication)
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = GetFarmSolutionDefinition(isWebApplication, def =>
                {
                    def.ShouldDeploy = true;
                });

                PrepareDeployedState(solutionDef, isWebApplication);
                TestFarmSolutionModel(solutionDef, isWebApplication);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm")]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm.Deploy")]
        public void CanDeploy_FarmSolution_As_Deploy_FromDeployed_State()
        {
            CanDeploy_FarmSolution_As_Deploy_FromDeployed_State_Internal(false);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication")]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication.Deploy")]
        public void CanDeploy_FarmSolution_As_Deploy_FromDeployed_State_UnderWebApplication()
        {
            CanDeploy_FarmSolution_As_Deploy_FromDeployed_State_Internal(true);
        }

        #endregion

        #region upgrade operations

        public void CanDeploy_FarmSolution_As_Upgrade_FromDeleted_State_Internal(bool isWebApplication)
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = GetFarmSolutionDefinition(isWebApplication, def =>
                {
                    def.ShouldUpgrade = true;
                });

                PrepareDeletedState(solutionDef, isWebApplication);
                TestFarmSolutionModel(solutionDef, isWebApplication);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm.Upgrade")]
        public void CanDeploy_FarmSolution_As_Upgrade_FromDeleted_State()
        {
            CanDeploy_FarmSolution_As_Upgrade_FromDeleted_State_Internal(false);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication.Upgrade")]
        public void CanDeploy_FarmSolution_As_Upgrade_FromDeleted_State_UnderWebApplication()
        {
            CanDeploy_FarmSolution_As_Upgrade_FromDeleted_State_Internal(true);
        }

        public void CanDeploy_FarmSolution_As_Upgrade_FromRetracted_State_Internal(bool isWebApplication)
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = GetFarmSolutionDefinition(isWebApplication, def =>
                {
                    def.ShouldUpgrade = true;
                });

                PrepareRetractedState(solutionDef, isWebApplication);
                TestFarmSolutionModel(solutionDef, isWebApplication);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm.Upgrade")]
        public void CanDeploy_FarmSolution_As_Upgrade_FromRetracted_State()
        {
            CanDeploy_FarmSolution_As_Upgrade_FromRetracted_State_Internal(false);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication.Upgrade")]
        public void CanDeploy_FarmSolution_As_Upgrade_FromRetracted_State_UnderWebApplication()
        {
            CanDeploy_FarmSolution_As_Upgrade_FromRetracted_State_Internal(true);
        }

        public void CanDeploy_FarmSolution_As_Upgrade_FromAdded_State_Internal(bool isWebApplication)
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = GetFarmSolutionDefinition(isWebApplication, def =>
                {
                    def.ShouldUpgrade = true;
                });

                PrepareAddedState(solutionDef, isWebApplication);
                //TestFarmSolutionModel(solutionDef);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm.Upgrade")]
        public void CanDeploy_FarmSolution_As_Upgrade_FromAdded_State()
        {
            CanDeploy_FarmSolution_As_Upgrade_FromAdded_State_Internal(false);
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication.Upgrade")]
        public void CanDeploy_FarmSolution_As_Upgrade_FromAdded_State_UnderWebApplication()
        {
            CanDeploy_FarmSolution_As_Upgrade_FromAdded_State_Internal(true);
        }

        public void CanDeploy_FarmSolution_As_Upgrade_FromDeployed_State_Internal(bool isWebApplication)
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = GetFarmSolutionDefinition(isWebApplication, def =>
                {
                    def.ShouldUpgrade = true;
                });

                PrepareDeployedState(solutionDef, isWebApplication);
                TestFarmSolutionModel(solutionDef, isWebApplication);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Farm.Upgrade")]
        public void CanDeploy_FarmSolution_As_Upgrade_FromDeployed_State()
        {
            CanDeploy_FarmSolution_As_Upgrade_FromDeployed_State_Internal(false);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.WebApplication.Upgrade")]
        public void CanDeploy_FarmSolution_As_Upgrade_FromDeployed_State_UnderWebApplication()
        {
            CanDeploy_FarmSolution_As_Upgrade_FromDeployed_State_Internal(true);
        }

        #endregion

        #region utils

        private void PrepareDeployedState(FarmSolutionDefinition solutionDef,
            bool isWebAcpplication)
        {
            var deployDef = solutionDef.Inherit(def =>
            {
                def.ShouldDelete = null;
                def.ShouldRetract = null;
                def.ShouldUpgrade = null;

                def.ShouldAdd = true;
                def.ShouldDeploy = true;
            });

            TestFarmSolutionModel(deployDef, isWebAcpplication);
        }

        private void PrepareAddedState(FarmSolutionDefinition solutionDef,
            bool isWebApplication)
        {
            var addDef = solutionDef.Inherit(def =>
            {
                def.ShouldDelete = null;
                def.ShouldDeploy = null;
                def.ShouldUpgrade = null;

                def.ShouldAdd = true;
                def.ShouldRetract = null;
            });

            TestFarmSolutionModel(addDef, isWebApplication);

            var retractDef = solutionDef.Inherit(def =>
            {
                def.ShouldDelete = null;
                def.ShouldDeploy = null;
                def.ShouldUpgrade = null;

                def.ShouldAdd = true;
                def.ShouldRetract = true;
            });

            TestFarmSolutionModel(retractDef, isWebApplication);
        }

        private void PrepareRetractedState(FarmSolutionDefinition solutionDef,
            bool isWebApplication)
        {
            var deployDef = solutionDef.Inherit(def =>
            {
                def.ShouldDelete = null;
                def.ShouldRetract = null;
                def.ShouldUpgrade = null;

                def.ShouldAdd = true;
                def.ShouldDeploy = true;
            });

            var retractDef = solutionDef.Inherit(def =>
            {
                def.ShouldDelete = null;
                def.ShouldDeploy = null;
                def.ShouldUpgrade = null;

                def.ShouldAdd = true;
                def.ShouldRetract = true;
            });

            TestFarmSolutionModel(deployDef, isWebApplication);
            TestFarmSolutionModel(retractDef, isWebApplication);
        }

        private void PrepareDeletedState(FarmSolutionDefinition solutionDef,
                bool isWebApplication)
        {
            var deletedDef = solutionDef.Inherit(def =>
            {
                def.ShouldDeploy = null;
                def.ShouldRetract = true;
                def.ShouldUpgrade = null;

                def.ShouldAdd = false;
                def.ShouldDelete = true;
            });

            TestFarmSolutionModel(deletedDef, isWebApplication);
        }

        protected virtual void TestFarmSolutionModel(FarmSolutionDefinition solutionDef, bool isWebApplicationLevel)
        {
            var newSolutiondef = solutionDef.Inherit();

            if (isWebApplicationLevel)
            {
                var originalModel = SPMeta2Model.NewWebApplicationModel(webApp =>
                {
                    webApp.AddFarmSolution(solutionDef);
                });

                var newModel = SPMeta2Model.NewWebApplicationModel(farm =>
                {
                    farm.AddFarmSolution(newSolutiondef);
                });

                TestModel(originalModel);
                TestModel(newModel);
            }
            else
            {
                var originalModel = SPMeta2Model.NewFarmModel(farm =>
                {
                    farm.AddFarmSolution(solutionDef);
                });

                var newModel = SPMeta2Model.NewFarmModel(farm =>
                {
                    farm.AddFarmSolution(newSolutiondef);
                });

                TestModel(originalModel);
                TestModel(newModel);
            }
        }

        #endregion
    }
}
