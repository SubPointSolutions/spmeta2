using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Docs.ProvisionSamples.Base;
using SPMeta2.Enumerations;
using SPMeta2.Syntax.Default;
using SubPointSolutions.Docs.Code.Enumerations;
using SubPointSolutions.Docs.Code.Metadata;

namespace SubPointSolutions.Docs.Views.Views.SPMeta2.reference
{
    [TestClass]
    [SampleMetadataTag(Name = BuiltInTagNames.SPRuntime, Value = BuiltInSPRuntimeTagValues.Foundation)]

    [SampleMetadataTag(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.Fields)]
    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.SiteModel)]

    //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
    public class LookupFieldDefinitionTests : ProvisionTestBase
    {
        #region methods

        [TestCategory("Docs.FieldDefinition")]
       

        [TestMethod]
        [TestCategory("Docs.LookupFieldDefinition")]

        [SampleMetadata(Title = "Add lookup field",
                        Description = ""
                        )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeployEmptyLookupField()
        {
            var emptyLookupField = new LookupFieldDefinition
            {
                Title = "Empty Lookup Field",
                InternalName = "m2EmptyLookupField",
                Group = "SPMeta2.Samples",
                Id = new Guid("B6387953-3967-4023-9D38-431F2C6A5E54")
            };

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site
                    .AddField(emptyLookupField);
            });

            DeployModel(model);
        }

        [TestCategory("Docs.FieldDefinition")]
       

        [TestMethod]
        [TestCategory("Docs.LookupFieldDefinition")]

        [SampleMetadata(Title = "Add lookup field binded to list",
                        Description = ""
                        )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeployLookupFieldBindedToList()
        {
            var leadTypeLookup = new LookupFieldDefinition
            {
                Title = "Lead Type",
                InternalName = "m2LeadType",
                Group = "SPMeta2.Samples",
                Id = new Guid("FEFC30A7-3B38-4034-BB2A-FFD538D46A63")
            };

            var lookupFieldModel = SPMeta2Model.NewSiteModel(site =>
            {
                site
                    .AddField(leadTypeLookup);
            });

            var leadRecords = new ListDefinition
            {
                Title = "Lead Records",
                Description = "A generic list.",
                TemplateType = BuiltInListTemplateTypeId.GenericList,
                Url = "m2LeadRecordsList"
            };

            var leadRecordTypes = new ListDefinition
            {
                Title = "Lead Record Types",
                Description = "A generic list.",
                TemplateType = BuiltInListTemplateTypeId.GenericList,
                Url = "m2LeadRecordTypesList"
            };

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web
                  .AddList(leadRecords, list =>
                  {
                      list.AddListFieldLink(leadTypeLookup);
                  })
                  .AddList(leadRecordTypes);
            });

            // 1. deploy lookup field without bindings
            DeployModel(lookupFieldModel);

            // 2. deploy lists
            DeployModel(webModel);

            // 3. update binding for the lookup field
            // LookupList/LookupListId could also be used
            leadTypeLookup.LookupListTitle = leadRecordTypes.Title;

            // 4. deploy lookup field again, so that it will be binded
            DeployModel(lookupFieldModel);
        }

        #endregion
    }
}