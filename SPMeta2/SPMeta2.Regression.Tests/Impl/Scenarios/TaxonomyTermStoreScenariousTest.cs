using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Containers.Consts;
using SPMeta2.Containers.Standard;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;
using SPMeta2.Containers.Utils;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class TaxonomyTermStoreScenariousTest : SPMeta2RegresionScenarioTestBase
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

        [TestCategory("Regression.Scenarios.Taxonomy.TermStore")]
        [TestMethod]
        public void CanDeploy_TaxonomyTermStoreByUseDefaultSiteCollectionTermStore()
        {
            var termStore = new TaxonomyTermStoreDefinition
            {
                UseDefaultSiteCollectionTermStore = true
            };

            var model = SPMeta2Model
                  .NewSiteModel(site =>
                  {
                      site.AddTaxonomyTermStore(termStore);
                  });

            TestModel(model);
        }

        [TestCategory("Regression.Scenarios.Taxonomy.TermStore")]
        [TestMethod]
        public void CanDeploy_TaxonomyTermStoreByName()
        {
            var termStoreName = ConvertUtils.ToString(RunnerEnvironmentUtils.GetEnvironmentVariable(EnvironmentConsts.DefaultTaxonomyStoreName));
            Assert.IsFalse(string.IsNullOrEmpty(termStoreName));

            var termStore = new TaxonomyTermStoreDefinition
            {
                Name = termStoreName
            };

            var model = SPMeta2Model
                  .NewSiteModel(site =>
                  {
                      site.AddTaxonomyTermStore(termStore);
                  });

            TestModel(model);
        }

        [TestCategory("Regression.Scenarios.Taxonomy.TermStore")]
        [TestMethod]
        public void CanDeploy_TaxonomyTermStoreById()
        {
            var termStoreId = ConvertUtils.ToGuid(RunnerEnvironmentUtils.GetEnvironmentVariable(EnvironmentConsts.DefaultTaxonomyStoreId));
            Assert.IsTrue(termStoreId.HasValue);

            var termStore = new TaxonomyTermStoreDefinition
            {
                Id = termStoreId.Value
            };

            var model = SPMeta2Model
                  .NewSiteModel(site =>
                  {
                      site.AddTaxonomyTermStore(termStore);
                  });

            TestModel(model);
        }



        #endregion
    }
}
