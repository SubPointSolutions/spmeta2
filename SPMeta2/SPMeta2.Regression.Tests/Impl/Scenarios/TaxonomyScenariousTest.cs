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
using SPMeta2.Models;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Standard.Syntax;
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

        protected void GenerateTermsTree(ModelNode termSetOrTermNode, int deep, bool cleanGuid)
        {
            if (deep == 0)
                return;

            termSetOrTermNode.AddRandomTerm(term =>
            {
                var termDef = term.Value as TaxonomyTermDefinition;
                termDef.Name = string.Format("InvertedLevel_{0}_{1}", deep, termDef.Name);

                if (cleanGuid)
                    termDef.Id = null;

                GenerateTermsTree(term, --deep, cleanGuid);
            });
        }

        protected ModelNode GenerateTermTaxonomyTree(int deep, bool cleanGuid)
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
                                           GenerateTermsTree(termSet, deep, cleanGuid);
                                       });
                               });
                       });
               });

            return model;
        }

        #region term tree, clean ID

        [TestMethod]
        [TestCategory("Regression.Scenarios.Taxonomy.Hierarchy")]
        public void CanDeploy_TaxonomyTerm_Named_Tree_Level_1()
        {
            var model = GenerateTermTaxonomyTree(1, true);
            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Taxonomy.Hierarchy")]
        public void CanDeploy_TaxonomyTerm_Named_Tree_Level_2()
        {
            var model = GenerateTermTaxonomyTree(2, true);
            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Taxonomy.Hierarchy")]
        public void CanDeploy_TaxonomyTerm_Named_Tree_Level_3()
        {
            var model = GenerateTermTaxonomyTree(3, true);
            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Taxonomy.Hierarchy")]
        public void CanDeploy_TaxonomyTerm_Named_Tree_Level_4()
        {
            var model = GenerateTermTaxonomyTree(4, true);
            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Taxonomy.Hierarchy")]
        public void CanDeploy_TaxonomyTerm_Named_Tree_Level_5()
        {
            var model = GenerateTermTaxonomyTree(5, true);
            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Taxonomy.Hierarchy")]
        public void CanDeploy_TaxonomyTerm_Named_Tree_Level_6()
        {
            var model = GenerateTermTaxonomyTree(6, true);
            TestModel(model);
        }

        #endregion

        #region term tree, non empty ID

        [TestMethod]
        [TestCategory("Regression.Scenarios.Taxonomy.Hierarchy")]
        public void CanDeploy_TaxonomyTerm_Id_Tree_Level_1()
        {
            var model = GenerateTermTaxonomyTree(1, false);
            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Taxonomy.Hierarchy")]
        public void CanDeploy_TaxonomyTerm_Id_Tree_Level_2()
        {
            var model = GenerateTermTaxonomyTree(2, false);
            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Taxonomy.Hierarchy")]
        public void CanDeploy_TaxonomyTerm_Id_Tree_Level_3()
        {
            var model = GenerateTermTaxonomyTree(3, false);
            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Taxonomy.Hierarchy")]
        public void CanDeploy_TaxonomyTerm_Id_Tree_Level_4()
        {
            var model = GenerateTermTaxonomyTree(4, false);
            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Taxonomy.Hierarchy")]
        public void CanDeploy_TaxonomyTerm_Id_Tree_Level_5()
        {
            var model = GenerateTermTaxonomyTree(5, false);
            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Taxonomy.Hierarchy")]
        public void CanDeploy_TaxonomyTerm_Id_Tree_Level_6()
        {
            var model = GenerateTermTaxonomyTree(6, false);
            TestModel(model);
        }

        #endregion

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
