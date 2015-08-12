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
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Modern;

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
        public void CanDeploy_TaxonomyFieldAsEmptySingleSelect()
        {
            var taxField = ModelGeneratorService.GetRandomDefinition<TaxonomyFieldDefinition>(def =>
            {
                def.IsMulti = false;

                def.UseDefaultSiteCollectionTermStore = false;
                def.SspId = null;
                def.SspName = string.Empty;
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
        public void CanDeploy_TaxonomyFieldAsEmptyMiltiSelect()
        {
            var taxField = ModelGeneratorService.GetRandomDefinition<TaxonomyFieldDefinition>(def =>
            {
                def.IsMulti = true;

                def.UseDefaultSiteCollectionTermStore = false;
                def.SspId = null;
                def.SspName = string.Empty;
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

            TestModels(new ModelNode[] { taxonomyModel, fieldModel });
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

            TestModels(new ModelNode[] { taxonomyModel, fieldModel });
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

            TestModels(new ModelNode[] { taxonomyModel, fieldModel });
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

            TestModels(new ModelNode[] { taxonomyModel, fieldModel });
        }

        #endregion

        #region TermGroups

        [TestCategory("Regression.Scenarios.Taxonomy.TaxonomyField.TaxonomyGroups")]
        [TestMethod]
        public void CanDeploy_TaxonomyFieldBindedToTermSet_ByName_Within_GroupByName()
        {
            Internal_TaxonomyFieldBinded_ByName_Within_GroupByName(true, false);
        }

        [TestCategory("Regression.Scenarios.Taxonomy.TaxonomyField.TaxonomyGroups")]
        [TestMethod]
        public void CanDeploy_TaxonomyFieldBindedToTerm_ByName_Within_GroupByName()
        {
            Internal_TaxonomyFieldBinded_ByName_Within_GroupByName(false, true);
        }

        [TestCategory("Regression.Scenarios.Taxonomy.TaxonomyField.TaxonomyGroups")]
        [TestMethod]
        public void CanDeploy_TaxonomyFieldBindedToTermSetAndTerm_ByName_Within_GroupByName()
        {
            Internal_TaxonomyFieldBinded_ByName_Within_GroupByName(true, true);
        }

        private void Internal_TaxonomyFieldBinded_ByName_Within_GroupByName(bool isTermSet, bool isTerm)
        {
            var termSetName = Rnd.String();
            var termName = Rnd.String();

            var term1Id = Rnd.Guid();
            var term2Id = Rnd.Guid();
            var term3Id = Rnd.Guid();

            // same same names
            var termSet1 = new TaxonomyTermSetDefinition
            {
                Name = termSetName,
                Description = Rnd.String(),
            };

            var termSet2 = new TaxonomyTermSetDefinition
            {
                Name = termSetName,
                Description = Rnd.String(),
            };

            var termSet3 = new TaxonomyTermSetDefinition
            {
                Name = termSetName,
                Description = Rnd.String(),
            };

            var term1 = new TaxonomyTermDefinition
            {
                Name = termName,
                Description = Rnd.String(),
                Id = term1Id
            };

            var term2 = new TaxonomyTermDefinition
            {
                Name = termName,
                Description = Rnd.String(),
                Id = term2Id
            };

            var term3 = new TaxonomyTermDefinition
            {
                Name = termName,
                Description = Rnd.String(),
                Id = term3Id
            };

            var termGroup1 = ModelGeneratorService.GetRandomDefinition<TaxonomyTermGroupDefinition>(def =>
            {

            });

            var termGroup2 = ModelGeneratorService.GetRandomDefinition<TaxonomyTermGroupDefinition>(def =>
            {

            });

            var termGroup3 = ModelGeneratorService.GetRandomDefinition<TaxonomyTermGroupDefinition>(def =>
            {

            });

            // binding to the 2nd one
            var taxField = ModelGeneratorService.GetRandomDefinition<TaxonomyFieldDefinition>(def =>
            {
                def.TermGroupName = termGroup2.Name;

                // these should be resolvbed as we are scoped to the 2nd group
                if (isTermSet)
                    def.TermSetName = termSet2.Name;

                if (isTerm)
                    def.TermName = term2.Name;
            });

            var taxonomyModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddRandomTermStore(termStore =>
                {
                    termStore.AddTaxonomyTermGroup(termGroup1, group =>
                    {
                        group.AddTaxonomyTermSet(termSet1, t =>
                        {
                            t.AddTaxonomyTerm(term1);
                        });
                    });

                    termStore.AddTaxonomyTermGroup(termGroup2, group =>
                    {
                        group.AddTaxonomyTermSet(termSet2, t =>
                        {
                            t.AddTaxonomyTerm(term2);
                        });
                    });

                    termStore.AddTaxonomyTermGroup(termGroup3, group =>
                    {
                        group.AddTaxonomyTermSet(termSet3, t =>
                        {
                            t.AddTaxonomyTerm(term3);
                        });
                    });
                });
            });

            var fieldModel = SPMeta2Model
                  .NewSiteModel(site =>
                  {
                      site.AddTaxonomyField(taxField);
                  });

            TestModels(new ModelNode[] { taxonomyModel, fieldModel });
        }

        [TestCategory("Regression.Scenarios.Taxonomy.TaxonomyField.TaxonomyGroups")]
        [TestMethod]
        public void CanDeploy_TaxonomyFieldBindedToTermSet_ByName_Within_GroupById()
        {
            Internal_TaxonomyFieldBinded_ByName_Within_GroupById(true, false);
        }

        [TestCategory("Regression.Scenarios.Taxonomy.TaxonomyField.TaxonomyGroups")]
        [TestMethod]
        public void CanDeploy_TaxonomyFieldBindedToTerm_ByName_Within_GroupById()
        {
            Internal_TaxonomyFieldBinded_ByName_Within_GroupById(false, true);
        }

        [TestCategory("Regression.Scenarios.Taxonomy.TaxonomyField.TaxonomyGroups")]
        [TestMethod]
        public void CanDeploy_TaxonomyFieldBindedToTermSetAndTerm_ByName_Within_GroupById()
        {
            Internal_TaxonomyFieldBinded_ByName_Within_GroupById(true, true);
        }

        private void Internal_TaxonomyFieldBinded_ByName_Within_GroupById(bool isTermSet, bool isTerm)
        {
            var termSetName = Rnd.String();
            var termName = Rnd.String();

            var term1Id = Rnd.Guid();
            var term2Id = Rnd.Guid();
            var term3Id = Rnd.Guid();

            // same same names
            var termSet1 = new TaxonomyTermSetDefinition
            {
                Name = termSetName,
                Description = Rnd.String(),
            };

            var termSet2 = new TaxonomyTermSetDefinition
            {
                Name = termSetName,
                Description = Rnd.String(),
            };

            var termSet3 = new TaxonomyTermSetDefinition
            {
                Name = termSetName,
                Description = Rnd.String(),
            };

            var term1 = new TaxonomyTermDefinition
            {
                Name = termName,
                Description = Rnd.String(),
                Id = term1Id
            };

            var term2 = new TaxonomyTermDefinition
            {
                Name = termName,
                Description = Rnd.String(),
                Id = term2Id
            };

            var term3 = new TaxonomyTermDefinition
            {
                Name = termName,
                Description = Rnd.String(),
                Id = term3Id
            };

            var termGroup1 = ModelGeneratorService.GetRandomDefinition<TaxonomyTermGroupDefinition>(def =>
            {

            });

            var termGroup2 = ModelGeneratorService.GetRandomDefinition<TaxonomyTermGroupDefinition>(def =>
            {
                def.Id = Guid.NewGuid();
            });

            var termGroup3 = ModelGeneratorService.GetRandomDefinition<TaxonomyTermGroupDefinition>(def =>
            {

            });

            // binding to the 2nd one
            var taxField = ModelGeneratorService.GetRandomDefinition<TaxonomyFieldDefinition>(def =>
            {
                def.TermGroupName = string.Empty;
                def.TermGroupId = termGroup2.Id;

                // these should be resolvbed as we are scoped to the 2nd group
                if (isTermSet)
                    def.TermSetName = termSet2.Name;

                if (isTerm)
                    def.TermName = term2.Name;
            });

            var taxonomyModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddRandomTermStore(termStore =>
                {
                    termStore.AddTaxonomyTermGroup(termGroup1, group =>
                    {
                        group.AddTaxonomyTermSet(termSet1, t =>
                        {
                            t.AddTaxonomyTerm(term1);
                        });
                    });

                    termStore.AddTaxonomyTermGroup(termGroup2, group =>
                    {
                        group.AddTaxonomyTermSet(termSet2, t =>
                        {
                            t.AddTaxonomyTerm(term2);
                        });
                    });

                    termStore.AddTaxonomyTermGroup(termGroup3, group =>
                    {
                        group.AddTaxonomyTermSet(termSet3, t =>
                        {
                            t.AddTaxonomyTerm(term3);
                        });
                    });
                });
            });

            var fieldModel = SPMeta2Model
                  .NewSiteModel(site =>
                  {
                      site.AddTaxonomyField(taxField);
                  });

            TestModels(new ModelNode[] { taxonomyModel, fieldModel });
        }

        [TestCategory("Regression.Scenarios.Taxonomy.TaxonomyField.TaxonomyGroups")]
        [TestMethod]
        public void CanDeploy_TaxonomyFieldBindedToTermSet_ById_Within_SiteCollectionGroup()
        {
            Internal_TaxonomyFieldBinded_ById_Within_SiteCollectionGroup(true, false);
        }

        [TestCategory("Regression.Scenarios.Taxonomy.TaxonomyField.TaxonomyGroups")]
        [TestMethod]
        public void CanDeploy_TaxonomyFieldBindedToTerm_ById_Within_SiteCollectionGroup()
        {
            Internal_TaxonomyFieldBinded_ById_Within_SiteCollectionGroup(false, true);
        }

        [TestCategory("Regression.Scenarios.Taxonomy.TaxonomyField.TaxonomyGroups")]
        [TestMethod]
        public void CanDeploy_TaxonomyFieldBindedToTermSetAndTerm_ById_Within_SiteCollectionGroup()
        {
            Internal_TaxonomyFieldBinded_ById_Within_SiteCollectionGroup(true, true);
        }

        private void Internal_TaxonomyFieldBinded_ById_Within_SiteCollectionGroup(bool isTermSet, bool isTerm)
        {

            var termSetName = Rnd.String();
            var termName = Rnd.String();

            var termSet1Id = Rnd.Guid();
            var termSet2Id = Rnd.Guid();
            var termSet3Id = Rnd.Guid();

            var term1Id = Rnd.Guid();
            var term2Id = Rnd.Guid();
            var term3Id = Rnd.Guid();

            // same same names
            var termSet1 = new TaxonomyTermSetDefinition
            {
                Name = termSetName,
                Description = Rnd.String(),
            };

            var termSet2 = new TaxonomyTermSetDefinition
            {
                Name = termSetName,
                Id = termSet2Id,
                Description = Rnd.String(),
            };

            var termSet3 = new TaxonomyTermSetDefinition
            {
                Name = termSetName,
                Description = Rnd.String(),
            };

            var term1 = new TaxonomyTermDefinition
            {
                Name = termName,
                Description = Rnd.String(),
                Id = term1Id
            };

            var term2 = new TaxonomyTermDefinition
            {
                Name = termName,
                Description = Rnd.String(),
                Id = term2Id,
            };

            var term3 = new TaxonomyTermDefinition
            {
                Name = termName,
                Description = Rnd.String(),
                Id = term3Id
            };

            var termGroup1 = ModelGeneratorService.GetRandomDefinition<TaxonomyTermGroupDefinition>(def =>
            {

            });

            var termGroup2 = ModelGeneratorService.GetRandomDefinition<TaxonomyTermGroupDefinition>(def =>
            {
                def.IsSiteCollectionGroup = true;
            });

            var termGroup3 = ModelGeneratorService.GetRandomDefinition<TaxonomyTermGroupDefinition>(def =>
            {

            });

            // binding to the 2nd one
            var taxField = ModelGeneratorService.GetRandomDefinition<TaxonomyFieldDefinition>(def =>
            {
                def.IsSiteCollectionGroup = true;

                // these should be resolvbed as we are scoped to the 2nd group within site collection group

                if (isTermSet)
                    def.TermSetId = termSet2.Id;

                if (isTerm)
                    def.TermId = term2.Id;
            });

            var taxonomyModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddRandomTermStore(termStore =>
                {
                    termStore.AddTaxonomyTermGroup(termGroup1, group =>
                    {
                        group.AddTaxonomyTermSet(termSet1, t =>
                        {
                            t.AddTaxonomyTerm(term1);
                        });
                    });

                    termStore.AddTaxonomyTermGroup(termGroup2, group =>
                    {
                        group.AddTaxonomyTermSet(termSet2, t =>
                        {
                            t.AddTaxonomyTerm(term2);
                        });
                    });

                    termStore.AddTaxonomyTermGroup(termGroup3, group =>
                    {
                        group.AddTaxonomyTermSet(termSet3, t =>
                        {
                            t.AddTaxonomyTerm(term3);
                        });
                    });
                });
            });

            var fieldModel = SPMeta2Model
                  .NewSiteModel(site =>
                  {
                      site.AddTaxonomyField(taxField);
                  });

            TestModels(new ModelNode[] { taxonomyModel, fieldModel });
        }

        #endregion
    }
}
