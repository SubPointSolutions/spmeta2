using Microsoft.VisualStudio.TestTools.UnitTesting;

using SPMeta2.Docs.ProvisionSamples.Base;
using SPMeta2.Docs.ProvisionSamples.Definitions;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Syntax.Default;
using SPMeta2.Standard.Syntax;
using SubPointSolutions.Docs.Code.Enumerations;
using SubPointSolutions.Docs.Code.Metadata;

namespace SPMeta2.Docs.ProvisionSamples.Provision.Definitions
{
    [TestClass]
    [SampleMetadataTag(Name = BuiltInTagNames.SPRuntime, Value = BuiltInSPRuntimeTagValues.Standard)]

    [SampleMetadataTag(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.Taxonomy)]
    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.SiteModel)]
    //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
    public class TaxonomyTermDefinitionTests : ProvisionTestBase
    {
        #region methods

        [TestMethod]
        [TestCategory("Docs.TaxonomyTermDefinition")]

        [SampleMetadata(Title = "Add taxonomy terms",
            Description = ""
            )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeploySimpleTaxonomyTerms()
        {
            // define term store
            var defaultSiteTermStore = new TaxonomyTermStoreDefinition
            {
                UseDefaultSiteCollectionTermStore = true
            };

            // define group
            var clientsGroup = new TaxonomyTermGroupDefinition
            {
                Name = "Clients"
            };

            // define term sets
            var smallBusiness = new TaxonomyTermSetDefinition
            {
                Name = "Small Business"
            };

            var mediumBusiness = new TaxonomyTermSetDefinition
            {
                Name = "Medium Business"
            };

            var enterpriseBusiness = new TaxonomyTermSetDefinition
            {
                Name = "Enterprise Business"
            };

            // define terms
            var microsoft = new TaxonomyTermDefinition
            {
                Name = "Microsoft"
            };

            var apple = new TaxonomyTermDefinition
            {
                Name = "Apple"
            };

            var oracle = new TaxonomyTermDefinition
            {
                Name = "Oracle"
            };

            var subPointSolutions = new TaxonomyTermDefinition
            {
                Name = "SubPoint Solutions"
            };

            // setup the model 
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddTaxonomyTermStore(defaultSiteTermStore, termStore =>
                {
                    termStore.AddTaxonomyTermGroup(clientsGroup, group =>
                    {
                        group
                            .AddTaxonomyTermSet(smallBusiness, termSet =>
                            {
                                termSet.AddTaxonomyTerm(subPointSolutions);
                            })
                            .AddTaxonomyTermSet(mediumBusiness)
                            .AddTaxonomyTermSet(enterpriseBusiness, termSet =>
                            {
                                termSet
                                    .AddTaxonomyTerm(microsoft)
                                    .AddTaxonomyTerm(apple)
                                    .AddTaxonomyTerm(oracle);
                            });
                    });
                });
            });

            DeployModel(model);
        }

        #endregion
    }
}