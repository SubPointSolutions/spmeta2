using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Definitions.Fields;

using SPMeta2.Docs.ProvisionSamples.Base;
using SPMeta2.Docs.ProvisionSamples.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Syntax.Default;
using SubPointSolutions.Docs.Code.Enumerations;
using SubPointSolutions.Docs.Code.Metadata;

namespace SPMeta2.Docs.ProvisionSamples.Provision.Definitions
{
    [TestClass]
    [SampleMetadataTag(Name = BuiltInTagNames.SPRuntime, Value = BuiltInSPRuntimeTagValues.Foundation)]

    [SampleMetadataTag(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.ContentTypes)]
    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.SiteModel)]
    //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
    public class UniqueContentTypeFieldsOrderDefinitionTests : ProvisionTestBase
    {
        #region methods


        [TestMethod]
        [TestCategory("Docs.UniqueContentTypeFieldsOrderDefinition")]

        [SampleMetadata(Title = "Reorder content type fields",
                    Description = ""
                    )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanReorderContentTypeFields()
        {
            var debitField = new NumberFieldDefinition
            {
                Title = "Debit",
                InternalName = "m2_MDebit",
                Group = "SPMeta2.Samples",
                Id = new Guid("2901EA31-CB32-4EE7-8482-9354C843F264"),
            };

            var creditField = new NumberFieldDefinition
            {
                Title = "Credit",
                InternalName = "m2_MCredit",
                Group = "SPMeta2.Samples",
                Id = new Guid("2F62D945-AFF8-4ACF-B090-4BB5A8FB13C9"),
            };

            var totalField = new NumberFieldDefinition
            {
                Title = "Total",
                InternalName = "m2_MTotal",
                Group = "SPMeta2.Samples",
                Id = new Guid("07D7B101-3F95-4413-B5D0-0EAA75E31697"),
            };

            var balanceContentType = new ContentTypeDefinition
            {
                Name = "M2 Balance",
                Id = new Guid("1861F08E-4E76-4DA3-9CE9-842B481FD0DA"),
                ParentContentTypeId = BuiltInContentTypeId.Item,
                Group = "SPMeta2.Samples"
            };

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site
                    .AddField(debitField)
                    .AddField(creditField)
                    .AddField(totalField)
                    .AddContentType(balanceContentType, contentType =>
                    {
                        contentType
                            .AddContentTypeFieldLink(totalField)
                            .AddContentTypeFieldLink(debitField)
                            .AddContentTypeFieldLink(creditField)
                            .AddUniqueContentTypeFieldsOrder(new UniqueContentTypeFieldsOrderDefinition
                            {
                                Fields = new List<FieldLinkValue>
                                {
                                    new FieldLinkValue{ Id = BuiltInFieldId.Title },
                                    new FieldLinkValue{ Id = creditField.Id },
                                    new FieldLinkValue{ Id = debitField.Id },
                                    new FieldLinkValue{ Id = totalField.Id }
                                }
                            });
                    });
            });

            DeployModel(model);
        }

        #endregion
    }
}