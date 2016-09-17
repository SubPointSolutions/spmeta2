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

    [SampleMetadataTagAttribute(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.ContentTypes)]

    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.WebModel)]

    //[SampleMetadataTagAttribute(Name = BuiltInTagNames.SampleHidden)]
    public class HideContentTypeFieldLinksDefinitionTests : ProvisionTestBase
    {
        #region methods


        [TestMethod]
        [TestCategory("Docs.HideContentTypeFieldLinksDefinition")]

        [SampleMetadata(Title = "Reorder content type fields",
                        Description = ""
                        )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanHideContentTypeFields()
        {
            var hiddenNotesField = new NoteFieldDefinition
            {
                Title = "Hidden Notes",
                InternalName = "m2_HiddenNotes",
                Group = "SPMeta2.Samples",
                Id = new Guid("13C47F4C-F3BA-431E-A76B-FCC03FED4E9B"),
            };

            var publicNotesField = new NoteFieldDefinition
            {
                Title = "Publis Notes",
                InternalName = "m2_PublicNotes",
                Group = "SPMeta2.Samples",
                Id = new Guid("BACEE8AA-90B4-4268-8257-EEA0706942E4"),
            };

            var hiddenNotesContentType = new ContentTypeDefinition
            {
                Name = "M2 Hidden Notes",
                Id = new Guid("1166D859-CC4B-4A5F-A1F3-28BE508C5A92"),
                ParentContentTypeId = BuiltInContentTypeId.Item,
                Group = "SPMeta2.Samples"
            };

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site
                    .AddField(hiddenNotesField)
                    .AddField(publicNotesField)
                    .AddContentType(hiddenNotesContentType, contentType =>
                    {
                        contentType
                            .AddContentTypeFieldLink(hiddenNotesField)
                            .AddContentTypeFieldLink(publicNotesField)
                            .AddHideContentTypeFieldLinks(new HideContentTypeFieldLinksDefinition
                            {
                                Fields = new List<FieldLinkValue>
                                {
                                   new FieldLinkValue{ Id = BuiltInFieldId.Title },
                                   new FieldLinkValue{ Id = hiddenNotesField.Id }
                                }
                            });
                    });
            });

            DeployModel(model);
        }

        #endregion
    }
}