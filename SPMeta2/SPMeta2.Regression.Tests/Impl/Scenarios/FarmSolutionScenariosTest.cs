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

        #region test

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution")]
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

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution")]
        public void CanDeploy_FarmSolution_As_Deploy()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = ModelGeneratorService.GetRandomDefinition<FarmSolutionDefinition>(def =>
                {
                    def.ShouldDeploy = true;
                });

                TestFarmSolutionModel(solutionDef);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution")]
        public void CanDeploy_FarmSolution_As_Delete()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = ModelGeneratorService.GetRandomDefinition<FarmSolutionDefinition>(def =>
                {
                    def.ShouldDelete = true;
                });

                TestFarmSolutionModel(solutionDef);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution")]
        public void CanDeploy_FarmSolution_As_Retract()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = ModelGeneratorService.GetRandomDefinition<FarmSolutionDefinition>(def =>
                {
                    def.ShouldRetract = true;
                });

                TestFarmSolutionModel(solutionDef);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution")]
        public void CanDeploy_FarmSolution_As_Delete_Deploy()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = ModelGeneratorService.GetRandomDefinition<FarmSolutionDefinition>(def =>
                {
                    def.ShouldDelete = true;
                    def.ShouldDeploy = true;
                });

                TestFarmSolutionModel(solutionDef);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution")]
        public void CanDeploy_FarmSolution_As_Retract_Deploy()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = ModelGeneratorService.GetRandomDefinition<FarmSolutionDefinition>(def =>
                {
                    def.ShouldRetract = true;
                    def.ShouldDeploy = true;
                });

                TestFarmSolutionModel(solutionDef);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.FarmSolution")]
        public void CanDeploy_FarmSolution_As_Retract_Delete_Deploy()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var solutionDef = ModelGeneratorService.GetRandomDefinition<FarmSolutionDefinition>(def =>
                {
                    def.ShouldRetract = true;
                    def.ShouldDelete = true;
                    def.ShouldDeploy = true;
                });

                TestFarmSolutionModel(solutionDef);
            });
        }

        #endregion

        #region utils

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
