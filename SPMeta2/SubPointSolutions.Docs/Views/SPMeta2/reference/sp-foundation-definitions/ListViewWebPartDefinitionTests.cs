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
    public class ListViewWebPartDefinitionTests : ProvisionTestBase
    {
        #region methods

       
        [TestMethod]
        [TestCategory("Docs.ListViewWebPartDefinition")]

        [SampleMetadata(Title = "Add LVWP binded to list by Title",
                Description = ""
                )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanBindListViewWebPartByListTitle()
        {
            var travelRequests = new ListDefinition
            {
                Title = "Travel Requests",
                Description = "A document library.",
                TemplateType = BuiltInListTemplateTypeId.DocumentLibrary,
                Url = "m2TravelRequests"
            };

            var listView = new ListViewWebPartDefinition
            {
                Title = "Travel Request Default View by List Title",
                Id = "m2TravelRequestsView",
                ZoneIndex = 10,
                ZoneId = "Main",
                ListTitle = travelRequests.Title
            };

            var webPartPage = new WebPartPageDefinition
            {
                Title = "M2 List View provision",
                FileName = "listview-webpart-provision.aspx",
                PageLayoutTemplate = BuiltInWebPartPageTemplates.spstd1
            };

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web
                  .AddList(travelRequests)
                  .AddHostList(BuiltInListDefinitions.SitePages, list =>
                  {
                      list.AddWebPartPage(webPartPage, page =>
                      {
                          page.AddListViewWebPart(listView);
                      });
                  });
            });

            DeployModel(model);
        }

       
        [TestMethod]
        [TestCategory("Docs.ListViewWebPartDefinition")]

        [SampleMetadata(Title = "Add LVWP binded to list by URL",
                Description = ""
                )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanBindListViewWebPartByListUrl()
        {
            var annualReviewsLibrary = new ListDefinition
            {
                Title = "Annual Reviews",
                Description = "A document library.",
                TemplateType = BuiltInListTemplateTypeId.DocumentLibrary,
                Url = "m2AnnualReviews"
            };

            var listView = new ListViewWebPartDefinition
            {
                Title = "Annual Reviews Default View by List Url",
                Id = "m2AnnualReviewsView",
                ZoneIndex = 10,
                ZoneId = "Main",
                ListUrl = annualReviewsLibrary.GetListUrl()
            };

            var webPartPage = new WebPartPageDefinition
            {
                Title = "M2 List View provision",
                FileName = "listview-webpart-provision.aspx",
                PageLayoutTemplate = BuiltInWebPartPageTemplates.spstd1
            };

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web
                  .AddList(annualReviewsLibrary)
                  .AddHostList(BuiltInListDefinitions.SitePages, list =>
                  {
                      list.AddWebPartPage(webPartPage, page =>
                      {
                          page.AddListViewWebPart(listView);
                      });
                  });
            });

            DeployModel(model);
        }

     
        [TestMethod]
        [TestCategory("Docs.ListViewWebPartDefinition")]

        [SampleMetadata(Title = "Add LVWP binded to list view by Title",
                Description = ""
                )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanBindListViewWebPartByListViewTitle()
        {
            var incidentsLibrary = new ListDefinition
            {
                Title = "Incidents library",
                Description = "A document library.",
                TemplateType = BuiltInListTemplateTypeId.DocumentLibrary,
                Url = "m2Incidents"
            };

            var incidentsView = new ListViewDefinition
            {
                Title = "Last Incidents",
                Fields = new Collection<string>
                {
                    BuiltInInternalFieldNames.Edit,
                    BuiltInInternalFieldNames.ID,
                    BuiltInInternalFieldNames.FileLeafRef
                },
                RowLimit = 10
            };

            var listView = new ListViewWebPartDefinition
            {
                Title = "Last Incidents binding by List View Title",
                Id = "m2LastIncidentsView",
                ZoneIndex = 10,
                ZoneId = "Main",
                ListUrl = incidentsLibrary.GetListUrl(),
                ViewName = incidentsView.Title
            };

            var webPartPage = new WebPartPageDefinition
            {
                Title = "M2 List View provision",
                FileName = "listview-webpart-provision.aspx",
                PageLayoutTemplate = BuiltInWebPartPageTemplates.spstd1
            };

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web
                  .AddList(incidentsLibrary, list =>
                  {
                      list.AddListView(incidentsView);
                  })
                  .AddHostList(BuiltInListDefinitions.SitePages, list =>
                  {
                      list.AddWebPartPage(webPartPage, page =>
                      {
                          page.AddListViewWebPart(listView);
                      });
                  });
            });

            DeployModel(model);
        }

       
        [TestMethod]
        [TestCategory("Docs.ListViewWebPartDefinition")]

        [SampleMetadata(Title = "Add LVWP binded to calendar view",
                Description = ""
                )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanBindListViewWebPartToCalendarView()
        {
            var companyEvents = new ListDefinition
            {
                Title = "Company Events",
                Description = "A document library.",
                TemplateType = BuiltInListTemplateTypeId.Events,
                Url = "m2CompanyEvents"
            };

            var webPartPage = new WebPartPageDefinition
            {
                Title = "M2 List View provision",
                FileName = "listview-webpart-provision.aspx",
                PageLayoutTemplate = BuiltInWebPartPageTemplates.spstd1
            };

            var listView = new ListViewWebPartDefinition
            {
                Title = "Company Events by List View Title",
                Id = "m2CompanyEvents",
                ZoneIndex = 10,
                ZoneId = "Main",
                ListUrl = companyEvents.GetListUrl(),
                ViewName = "Calendar"
            };

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web
                  .AddList(companyEvents)
                  .AddHostList(BuiltInListDefinitions.SitePages, list =>
                  {
                      list.AddWebPartPage(webPartPage, page =>
                      {
                          page.AddListViewWebPart(listView);
                      });
                  });
            });

            DeployModel(model);
        }

        #endregion
    }
}