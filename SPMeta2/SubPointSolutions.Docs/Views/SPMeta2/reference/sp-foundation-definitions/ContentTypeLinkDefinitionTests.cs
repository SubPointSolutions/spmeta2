using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Docs.ProvisionSamples.Base;
using SPMeta2.Docs.ProvisionSamples.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Syntax.Default;
using SubPointSolutions.Docs.Code.Enumerations;
using SubPointSolutions.Docs.Code.Metadata;
using System;

namespace SPMeta2.Docs.ProvisionSamples.Provision.Definitions
{
    [TestClass]
    [SampleMetadataTag(Name = BuiltInTagNames.SPRuntime, Value = BuiltInSPRuntimeTagValues.Foundation)]

    [SampleMetadataTag(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.ContentTypes)]
    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.WebModel)]

    //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
    public class ContentTypeLinkDefinitionTests : ProvisionTestBase
    {
        #region methods

        [TestMethod]
        [TestCategory("Docs.ContentTypeLinkDefinition")]

        [SampleMetadata(Title = "Add content type to list",
                        Description = ""
                        )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeployListItemContentTypeLinkDefinition()
        {
            var customerInfoContentType = new ContentTypeDefinition
            {
                Name = "Customer Information",
                Id = new Guid("e33acc19-6d61-43b0-a313-4177065cd7c3"),
                ParentContentTypeId = BuiltInContentTypeId.Item,
                Group = "SPMeta2.Samples"
            };

            var customerInfoList = new ListDefinition
            {
                Title = "Customer Information",
                Description = "A list to store customer information.",
                TemplateType = BuiltInListTemplateTypeId.GenericList,
                Url = "CustomerInfo",
                ContentTypesEnabled = true
            };

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(customerInfoList, list =>
                {
                    list.AddContentTypeLink(customerInfoContentType);
                });
            });

            DeployModel(model);
        }


        [TestMethod]
        [TestCategory("Docs.ContentTypeLinkDefinition")]

        [SampleMetadata(Title = "Add content type to document library",
                        Description = ""
                        )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeployDocumentItemContentTypeLinkDefinition()
        {
            var customerReportContentType = new ContentTypeDefinition
            {
                Name = "Customer Report",
                Id = new Guid("1836765c-6264-479b-a95b-a553a3d14ba3"),
                ParentContentTypeId = BuiltInContentTypeId.Document,
                Group = "SPMeta2.Samples"
            };

            var customerInfoList = new ListDefinition
            {
                Title = "Customer Reports",
                Description = "A list to store customer reports.",
                TemplateType = BuiltInListTemplateTypeId.DocumentLibrary,
                Url = "CustomerReports",
                ContentTypesEnabled = true
            };

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(customerInfoList, list =>
                {
                    list.AddContentTypeLink(customerReportContentType);
                });
            });

            DeployModel(model);
        }

        #endregion
    }
}