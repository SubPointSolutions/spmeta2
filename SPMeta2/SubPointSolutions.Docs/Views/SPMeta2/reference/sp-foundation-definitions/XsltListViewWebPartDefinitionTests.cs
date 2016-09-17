using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.CSOM.DefaultSyntax;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Webparts;

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

    [SampleMetadataTag(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.WebParts)]
    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.WebModel)]
    //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
    public class XsltListViewWebPartDefinitionTests : ProvisionTestBase
    {
        #region methods

      
        [TestMethod]
        [TestCategory("Docs.XsltListViewWebPartDefinition")]

        [SampleMetadata(Title = "Add XLVWP binded to list by Title",
            Description = ""
            )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanBindXsltListViewWebPartByListTitle()
        {
            var inventoryLibrary = new ListDefinition
            {
                Title = "Inventory library",
                Description = "A document library.",
                TemplateType = BuiltInListTemplateTypeId.DocumentLibrary,
                Url = "InventoryLibrary"
            };

            var xsltListView = new XsltListViewWebPartDefinition
            {
                Title = "Inventory Default View by List Title",
                Id = "m2InventoryView",
                ZoneIndex = 10,
                ZoneId = "Main",
                ListTitle = inventoryLibrary.Title
            };

            var webPartPage = new WebPartPageDefinition
            {
                Title = "M2 Xslt List View provision",
                FileName = "xslt-listview-webpart-provision.aspx",
                PageLayoutTemplate = BuiltInWebPartPageTemplates.spstd1
            };

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web
                  .AddList(inventoryLibrary)
                  .AddHostList(BuiltInListDefinitions.SitePages, list =>
                  {
                      list.AddWebPartPage(webPartPage, page =>
                      {
                          page.AddXsltListViewWebPart(xsltListView);
                      });
                  });
            });

            DeployModel(model);
        }

       
        [TestMethod]
        [TestCategory("Docs.XsltListViewWebPartDefinition")]

        [SampleMetadata(Title = "Add XLVWP binded to list by URL",
            Description = ""
            )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanBindXsltListViewWebPartByListUrl()
        {
            var booksLibrary = new ListDefinition
            {
                Title = "Books library",
                Description = "A document library.",
                TemplateType = BuiltInListTemplateTypeId.DocumentLibrary,
                Url = "BooksLibrary"
            };

            var xsltListView = new XsltListViewWebPartDefinition
            {
                Title = "Books Default View by List Url",
                Id = "m2BooksView",
                ZoneIndex = 10,
                ZoneId = "Main",
                ListUrl = booksLibrary.GetListUrl()
            };

            var webPartPage = new WebPartPageDefinition
            {
                Title = "M2 Xslt List View provision",
                FileName = "xslt-listview-webpart-provision.aspx",
                PageLayoutTemplate = BuiltInWebPartPageTemplates.spstd1
            };

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web
                  .AddList(booksLibrary)
                  .AddHostList(BuiltInListDefinitions.SitePages, list =>
                  {
                      list.AddWebPartPage(webPartPage, page =>
                      {
                          page.AddXsltListViewWebPart(xsltListView);
                      });
                  });
            });

            DeployModel(model);
        }

        
        [TestMethod]
        [TestCategory("Docs.XsltListViewWebPartDefinition")]

        [SampleMetadata(Title = "Add XLVWP binded to list view by Title",
            Description = ""
            )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanBindXsltListViewWebPartByListViewTitle()
        {
            var booksLibrary = new ListDefinition
            {
                Title = "Books library",
                Description = "A document library.",
                TemplateType = BuiltInListTemplateTypeId.DocumentLibrary,
                Url = "BooksLibrary"
            };

            var booksView = new ListViewDefinition
            {
                Title = "Popular Books",
                Fields = new Collection<string>
                {
                    BuiltInInternalFieldNames.Edit,
                    BuiltInInternalFieldNames.ID,
                    BuiltInInternalFieldNames.FileLeafRef
                },
                RowLimit = 10
            };

            var xsltListView = new XsltListViewWebPartDefinition
            {
                Title = "Popular Books binding by List View Title",
                Id = "m2PopularBooksView",
                ZoneIndex = 10,
                ZoneId = "Main",
                ListUrl = booksLibrary.GetListUrl(),
                ViewName = booksView.Title
            };

            var webPartPage = new WebPartPageDefinition
            {
                Title = "M2 Xslt List View provision",
                FileName = "xslt-listview-webpart-provision.aspx",
                PageLayoutTemplate = BuiltInWebPartPageTemplates.spstd1
            };

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web
                  .AddList(booksLibrary, list =>
                  {
                      list.AddListView(booksView);
                  })
                  .AddHostList(BuiltInListDefinitions.SitePages, list =>
                  {
                      list.AddWebPartPage(webPartPage, page =>
                      {
                          page.AddXsltListViewWebPart(xsltListView);
                      });
                  });
            });

            DeployModel(model);
        }

        #endregion
    }
}