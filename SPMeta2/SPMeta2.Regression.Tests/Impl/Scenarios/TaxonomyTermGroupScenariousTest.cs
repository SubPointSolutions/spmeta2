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

using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class TaxonomyTermGroupScenariousTest : SPMeta2RegresionScenarioTestBase
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

        [TestCategory("Regression.Scenarios.Taxonomy.TermGroup")]
        [TestMethod]
        public void CanDeploy_TaxonomyTermGroupByName()
        {
            var termGroup = ModelGeneratorService.GetRandomDefinition<TaxonomyTermGroupDefinition>(def =>
            {
            });

            var model = SPMeta2Model
                  .NewSiteModel(site =>
                  {
                      site
                          .AddRandomTermStore(store =>
                          {
                              store.AddTaxonomyTermGroup(termGroup);
                          });
                  });

            TestModel(model);
        }


        [TestCategory("Regression.Scenarios.Taxonomy.TermGroup")]
        [TestMethod]
        public void CanDeploy_TaxonomyTermGroupByNameAndId()
        {
            var termGroup = ModelGeneratorService.GetRandomDefinition<TaxonomyTermGroupDefinition>(def =>
            {
                def.Id = Guid.NewGuid();
            });

            var model = SPMeta2Model
                  .NewSiteModel(site =>
                  {
                      site
                          .AddRandomTermStore(store =>
                          {
                              store.AddTaxonomyTermGroup(termGroup);
                          });
                  });

            TestModel(model);
        }

        #endregion

        #region system & default site group

        //[TestCategory("Regression.Scenarios.Taxonomy.TermGroup")]
        //[TestMethod]
        //public void CanDeploy_TaxonomyTermGroupAsSystem()
        //{
        //    Exception expectedException = null;

        //    try
        //    {
        //        var termGroup = ModelGeneratorService.GetRandomDefinition<TaxonomyTermGroupDefinition>(def =>
        //        {
        //            def.IsSystemGroup = true;
        //        });

        //        var model = SPMeta2Model
        //              .NewSiteModel(site =>
        //              {
        //                  site
        //                      .AddRandomTermStore(store =>
        //                      {
        //                          store.AddTaxonomyTermGroup(termGroup, group =>
        //                          {
        //                              group.AddRandomTermSet();
        //                          });
        //                      });
        //              });

        //        TestModel(model);
        //    }
        //    catch (Exception ee)
        //    {
        //        expectedException = ee;
        //    }

        //    // TODO

        //    Assert.IsNotNull(expectedException);
        //    Assert.IsTrue(expectedException.GetType().Name == "TermStoreOperationException");
        //    Assert.IsTrue(expectedException.Message.Contains("Creating a term set in system Group is disallowed"));
        //}

        [TestCategory("Regression.Scenarios.Taxonomy.TermGroup")]
        [TestMethod]
        public void CanDeploy_TaxonomyTermGroupAsSiteCollectionGroup()
        {
            var termGroup = ModelGeneratorService.GetRandomDefinition<TaxonomyTermGroupDefinition>(def =>
            {
                def.IsSiteCollectionGroup = true;
            });

            var model = SPMeta2Model
                  .NewSiteModel(site =>
                  {
                      site
                          .AddRandomTermStore(store =>
                          {
                              store.AddTaxonomyTermGroup(termGroup, group =>
                              {
                                  group.AddRandomTermSet();
                              });
                          });
                  });

            TestModel(model);
        }

        #endregion
    }
}
