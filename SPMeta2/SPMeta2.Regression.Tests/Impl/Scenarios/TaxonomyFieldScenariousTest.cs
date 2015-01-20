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
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class TaxonomyFieldScenariousTest : SPMeta2RegresionScenarioTestBase
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

        #region IsMult

        [TestCategory("Regression.Scenarios.Taxonomy.TaxonomyField")]
        [TestMethod]
        public void CanDeploy_TaxonomyFieldAsSingleSelect()
        {
            var taxField = ModelGeneratorService.GetRandomDefinition<TaxonomyFieldDefinition>(def =>
            {
                def.IsMulti = false;
            });

            var model = SPMeta2Model
                  .NewSiteModel(site =>
                  {
                      site.AddTaxonomyField(taxField);
                  });

            TestModel(model);
        }


        [TestCategory("Regression.Scenarios.Taxonomy.TaxonomyField")]
        [TestMethod]
        public void CanDeploy_TaxonomyFieldAsMiltiSelect()
        {
            var taxField = ModelGeneratorService.GetRandomDefinition<TaxonomyFieldDefinition>(def =>
            {
                def.IsMulti = true;
            });

            var model = SPMeta2Model
                  .NewSiteModel(site =>
                  {
                      site.AddTaxonomyField(taxField);
                  });

            TestModel(model);
        }

        #endregion

        #region TermSet binding

        [TestCategory("Regression.Scenarios.Taxonomy.TaxonomyField")]
        [TestMethod]
        public void CanDeploy_TaxonomyFieldBindedToTermSetByName()
        {
            var termSet = new TaxonomyTermSetDefinition
            {
                Name = Rnd.String(),
                Description = Rnd.String(),
            };

            var taxField = ModelGeneratorService.GetRandomDefinition<TaxonomyFieldDefinition>(def =>
            {
                def.TermSetName = termSet.Name;
            });

            var taxonomyModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddRandomTermStore(termStore =>
                {
                    termStore.AddRandomTermGroup(group =>
                    {
                        group.AddTaxonomyTermSet(termSet);
                    });
                });
            });

            var fieldModel = SPMeta2Model
                  .NewSiteModel(site =>
                  {
                      site.AddTaxonomyField(taxField);
                  });

            TestModels(new[] { taxonomyModel, fieldModel });
        }

        [TestCategory("Regression.Scenarios.Taxonomy.TaxonomyField")]
        [TestMethod]
        public void CanDeploy_TaxonomyFieldBindedToTermSetById()
        {
            var termSet = new TaxonomyTermSetDefinition
            {
                Name = Rnd.String(),
                Description = Rnd.String(),
                Id = Rnd.Guid()
            };

            var taxField = ModelGeneratorService.GetRandomDefinition<TaxonomyFieldDefinition>(def =>
            {
                def.TermSetId = termSet.Id.Value;
            });

            var taxonomyModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddRandomTermStore(termStore =>
                {
                    termStore.AddRandomTermGroup(group =>
                    {
                        group.AddTaxonomyTermSet(termSet);
                    });
                });
            });

            var fieldModel = SPMeta2Model
                  .NewSiteModel(site =>
                  {
                      site.AddTaxonomyField(taxField);
                  });

            TestModels(new[] { taxonomyModel, fieldModel });
        }

        #endregion

        #region test set and term binding

        [TestCategory("Regression.Scenarios.Taxonomy.TaxonomyField")]
        [TestMethod]
        public void CanDeploy_TaxonomyFieldBindedToTermByName()
        {
            var termSet = new TaxonomyTermSetDefinition
            {
                Name = Rnd.String(),
                Description = Rnd.String(),
            };

            var term = new TaxonomyTermDefinition
            {
                Name = Rnd.String(),
                Description = Rnd.String(),
            };

            var taxField = ModelGeneratorService.GetRandomDefinition<TaxonomyFieldDefinition>(def =>
            {
                def.TermSetName = termSet.Name;
                def.TermName = term.Name;
            });

            var taxonomyModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddRandomTermStore(termStore =>
                {
                    termStore.AddRandomTermGroup(group =>
                    {
                        group.AddTaxonomyTermSet(termSet, t =>
                        {
                            t.AddTaxonomyTerm(term);
                        });
                    });
                });
            });

            var fieldModel = SPMeta2Model
                  .NewSiteModel(site =>
                  {
                      site.AddTaxonomyField(taxField);
                  });

            TestModels(new[] { taxonomyModel, fieldModel });
        }

        [TestCategory("Regression.Scenarios.Taxonomy.TaxonomyField")]
        [TestMethod]
        public void CanDeploy_TaxonomyFieldBindedToTermById()
        {
            var termSet = new TaxonomyTermSetDefinition
            {
                Name = Rnd.String(),
                Description = Rnd.String(),
            };

            var term = new TaxonomyTermDefinition
            {
                Name = Rnd.String(),
                Description = Rnd.String(),
                Id = Guid.NewGuid()
            };

            var taxField = ModelGeneratorService.GetRandomDefinition<TaxonomyFieldDefinition>(def =>
            {
                def.TermSetName = termSet.Name;
                def.TermId = term.Id.Value;
            });

            var taxonomyModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddRandomTermStore(termStore =>
                {
                    termStore.AddRandomTermGroup(group =>
                    {
                        group.AddTaxonomyTermSet(termSet, t =>
                        {
                            t.AddTaxonomyTerm(term);
                        });
                    });
                });
            });

            var fieldModel = SPMeta2Model
                  .NewSiteModel(site =>
                  {
                      site.AddTaxonomyField(taxField);
                  });

            TestModels(new[] { taxonomyModel, fieldModel });
        }

        #endregion
    }
}
