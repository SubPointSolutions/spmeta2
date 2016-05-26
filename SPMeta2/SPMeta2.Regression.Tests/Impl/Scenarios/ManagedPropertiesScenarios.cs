using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;
using SPMeta2.Exceptions;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class ManagedPropertiesScenarios : SPMeta2RegresionScenarioTestBase
    {
        #region internal

        [ClassInitializeAttribute]
        public static void Init(TestContext context)
        {
            InternalInit();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            InternalCleanup();
        }

        #endregion

        #region tests

        [TestMethod]
        [TestCategory("Regression.Scenarios.ManagedProperties")]
        public void CanDeploy_ManagedProperty_OnFarm()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var managedProperty = ModelGeneratorService.GetRandomDefinition<ManagedPropertyDefinition>();

                var mdoel = SPMeta2Model.NewFarmModel(farm =>
                {
                    farm.AddManagedProperty(managedProperty);
                });

                TestModel(mdoel);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ManagedProperties.Sys")]
        public void CanDeploy_ManagedProperty_WithWrongCrawledProperty_OnFarm()
        {
            // https://github.com/SubPointSolutions/spmeta2/pull/680

            // Whenever you try to assign a mapping for a crawled property 
            // which doesn't exist to the mappings property of a ManagedPropertyDefinition 
            // you'll get a null reference exception.

            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var managedProperty = ModelGeneratorService.GetRandomDefinition<ManagedPropertyDefinition>(def =>
                {
                    def.Mappings.Clear();
                    def.Mappings.Add(new ManagedPropertyMappping
                    {
                        CrawledPropertyName = Rnd.String()
                    });
                });

                var mdoel = SPMeta2Model.NewFarmModel(farm =>
                {
                    farm.AddManagedProperty(managedProperty);
                });

                TestModel(mdoel);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ManagedProperties")]
        public void CanDeploy_ManagedProperty_OnSite()
        {
            // TODO
            // site scoped level is not supported yet
            // test was added for the future

            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                WithExcpectedException(typeof(SPMeta2NotImplementedException), () =>
                {
                    var managedProperty = ModelGeneratorService.GetRandomDefinition<ManagedPropertyDefinition>();

                    var mdoel = SPMeta2Model.NewSiteModel(farm =>
                    {
                        farm.AddManagedProperty(managedProperty);
                    });

                    TestModel(mdoel);
                });
            });
        }

        #endregion
    }
}
