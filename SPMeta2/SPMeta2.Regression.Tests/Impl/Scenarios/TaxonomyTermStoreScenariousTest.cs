using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Containers.Standard;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;

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
            // TODO
            var termStore = new TaxonomyTermStoreDefinition
            {
                Name = "Managed Metadata Application"
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
            // TODO
            var termStore = new TaxonomyTermStoreDefinition
            {
                Id = new Guid("{bb9dd05d-a115-45b3-9019-6957a0a87ed1}")
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
