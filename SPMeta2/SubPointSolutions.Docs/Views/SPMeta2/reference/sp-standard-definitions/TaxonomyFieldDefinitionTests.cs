using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SPMeta2.Docs.ProvisionSamples.Base;
using SPMeta2.Docs.ProvisionSamples.Definitions;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;
using SubPointSolutions.Docs.Code.Enumerations;
using SubPointSolutions.Docs.Code.Metadata;

namespace SPMeta2.Docs.ProvisionSamples.Provision.Definitions
{
    [TestClass]
    [SampleMetadataTag(Name = BuiltInTagNames.SPRuntime, Value = BuiltInSPRuntimeTagValues.Standard)]

    [SampleMetadataTag(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.Fields)]
    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.SiteModel)]

    //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
    public class TaxonomyFieldDefinitionTests : ProvisionTestBase
    {
        #region methods

        [TestMethod]
        [TestCategory("Docs.FieldDefinition")]

        [SampleMetadata(Title = "Add taxonomy field",
                        Description = ""
                        )]
        [SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
       
        public void CanDeployTaxonomyField()
        {
            // define a taxonomy
            // term store -> group -> term set -> terms
            var taxDefaultTermStore = new TaxonomyTermStoreDefinition
            {
                UseDefaultSiteCollectionTermStore = true
            };

            var taxTermGroup = new TaxonomyTermGroupDefinition
            {
                Name = "M2 Taxonomy"
            };

            var taxTermSet = new TaxonomyTermSetDefinition
            {
                Name = "Locations"
            };

            var taxTermLondon = new TaxonomyTermDefinition
            {
                Name = "London"
            };

            var taxTermSydney = new TaxonomyTermDefinition
            {
                Name = "Sydney"
            };

            // define the field
            var location = new TaxonomyFieldDefinition
            {
                Title = "Location",
                InternalName = "dcs_LocationTax",
                Group = "SPMeta2.Samples",
                Id = new Guid("FE709AC2-E3A1-4A25-8F71-3480667CD98F"),
                IsMulti = false,
                UseDefaultSiteCollectionTermStore = true,
                TermSetName = taxTermSet.Name
            };

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site
                    .AddHostTaxonomyTermStore(taxDefaultTermStore, store =>
                    {
                        store.AddTaxonomyTermGroup(taxTermGroup, group =>
                        {
                            group.AddTaxonomyTermSet(taxTermSet, termSet =>
                            {
                                termSet
                                    .AddTaxonomyTerm(taxTermLondon)
                                    .AddTaxonomyTerm(taxTermSydney);
                            });
                        });
                    })
                    .AddTaxonomyField(location);
            });

            DeployModel(model);
        }

        #endregion
    }
}