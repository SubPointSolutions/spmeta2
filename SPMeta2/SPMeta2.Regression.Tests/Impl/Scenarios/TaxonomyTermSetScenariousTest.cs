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
    public class TaxonomyTermSetScenariousTest : SPMeta2RegresionScenarioTestBase
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

        [TestCategory("Regression.Scenarios.Taxonomy.TermSet")]
        [TestMethod]
        public void CanDeploy_TaxonomyTermSetByName()
        {
            var termSet = ModelGeneratorService.GetRandomDefinition<TaxonomyTermSetDefinition>(def =>
            {
            });

            var model = SPMeta2Model
                  .NewSiteModel(site =>
                  {
                      site
                          .AddRandomTermStore(store =>
                          {
                              store.AddRandomTermGroup(group =>
                              {
                                  group.AddTaxonomyTermSet(termSet);
                              });
                          });
                  });

            TestModel(model);
        }


        [TestCategory("Regression.Scenarios.Taxonomy.TermSet")]
        [TestMethod]
        public void CanDeploy_TaxonomyTermSetByNameAndId()
        {
            var termSet = ModelGeneratorService.GetRandomDefinition<TaxonomyTermSetDefinition>(def =>
            {
                def.Id = Guid.NewGuid();
            });

            var model = SPMeta2Model
                  .NewSiteModel(site =>
                  {
                      site
                          .AddRandomTermStore(store =>
                          {
                              store.AddRandomTermGroup(group =>
                              {
                                  group.AddTaxonomyTermSet(termSet);
                              });
                          });
                  });

            TestModel(model);
        }

        #endregion
    }
}
