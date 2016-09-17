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
    public class TaxonomyTermGroupDefinitionTests : ProvisionTestBase
    {
        #region methods

       

        [TestMethod]
        [TestCategory("Docs.TaxonomyTermGroupDefinition")]

        [SampleMetadata(Title = "Add taxonomy term group",
            Description = ""
            )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeploySimpleTaxonomyGroup()
        {
            var defaultSiteTermStore = new TaxonomyTermStoreDefinition
            {
                UseDefaultSiteCollectionTermStore = true
            };

            var clientsGroup = new TaxonomyTermGroupDefinition
            {
                Name = "Clients"
            };

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddTaxonomyTermStore(defaultSiteTermStore, termStore =>
                {
                    termStore
                        .AddTaxonomyTermGroup(clientsGroup);
                });
            });

            DeployModel(model);
        }

      
        [TestMethod]
        [TestCategory("Docs.TaxonomyTermGroupDefinition")]
        [SampleMetadata(Title = "Add taxonomy term groups",
            Description = ""
            )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeploySimpleTaxonomyGroups()
        {
            var defaultSiteTermStore = new TaxonomyTermStoreDefinition
            {
                UseDefaultSiteCollectionTermStore = true
            };

            var clientsGroup = new TaxonomyTermGroupDefinition
            {
                Name = "Clients"
            };

            var parthersGroup = new TaxonomyTermGroupDefinition
            {
                Name = "Parthers"
            };

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddTaxonomyTermStore(defaultSiteTermStore, termStore =>
                {
                    termStore
                        .AddTaxonomyTermGroup(clientsGroup)
                        .AddTaxonomyTermGroup(parthersGroup);
                });
            });

            DeployModel(model);
        }

        #endregion
    }
}