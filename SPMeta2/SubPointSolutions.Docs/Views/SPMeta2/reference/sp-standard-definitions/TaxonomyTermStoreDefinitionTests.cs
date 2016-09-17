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
    public class TaxonomyTermStoreDefinitionTests : ProvisionTestBase
    {
        #region methods

      
        [TestMethod]
        [TestCategory("Docs.TaxonomyTermStoreDefinition")]
        [SampleMetadata(Title = "Add taxonomy term store by Name",
            Description = ""
            )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void LookupTermStoreByName()
        {
            var mmsTermStore = new TaxonomyTermStoreDefinition
            {
                Name = "Managed Metadata Service"
            };

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddTaxonomyTermStore(mmsTermStore, termStore =>
                {
                    // do stuff, add groups, term sets
                });
            });

            DeployModel(model);
        }

        [TestMethod]
        [TestCategory("Docs.TaxonomyTermStoreDefinition")]
        [SampleMetadata(Title = "Add default taxonomy term store",
            Description = ""
            )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void LookupDefaultSiteTermStore()
        {
            var defaultSiteTermStore = new TaxonomyTermStoreDefinition
            {
                UseDefaultSiteCollectionTermStore = true
            };

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddTaxonomyTermStore(defaultSiteTermStore, termStore =>
                {
                    // do stuff, add groups, term sets
                });
            });

            DeployModel(model);
        }

        #endregion
    }
}