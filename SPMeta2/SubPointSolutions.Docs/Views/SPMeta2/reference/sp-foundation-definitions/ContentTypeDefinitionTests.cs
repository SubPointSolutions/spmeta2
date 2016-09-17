using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Docs.ProvisionSamples.Base;
using SPMeta2.Docs.ProvisionSamples.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Syntax.Default;
using System;
using SubPointSolutions.Docs.Code.Definitions;
using SubPointSolutions.Docs.Code.Enumerations;
using SubPointSolutions.Docs.Code.Metadata;


namespace SPMeta2.Docs.ProvisionSamples.Provision.Definitions
{
    [TestClass]
    [SampleMetadataTag(Name = BuiltInTagNames.SPRuntime, Value = BuiltInSPRuntimeTagValues.Foundation)]

    [SampleMetadataTag(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.ContentTypes)]

    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.SiteModel)]
    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.WebModel)]

    //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
    public class ContentTypeDefinitionTests : ProvisionTestBase
    {
        #region methods

     
        [TestMethod]
        [TestCategory("Docs.ContentTypeDefinition")]

        [SampleMetadata(Title = "Add item content type",
                        Description = ""
                        )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeploySimpleListContentType()
        {
            var listContentType = new ContentTypeDefinition
            {
                Name = "Custom list item",
                Id = new Guid("79658c1e-3096-4c44-bd55-4228d01a5b97"),
                ParentContentTypeId = BuiltInContentTypeId.Item,
                Group = "SPMeta2.Samples"
            };

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site
                   .AddContentType(listContentType);
            });

            DeployModel(model);
        }


        [TestMethod]
        [TestCategory("Docs.ContentTypeDefinition")]

        [SampleMetadata(Title = "Add document content type",
                        Description = ""
                        )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeploySimpleDocumentContentType()
        {
            var documentContentType = new ContentTypeDefinition
            {
                Name = "Custom document",
                Id = new Guid("008e7c50-a271-4fcd-9f01-f18daad5bd7e"),
                ParentContentTypeId = BuiltInContentTypeId.Document,
                Group = "SPMeta2.Samples"
            };

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site
                   .AddContentType(documentContentType);
            });

            DeployModel(model);
        }

        [TestMethod]
        [TestCategory("Docs.ContentTypeDefinition")]

        [SampleMetadata(Title = "Add document set content type",
                        Description = ""
                        )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeploySimpleDocumentSetContentType()
        {
            var documentContentType = new ContentTypeDefinition
            {
                Name = "Custom document set",
                Id = new Guid("AAC93B98-F776-4D5C-9E6E-66F2DC45A467"),
                ParentContentTypeId = BuiltInContentTypeId.DocumentSet_Correct,
                Group = "SPMeta2.Samples"
            };

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site
                   .AddContentType(documentContentType);
            });

            DeployModel(model);
        }

        [TestMethod]
        [TestCategory("Docs.ContentTypeDefinition")]

        [SampleMetadata(Title = "Add several content types",
                        Description = ""
                        )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeploySimpleContentTypes()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site
                   .AddContentType(DocContentTypes.CustomerAccount)
                   .AddContentType(DocContentTypes.CustomerDocument);
            });

            DeployModel(model);
        }

    
        [TestMethod]
        [TestCategory("Docs.ContentTypeDefinition")]

        [SampleMetadata(Title = "Add content type with fields",
                        Description = ""
                        )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeploySimpleContentTypesWithFields()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site
                    .AddField(DocFields.Clients.ClientCredit)
                    .AddField(DocFields.Clients.ClientDebit)
                    .AddField(DocFields.Clients.ClientDescription)
                    .AddField(DocFields.Clients.ClientNumber)
                    .AddField(DocFields.Clients.ClientWebSite)

                   .AddContentType(DocContentTypes.CustomerAccount, contentType =>
                   {
                       contentType
                         .AddContentTypeFieldLink(DocFields.Clients.ClientCredit)
                         .AddContentTypeFieldLink(DocFields.Clients.ClientDebit)
                         .AddContentTypeFieldLink(DocFields.Clients.ClientWebSite);
                   })
                   .AddContentType(DocContentTypes.CustomerDocument, contentType =>
                   {
                       contentType
                          .AddContentTypeFieldLink(DocFields.Clients.ClientDescription)
                          .AddContentTypeFieldLink(DocFields.Clients.ClientNumber);
                   });
            });

            DeployModel(model);
        }

        [TestMethod]
        [TestCategory("Docs.ContentTypeDefinition")]

        [SampleMetadata(Title = "Add parent-child content types",
                        Description = ""
                        )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeployHierarhicalContentTypes()
        {
            var rootDocumentContentType = new ContentTypeDefinition
            {
                Name = "A root document",
                Id = new Guid("b0ec3794-8bf3-49ed-b8d1-24a4df5ac75b"),
                ParentContentTypeId = BuiltInContentTypeId.Document,
                Group = "SPMeta2.Samples"
            };

            var childDocumentContentType = new ContentTypeDefinition
            {
                Name = "A child document",
                Id = new Guid("84ab43ee-1f9d-4436-a9de-868bd7a36400"),
                // use GetContentTypeId() to get the content type ID and refer as a parent ID
                ParentContentTypeId = rootDocumentContentType.GetContentTypeId(),
                Group = "SPMeta2.Samples"
            };

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site
                   .AddContentType(rootDocumentContentType)
                   .AddContentType(childDocumentContentType);
            });

            DeployModel(model);
        }

        #endregion
    }
}
