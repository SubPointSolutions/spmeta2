using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Containers.Standard;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Containers.Services;
using SPMeta2.Regression.Tests.Utils;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Modern;
using ReflectionUtils = SPMeta2.Utils.ReflectionUtils;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class TaxonomyTermScenariousTest : SPMeta2RegresionScenarioTestBase
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

        [TestCategory("Regression.Scenarios.Taxonomy.Term")]
        [TestMethod]
        public void CanDeploy_TaxonomyTerms_WithCustomProperties()
        {
            var term = ModelGeneratorService.GetRandomDefinition<TaxonomyTermDefinition>(def =>
            {
                def.CustomProperties.Add(new TaxonomyTermCustomProperty
                {
                    Name = Rnd.String(),
                    Value = Rnd.String()
                });

                def.CustomProperties.Add(new TaxonomyTermCustomProperty
                {
                    Name = Rnd.String(),
                    Value = Rnd.HttpUrl()
                });

                def.CustomProperties.Add(new TaxonomyTermCustomProperty
                {
                    Name = Rnd.Int().ToString(),
                    Value = Rnd.Int().ToString()
                });
            });

            var subTerm = ModelGeneratorService.GetRandomDefinition<TaxonomyTermDefinition>(def =>
            {
                def.CustomProperties.Add(new TaxonomyTermCustomProperty
                {
                    Name = Rnd.String(),
                    Value = Rnd.String()
                });

                def.CustomProperties.Add(new TaxonomyTermCustomProperty
                {
                    Name = Rnd.String(),
                    Value = Rnd.HttpUrl()
                });

                def.CustomProperties.Add(new TaxonomyTermCustomProperty
                {
                    Name = Rnd.Int().ToString(),
                    Value = Rnd.Int().ToString()
                });
            });

            var model = SPMeta2Model
                  .NewSiteModel(site =>
                  {
                      site
                          .AddRandomTermStore(store =>
                          {
                              store.AddRandomTermGroup(group =>
                              {
                                  group.AddRandomTermSet(termSet =>
                                  {
                                      termSet.AddTaxonomyTerm(term, t =>
                                      {
                                          t.AddTaxonomyTerm(subTerm);
                                      });
                                  });
                              });
                          });
                  });

            TestModel(model);
        }

        [TestCategory("Regression.Scenarios.Taxonomy.Term")]
        [TestMethod]
        public void CanDeploy_TaxonomyTermByName()
        {
            var term = ModelGeneratorService.GetRandomDefinition<TaxonomyTermDefinition>(def =>
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
                                  group.AddRandomTermSet(termSet =>
                                  {
                                      termSet.AddTaxonomyTerm(term);
                                  });
                              });
                          });
                  });

            TestModel(model);
        }


        [TestCategory("Regression.Scenarios.Taxonomy.Term")]
        [TestMethod]
        public void CanDeploy_TaxonomyTermByNameAndId()
        {
            var term = ModelGeneratorService.GetRandomDefinition<TaxonomyTermDefinition>(def =>
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
                                  group.AddRandomTermSet(termSet =>
                                  {
                                      termSet.AddTaxonomyTerm(term);
                                  });
                              });
                          });
                  });

            TestModel(model);
        }



        #endregion

        #region special chars


        [TestCategory("Regression.Scenarios.Taxonomy.Term.SpecialChars")]
        [TestMethod]
        public void CanDeploy_TaxonomyTerm_With_Ampersand_And_DoubleQuotes()
        {
            var term1 = ModelGeneratorService.GetRandomDefinition<TaxonomyTermDefinition>(def =>
            {
                def.Name = string.Format("1 - \"{0}\" & \"{1}\"", Rnd.String(), Rnd.String());
            });

            var term2 = ModelGeneratorService.GetRandomDefinition<TaxonomyTermDefinition>(def =>
            {
                def.Name = string.Format("2 - \"{0}\" & \"{1}\"", Rnd.String(), Rnd.String());
            });

            var term3 = ModelGeneratorService.GetRandomDefinition<TaxonomyTermDefinition>(def =>
            {
                def.Name = string.Format("3 - \"{0}\" & \"{1}\"", Rnd.String(), Rnd.String());
            });

            var term4 = ModelGeneratorService.GetRandomDefinition<TaxonomyTermDefinition>(def =>
            {
                def.Name = string.Format("4 - \"{0} & \"{1}\"", Rnd.String(), Rnd.String());
            });

            var term5 = ModelGeneratorService.GetRandomDefinition<TaxonomyTermDefinition>(def =>
            {
                def.Name = string.Format("5 - \"{0}\" & \"{1}\"", Rnd.String(), Rnd.String());
            });

            var isSameTerm = true;

            var model = SPMeta2Model
                  .NewSiteModel(site =>
                  {
                      site
                          .AddRandomTermStore(store =>
                          {
                              store.AddRandomTermGroup(group =>
                              {
                                  group.AddRandomTermSet(termSet =>
                                  {
                                      termSet.AddTaxonomyTerm(term1);
                                      termSet.AddTaxonomyTerm(term2);
                                      termSet.AddTaxonomyTerm(term3, t3 =>
                                      {
                                          t3.AddTaxonomyTerm(term4);
                                          t3.AddTaxonomyTerm(term5);
                                      });
                                  });
                              });
                          });
                  });

            TestModel(model);
        }


        #endregion
    }
}
