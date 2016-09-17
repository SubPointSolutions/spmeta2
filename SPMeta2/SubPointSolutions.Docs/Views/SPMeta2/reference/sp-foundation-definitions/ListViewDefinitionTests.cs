using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Docs.ProvisionSamples.Base;
using SPMeta2.Docs.ProvisionSamples.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Syntax.Default;
using System.Collections.ObjectModel;
using System.Text;
using SubPointSolutions.Docs.Code.Enumerations;
using SubPointSolutions.Docs.Code.Metadata;

namespace SPMeta2.Docs.ProvisionSamples.Provision.Definitions
{
    [TestClass]

    [SampleMetadataTag(Name = BuiltInTagNames.SPRuntime, Value = BuiltInSPRuntimeTagValues.Foundation)]

    [SampleMetadataTag(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.ListViews)]
    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.WebModel)]

    //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
    public class ListViewDefinitionTests : ProvisionTestBase
    {
        #region methods

        [TestMethod]
        [TestCategory("Docs.ListViewDefinition")]

        [SampleMetadata(Title = "Add list view",
                Description = ""
                )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeploySimpleListViews()
        {
            var approvedDocuments = new ListViewDefinition
            {
                Title = "Approved Documents",
                Fields = new Collection<string>
                {
                    BuiltInInternalFieldNames.ID,
                    BuiltInInternalFieldNames.FileLeafRef
                }
            };

            var inProgressDocuments = new ListViewDefinition
            {
                Title = "In Progress Documents",
                Fields = new Collection<string>
                {
                    BuiltInInternalFieldNames.ID,
                    BuiltInInternalFieldNames.FileLeafRef
                }
            };

            var documentLibrary = new ListDefinition
            {
                Title = "CustomerDocuments",
                Description = "A customr document library.",
                TemplateType = BuiltInListTemplateTypeId.DocumentLibrary,
                Url = "CustomerDocuments"
            };

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(documentLibrary, list =>
                {
                    list.AddListView(approvedDocuments);
                    list.AddListView(inProgressDocuments);

                });
            });

            DeployModel(model);
        }


        [TestMethod]
        [TestCategory("Docs.ListViewDefinition")]

        [SampleMetadata(Title = "Add list view with URL",
                Description = ""
                )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeploySimpleListViewsWithCustomUrl()
        {
            var returnedDocuments = new ListViewDefinition
            {
                Title = "Returned Documents",
                Url = "Returned.aspx",
                Fields = new Collection<string>
                {
                    BuiltInInternalFieldNames.ID,
                    BuiltInInternalFieldNames.FileLeafRef
                }
            };

            var documentLibrary = new ListDefinition
            {
                Title = "CustomerDocuments",
                Description = "A customr document library.",
                TemplateType = BuiltInListTemplateTypeId.DocumentLibrary,
                Url = "CustomerDocuments"
            };

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(documentLibrary, list =>
                {
                    list.AddListView(returnedDocuments);
                });
            });

            DeployModel(model);
        }

      

        [TestMethod]
        [TestCategory("Docs.ListViewDefinition")]
        [SampleMetadata(Title = "Add list view with CAML",
                Description = ""
                )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeploySimpleListViewsWithCAMLQuery()
        {
            var createdQuery = new StringBuilder();

            createdQuery.Append("<Where>");
            createdQuery.Append("</Where>");
            createdQuery.Append("<OrderBy>");
            createdQuery.Append("  <FieldRef Name='ID' Ascending='FALSE'/>");
            createdQuery.Append("</OrderBy>");

            var lastTenCreatedDocuments = new ListViewDefinition
            {
                Title = "Last 10 Created Documents",
                RowLimit = 10,
                Query = createdQuery.ToString(),
                Fields = new Collection<string>
                {
                    BuiltInInternalFieldNames.ID,
                    BuiltInInternalFieldNames.FileLeafRef
                }
            };

            var editedQuery = new StringBuilder();

            editedQuery.Append("<Where>");
            editedQuery.Append("</Where>");
            editedQuery.Append("<OrderBy>");
            editedQuery.Append("  <FieldRef Name='Modified' Ascending='FALSE'/>");
            editedQuery.Append("</OrderBy>");

            var lastTenEditedDocuments = new ListViewDefinition
            {
                Title = "Last 10 Edited Documents",
                RowLimit = 10,
                Query = editedQuery.ToString(),
                Fields = new Collection<string>
                {
                    BuiltInInternalFieldNames.ID,
                    BuiltInInternalFieldNames.FileLeafRef
                }
            };

            var documentLibrary = new ListDefinition
            {
                Title = "CustomerDocuments",
                Description = "A customr document library.",
                TemplateType = BuiltInListTemplateTypeId.DocumentLibrary,
                Url = "CustomerDocuments"
            };

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(documentLibrary, list =>
                {
                    list.AddListView(lastTenCreatedDocuments);
                    list.AddListView(lastTenEditedDocuments);
                });
            });

            DeployModel(model);
        }

        #endregion
    }
}