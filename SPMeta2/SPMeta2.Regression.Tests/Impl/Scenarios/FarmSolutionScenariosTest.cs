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
        [TestCategory("Regression.Scenarios.FarmSolution.Default")]
        public void CanDeploy_FarmSolution_As_Default()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = ModelGeneratorService.GetRandomDefinition<FarmSolutionDefinition>(def =>
                {

                });

                TestFarmSolutionModel(solutionDef);
            });
        }

        #endregion

        #region add operations

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Add")]
        public void CanDeploy_FarmSolution_As_Add_FromDeleted_State()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = ModelGeneratorService.GetRandomDefinition<FarmSolutionDefinition>(def =>
                {

                });

                PrepareDeletedState(solutionDef);

                TestFarmSolutionModel(solutionDef);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Add")]
        public void CanDeploy_FarmSolution_As_Add_FromRetracted_State()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = ModelGeneratorService.GetRandomDefinition<FarmSolutionDefinition>(def =>
                {

                });

                PrepareRetractedState(solutionDef);
                TestFarmSolutionModel(solutionDef);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Add")]
        public void CanDeploy_FarmSolution_As_Add_FromAdded_State()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = ModelGeneratorService.GetRandomDefinition<FarmSolutionDefinition>(def =>
                {

                });

                PrepareAddedState(solutionDef);
                TestFarmSolutionModel(solutionDef);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Add")]
        public void CanDeploy_FarmSolution_As_Add_FromDeployed_State()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = ModelGeneratorService.GetRandomDefinition<FarmSolutionDefinition>(def =>
                {

                });

                PrepareDeployedState(solutionDef);
                TestFarmSolutionModel(solutionDef);
            });
        }

        #endregion

        #region delete operations

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Delete")]
        public void CanDeploy_FarmSolution_As_Delete_FromDeleted_State()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = ModelGeneratorService.GetRandomDefinition<FarmSolutionDefinition>(def =>
                {
                    def.ShouldDelete = true;
                    def.ShouldAdd = false;
                });

                PrepareDeletedState(solutionDef);

                TestFarmSolutionModel(solutionDef);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Delete")]
        public void CanDeploy_FarmSolution_As_Delete_FromRetracted_State()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = ModelGeneratorService.GetRandomDefinition<FarmSolutionDefinition>(def =>
                {
                    def.ShouldDelete = true;
                    def.ShouldAdd = false;
                });

                PrepareRetractedState(solutionDef);
                TestFarmSolutionModel(solutionDef);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Delete")]
        public void CanDeploy_FarmSolution_As_Delete_FromAdded_State()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = ModelGeneratorService.GetRandomDefinition<FarmSolutionDefinition>(def =>
                {
                    def.ShouldDelete = true;
                    def.ShouldAdd = false;
                });

                PrepareAddedState(solutionDef);
                TestFarmSolutionModel(solutionDef);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Delete")]
        public void CanDeploy_FarmSolution_As_Delete_FromDeployed_State()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = ModelGeneratorService.GetRandomDefinition<FarmSolutionDefinition>(def =>
                {
                    def.ShouldDelete = true;
                    def.ShouldAdd = false;
                });

                PrepareDeployedState(solutionDef);
                TestFarmSolutionModel(solutionDef);
            });
        }

        #endregion

        #region retract operations

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Retract")]
        public void CanDeploy_FarmSolution_As_Retract_FromDeleted_State()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = ModelGeneratorService.GetRandomDefinition<FarmSolutionDefinition>(def =>
                {
                    def.ShouldRetract = true;
                });

                PrepareDeletedState(solutionDef);

                TestFarmSolutionModel(solutionDef);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Retract")]
        public void CanDeploy_FarmSolution_As_Retract_FromRetracted_State()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = ModelGeneratorService.GetRandomDefinition<FarmSolutionDefinition>(def =>
                {
                    def.ShouldRetract = true;
                });

                PrepareRetractedState(solutionDef);
                TestFarmSolutionModel(solutionDef);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Retract")]
        public void CanDeploy_FarmSolution_As_Retract_FromAdded_State()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = ModelGeneratorService.GetRandomDefinition<FarmSolutionDefinition>(def =>
                {
                    def.ShouldRetract = true;
                });

                PrepareAddedState(solutionDef);
                TestFarmSolutionModel(solutionDef);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Retract")]
        public void CanDeploy_FarmSolution_As_Retract_FromDeployed_State()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = ModelGeneratorService.GetRandomDefinition<FarmSolutionDefinition>(def =>
                {
                    def.ShouldRetract = true;
                });

                PrepareDeployedState(solutionDef);
                TestFarmSolutionModel(solutionDef);
            });
        }

        #endregion


        #region deploy operations

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Deploy")]
        public void CanDeploy_FarmSolution_As_Deploy_FromDeleted_State()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = ModelGeneratorService.GetRandomDefinition<FarmSolutionDefinition>(def =>
                {
                    def.ShouldDeploy = true;
                });

                PrepareDeletedState(solutionDef);
                TestFarmSolutionModel(solutionDef);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Deploy")]
        public void CanDeploy_FarmSolution_As_Deploy_FromRetracted_State()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = ModelGeneratorService.GetRandomDefinition<FarmSolutionDefinition>(def =>
                {
                    def.ShouldDeploy = true;
                });

                PrepareRetractedState(solutionDef);
                TestFarmSolutionModel(solutionDef);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Deploy")]
        public void CanDeploy_FarmSolution_As_Deploy_FromAdded_State()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = ModelGeneratorService.GetRandomDefinition<FarmSolutionDefinition>(def =>
                {
                    def.ShouldDeploy = true;
                });

                PrepareAddedState(solutionDef);
                TestFarmSolutionModel(solutionDef);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Deploy")]
        public void CanDeploy_FarmSolution_As_Deploy_FromDeployed_State()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = ModelGeneratorService.GetRandomDefinition<FarmSolutionDefinition>(def =>
                {
                    def.ShouldDeploy = true;
                });

                PrepareDeployedState(solutionDef);
                TestFarmSolutionModel(solutionDef);
            });
        }

        #endregion

        #region upgrade operations

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Upgrade")]
        public void CanDeploy_FarmSolution_As_Upgrade_FromDeleted_State()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = ModelGeneratorService.GetRandomDefinition<FarmSolutionDefinition>(def =>
                {
                    def.ShouldUpgrade = true;
                });

                PrepareDeletedState(solutionDef);
                TestFarmSolutionModel(solutionDef);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Upgrade")]
        public void CanDeploy_FarmSolution_As_Upgrade_FromRetracted_State()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = ModelGeneratorService.GetRandomDefinition<FarmSolutionDefinition>(def =>
                {
                    def.ShouldUpgrade = true;
                });

                PrepareRetractedState(solutionDef);
                TestFarmSolutionModel(solutionDef);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Upgrade")]
        public void CanDeploy_FarmSolution_As_Upgrade_FromAdded_State()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = ModelGeneratorService.GetRandomDefinition<FarmSolutionDefinition>(def =>
                {
                    def.ShouldUpgrade = true;
                });

                PrepareAddedState(solutionDef);
                //TestFarmSolutionModel(solutionDef);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution.Upgrade")]
        public void CanDeploy_FarmSolution_As_Upgrade_FromDeployed_State()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = ModelGeneratorService.GetRandomDefinition<FarmSolutionDefinition>(def =>
                {
                    def.ShouldUpgrade = true;
                });

                PrepareDeployedState(solutionDef);
                TestFarmSolutionModel(solutionDef);
            });
        }

        #endregion

        #region utils

        private void PrepareDeployedState(FarmSolutionDefinition solutionDef)
        {
            var deployDef = solutionDef.Inherit(def =>
            {
                def.ShouldDelete = null;
                def.ShouldRetract = null;
                def.ShouldUpgrade = null;

                def.ShouldAdd = true;
                def.ShouldDeploy = true;
            });

            TestFarmSolutionModel(deployDef);
        }

        private void PrepareAddedState(FarmSolutionDefinition solutionDef)
        {
            var addDef = solutionDef.Inherit(def =>
            {
                def.ShouldDelete = null;
                def.ShouldDeploy = null;
                def.ShouldUpgrade = null;

                def.ShouldAdd = true;
                def.ShouldRetract = null;
            });

            TestFarmSolutionModel(addDef);

            var retractDef = solutionDef.Inherit(def =>
            {
                def.ShouldDelete = null;
                def.ShouldDeploy = null;
                def.ShouldUpgrade = null;

                def.ShouldAdd = true;
                def.ShouldRetract = true;
            });

            TestFarmSolutionModel(retractDef);
        }

        private void PrepareRetractedState(FarmSolutionDefinition solutionDef)
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

            TestFarmSolutionModel(deployDef);
            TestFarmSolutionModel(retractDef);
        }

        private void PrepareDeletedState(FarmSolutionDefinition solutionDef)
        {
            var deletedDef = solutionDef.Inherit(def =>
            {
                def.ShouldDeploy = null;
                def.ShouldRetract = true;
                def.ShouldUpgrade = null;

                def.ShouldAdd = false;
                def.ShouldDelete = true;
            });

            TestFarmSolutionModel(deletedDef);
        }

        protected virtual void TestFarmSolutionModel(FarmSolutionDefinition solutionDef)
        {
            var newSolutiondef = solutionDef.Inherit();

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

        #endregion
    }
}
