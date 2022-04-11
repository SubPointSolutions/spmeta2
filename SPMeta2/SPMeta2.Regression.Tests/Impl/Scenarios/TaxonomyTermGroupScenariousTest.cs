﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void CanDeploy_TaxonomyTermGroup_As_SiteCollectionGroup()
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

        [TestCategory("Regression.Scenarios.Taxonomy.TermGroup")]
        [TestMethod]
        public void CanDeploy_TaxonomyTermGroup_As_NonSiteCollectionGroup()
        {
            // woudn't work for O365
            var termGroup = ModelGeneratorService.GetRandomDefinition<TaxonomyTermGroupDefinition>(def =>
            {
                def.IsSiteCollectionGroup = false;
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

        [TestCategory("Regression.Scenarios.Taxonomy.TermGroup")]
        [TestMethod]
        public void CanDeploy_TaxonomyTermGroup_As_NonSiteCollectionGrou_SameNameAndId()
        {
            // woudn't work for O365
            // must be scoped for on-premis execution only

            // SPMeta2 Provisioning Taxonomy Group with CSOM Standard #959
            // https://github.com/SubPointSolutions/spmeta2/issues/959

            var termGroup1 = ModelGeneratorService.GetRandomDefinition<TaxonomyTermGroupDefinition>(def =>
            {
                def.Name = Rnd.String();
                def.Id = Rnd.Guid();

                def.IsSiteCollectionGroup = false;
            });

            var termGroup2 = termGroup1.Inherit();

            var model = SPMeta2Model
                  .NewSiteModel(site =>
                  {
                      site
                          .AddRandomTermStore(store =>
                          {
                              store.AddTaxonomyTermGroup(termGroup1);
                              store.AddTaxonomyTermGroup(termGroup2);
                          });
                  });

            TestModel(model);
        }

        #endregion
    }
}
