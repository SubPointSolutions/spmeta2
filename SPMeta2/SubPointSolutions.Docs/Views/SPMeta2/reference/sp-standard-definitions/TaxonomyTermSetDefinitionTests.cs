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
    public class TaxonomyTermSetDefinitionTests : ProvisionTestBase
    {
        #region methods

       
        [TestMethod]
        [TestCategory("Docs.TaxonomyTermSetDefinition")]

        [SampleMetadata(Title = "Add taxonomy termsets",
            Description = ""
            )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeploySimpleTaxonomyTermSets()
        {
            var defaultSiteTermStore = new TaxonomyTermStoreDefinition
            {
                UseDefaultSiteCollectionTermStore = true
            };

            var clientsGroup = new TaxonomyTermGroupDefinition
            {
                Name = "Clients"
            };

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

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddTaxonomyTermStore(defaultSiteTermStore, termStore =>
                {
                    termStore.AddTaxonomyTermGroup(clientsGroup, group =>
                    {
                        group
                            .AddTaxonomyTermSet(smallBusiness)
                            .AddTaxonomyTermSet(mediumBusiness)
                            .AddTaxonomyTermSet(enterpriseBusiness);
                    });
                });
            });

            DeployModel(model);
        }

        #endregion
    }
}