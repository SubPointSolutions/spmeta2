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
using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class TaxonomyScenariousTest : SPMeta2RegresionScenarioTestBase
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

        [TestMethod]
        [TestCategory("Regression.Scenarios.Taxonomy")]
        public void CanDeploy_TaxonomyTermTree()
        {
            var model = SPMeta2Model
                 .NewSiteModel(site =>
                 {
                     site
                         .AddRandomTermStore(store =>
                         {
                             store
                                 .AddRandomTermGroup(group =>
                                 {
                                     group
                                         .AddRandomTermSet(termSet =>
                                         {
                                             termSet
                                                 .AddRandomTerm(level1 =>
                                                 {
                                                     level1
                                                         .AddRandomTerm(level2 =>
                                                         {
                                                             level2.AddRandomTerm();
                                                         });
                                                 });
                                         });
                                 });
                         });
                 });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Taxonomy")]
        public void CanDeploy_TaxonomyHierarchy()
        {
            var model = SPMeta2Model
                  .NewSiteModel(site =>
                  {
                      site
                          .AddRandomTermStore(store =>
                          {
                              store
                                  .AddRandomTermGroup(group =>
                                  {
                                      group
                                          .AddRandomTermSet(termSet =>
                                          {
                                              termSet
                                                  .AddRandomTerm()
                                                  .AddRandomTerm()
                                                  .AddRandomTerm()
                                                  .AddRandomTerm();
                                          })
                                          .AddRandomTermSet(termSet =>
                                          {
                                              termSet
                                                  .AddRandomTerm()
                                                  .AddRandomTerm();
                                          });
                                  })
                                  .AddRandomTermGroup(group =>
                                  {
                                      group
                                          .AddRandomTermSet(termSet =>
                                          {
                                              termSet
                                                  .AddRandomTerm()
                                                  .AddRandomTerm();
                                          });
                                  });
                          });
                  });

            TestModel(model);
        }


        #endregion
    }
}
