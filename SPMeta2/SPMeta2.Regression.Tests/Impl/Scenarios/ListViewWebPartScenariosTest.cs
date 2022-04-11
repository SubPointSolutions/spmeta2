using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Containers.Extensions;
using SPMeta2.Containers.Standard;

using SPMeta2.Definitions;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Definitions.Extended;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Enumerations;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Modern;
using SPMeta2.Definitions.Base;

using SPMeta2.Regression.Tests.Extensions;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class ListViewWebPartScenariosTest : ListViewWebPartScenariosTestBase
    {
        #region internal

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            InternalInit();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            InternalCleanup();
        }

        #endregion

        #region list binding tests

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.ListViewWebPart")]
        public void CanDeploy_ListViewWebPart_ByListTitle()
        {
            var sourceList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def => { });
            var listViewWebpart = ModelGeneratorService.GetRandomDefinition<ListViewWebPartDefinition>(def =>
            {
                def.ListId = Guid.Empty;
                def.ListTitle = BuiltInDefinitions.BuiltInListDefinitions.StyleLibrary.Title;
                def.ListUrl = string.Empty;

                def.ViewName = string.Empty;
                def.ViewId = null;
            });


            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddList(sourceList)
                        .AddHostList(BuiltInListDefinitions.SitePages, list =>
                        {
                            list
                                .AddRandomWebPartPage(page =>
                                {
                                    page.AddListViewWebPart(listViewWebpart);
                                });
                        });

                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.ListViewWebPart")]
        public void CanDeploy_ListViewWebPart_ByListUrl()
        {
            var sourceList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def => { });
            var listViewWebpart = ModelGeneratorService.GetRandomDefinition<ListViewWebPartDefinition>(def =>
            {
                def.ListId = Guid.Empty;
                def.ListTitle = string.Empty;
#pragma warning disable 618
                def.ListUrl = BuiltInListDefinitions.StyleLibrary.GetListUrl();
#pragma warning restore 618

                def.ViewName = string.Empty;
                def.ViewId = null;
            });


            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddList(sourceList)
                        .AddHostList(BuiltInListDefinitions.SitePages, list =>
                        {
                            list
                                .AddRandomWebPartPage(page =>
                                {
                                    page.AddListViewWebPart(listViewWebpart);
                                });
                        });

                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.ListViewWebPart")]
        public void CanDeploy_ListViewWebPart_ByListId()
        {
            WithDisabledDefinitionImmutabilityValidation(() =>
            {
                var sourceList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def => { });
                var listViewWebpart = ModelGeneratorService.GetRandomDefinition<ListViewWebPartDefinition>(def =>
                {
                    // it will be updated later, just passing model validation
                    def.ListId = Guid.NewGuid();
                    def.ListTitle = string.Empty;
                    def.ListUrl = string.Empty;

                    def.ViewName = string.Empty;
                    def.ViewId = null;
                });

                var model = SPMeta2Model
                    .NewWebModel(web =>
                    {
                        web
                            .AddList(sourceList, list =>
                            {
                                list.OnProvisioned<object>(context =>
                                {
                                    listViewWebpart.ListId = ExtractListId(context);
                                });
                            })
                            .AddHostList(BuiltInListDefinitions.SitePages, list =>
                            {
                                list
                                    .AddRandomWebPartPage(page =>
                                    {
                                        page.AddListViewWebPart(listViewWebpart);
                                    });
                            });

                    });

                TestModel(model);
            });
        }

        #endregion

        #region list view binding tests

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.ListViewWebPart")]
        public void CanDeploy_ListViewWebPart_ByViewId()
        {
            WithDisabledDefinitionImmutabilityValidation(() =>
            {

                var sourceList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def => { });
                var sourceView = ModelGeneratorService.GetRandomDefinition<ListViewDefinition>(def =>
                {
                    def.Fields = new System.Collections.ObjectModel.Collection<string>
                {
                    BuiltInInternalFieldNames.ID,
                    BuiltInInternalFieldNames.Edit,
                    BuiltInInternalFieldNames.Title                    
                };

                    def.IsDefault = false;
                });

                var listViewWebpart = ModelGeneratorService.GetRandomDefinition<ListViewWebPartDefinition>(def =>
                {
                    def.ListId = Guid.Empty;
                    def.ListTitle = string.Empty;
#pragma warning disable 618
                    def.ListUrl = sourceList.GetListUrl();
#pragma warning restore 618

                    def.ViewName = string.Empty;
                    def.ViewId = null;
                });

                var model = SPMeta2Model
                    .NewWebModel(web =>
                    {
                        web
                            .AddList(sourceList, list =>
                            {
                                list.AddListView(sourceView, view =>
                                {
                                    view.OnProvisioned<object>(context =>
                                    {
                                        listViewWebpart.ViewId = ExtractViewId(context);
                                    });
                                });
                            })
                            .AddHostList(BuiltInListDefinitions.SitePages, list =>
                            {
                                list
                                    .AddRandomWebPartPage(page =>
                                    {
                                        page.AddListViewWebPart(listViewWebpart);
                                    });
                            });

                    });

                TestModel(model);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.ListViewWebPart")]
        public void CanDeploy_ListViewWebPart_ByViewName()
        {
            var sourceList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def => { });
            var sourceView = ModelGeneratorService.GetRandomDefinition<ListViewDefinition>(def =>
            {
                def.Fields = new System.Collections.ObjectModel.Collection<string>
                {
                    BuiltInInternalFieldNames.ID,
                    BuiltInInternalFieldNames.Edit,
                    BuiltInInternalFieldNames.Title                    
                };

                def.IsDefault = false;
            });

            var listViewWebpart = ModelGeneratorService.GetRandomDefinition<ListViewWebPartDefinition>(def =>
            {
                def.ListId = Guid.Empty;
                def.ListTitle = string.Empty;
#pragma warning disable 618
                def.ListUrl = sourceList.GetListUrl();
#pragma warning restore 618

                def.ViewName = sourceView.Title;
                def.ViewId = null;
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddList(sourceList, list =>
                        {
                            list.AddListView(sourceView);
                        })
                        .AddHostList(BuiltInListDefinitions.SitePages, list =>
                        {
                            list
                                .AddRandomWebPartPage(page =>
                                {
                                    page.AddListViewWebPart(listViewWebpart);
                                });
                        });

                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.ListViewWebPart")]
        public void CanDeploy_ListViewWebPart_ByViewUrl()
        {
            var sourceList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def => { });
            var sourceView = ModelGeneratorService.GetRandomDefinition<ListViewDefinition>(def =>
            {
                def.Fields = new System.Collections.ObjectModel.Collection<string>
                {
                    BuiltInInternalFieldNames.ID,
                    BuiltInInternalFieldNames.Edit,
                    BuiltInInternalFieldNames.Title                    
                };

                def.Url = string.Format("{0}.aspx", Rnd.String());
                def.IsDefault = false;
            });

            var listViewWebpart = ModelGeneratorService.GetRandomDefinition<ListViewWebPartDefinition>(def =>
            {
                def.ListId = Guid.Empty;
                def.ListTitle = string.Empty;
#pragma warning disable 618
                def.ListUrl = sourceList.GetListUrl();
#pragma warning restore 618

                def.ViewName = string.Empty;
                def.ViewId = null;
                def.ViewUrl = sourceView.Url;
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddList(sourceList, list =>
                        {
                            list.AddListView(sourceView);
                        })
                        .AddHostList(BuiltInListDefinitions.SitePages, list =>
                        {
                            list
                                .AddRandomWebPartPage(page =>
                                {
                                    page.AddListViewWebPart(listViewWebpart);
                                });
                        });

                });

            TestModel(model);
        }

        #endregion

        #region calendar provision issue

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.ListViewWebPart.Calendar")]
        public void CanDeploy_ListViewWebPart_As_CalendarView_ToWebPartPage()
        {
            // CSOM sometime fails to provision ListViewWebPart + CalendarView #570
            // https://github.com/SubPointSolutions/spmeta2/issues/570

            var sourceList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.Events;
            });

            var listViewWebpart = ModelGeneratorService.GetRandomDefinition<ListViewWebPartDefinition>(def =>
            {
                def.ListId = Guid.Empty;
                def.ListTitle = sourceList.Title;
                def.ListUrl = string.Empty;

                def.ViewName = string.Empty;
                def.ViewId = null;
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web
                    .AddList(sourceList)
                    .AddHostList(BuiltInListDefinitions.SitePages, list =>
                    {
                        list.AddRandomWebPartPage(page =>
                        {
                            page.AddListViewWebPart(listViewWebpart);
                        });
                    });
            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.ListViewWebPart.Calendar")]
        public void CanDeploy_ListViewWebPart_As_CalendarView_ToPublishingPage()
        {
            // CSOM sometime fails to provision ListViewWebPart + CalendarView #570
            // https://github.com/SubPointSolutions/spmeta2/issues/570

            var sourceList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.Events;
            });

            var listViewWebpart = ModelGeneratorService.GetRandomDefinition<ListViewWebPartDefinition>(def =>
            {
                def.ListId = Guid.Empty;
                def.ListTitle = sourceList.Title;
                def.ListUrl = string.Empty;

                def.ViewName = string.Empty;
                def.ViewId = null;
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web
                    .AddList(sourceList)
                    .AddHostList(BuiltInListDefinitions.Pages, list =>
                    {
                        list.AddRandomPublishingPage(page =>
                        {
                            (page.Value as PublishingPageDefinition).PageLayoutFileName = BuiltInPublishingPageLayoutNames.BlankWebPartPage;

                            page.AddListViewWebPart(listViewWebpart);
                        });
                    });
            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.ListViewWebPart.Calendar")]
        public void CanDeploy_ListViewWebPart_As_CalendarView_ToWikiPage()
        {
            // CSOM sometime fails to provision ListViewWebPart + CalendarView #570
            // https://github.com/SubPointSolutions/spmeta2/issues/570

            var sourceList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.Events;
            });

            var listViewWebpart = ModelGeneratorService.GetRandomDefinition<ListViewWebPartDefinition>(def =>
            {
                def.ListId = Guid.Empty;
                def.ListTitle = sourceList.Title;
                def.ListUrl = string.Empty;

                def.ViewName = string.Empty;
                def.ViewId = null;
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web
                    .AddList(sourceList)
                    .AddHostList(BuiltInListDefinitions.SitePages, list =>
                    {
                        list.AddRandomWikiPage(page =>
                        {
                            page.AddListViewWebPart(listViewWebpart);
                        });
                    });
            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.ListViewWebPart.Calendar")]
        public void CanDeploy_XsltListViewWebPart_As_GridView()
        {
            // CSOM issue to get GridView on the XsltLIstViewWebPart done #725
            // https://github.com/SubPointSolutions/spmeta2/issues/725

            var sourceList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });

            var sourceListView = ModelGeneratorService.GetRandomDefinition<ListViewDefinition>(def =>
            {
                def.Title = Rnd.String();
                def.Type = BuiltInViewType.Grid;

                def.IsDefault = false;

                def.TabularView = null;

                def.Fields = new Collection<string>
                    {
                        BuiltInInternalFieldNames.ID,
                        BuiltInInternalFieldNames.Title
                    };
            });

            var listViewWebpart = ModelGeneratorService.GetRandomDefinition<XsltListViewWebPartDefinition>(def =>
            {
                def.ListId = Guid.Empty;
                def.ListTitle = sourceList.Title;
                def.ListUrl = string.Empty;

                def.ViewName = sourceListView.Title;
                def.ViewId = null;
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web
                    .AddList(sourceList, list =>
                    {
                        list.AddListView(sourceListView);
                    })
                    .AddHostList(BuiltInListDefinitions.SitePages, list =>
                    {
                        var pageName = string.Empty;

                        list.AddRandomWebPartPage(page =>
                        {
                            pageName = (page.Value as WebPartPageDefinition).FileName;

                            page.AddXsltListViewWebPart(listViewWebpart);

                        });

                        list.AddDefinitionNode(new XsltListViewWebPartGridModePresenceDefinition
                        {
                            PageFileName = pageName,
                            WebPartDefinitions = new List<WebPartDefinitionBase>(new[] { listViewWebpart })
                        }, def =>
                        {
                            def.RegExcludeFromEventsValidation();
                        });
                    });
            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.ListViewWebPart.Calendar")]
        public void CanDeploy_ListViewWebPart_As_Calender_With_DateRangesOverlap()
        {
            // ListViewWebPartDefinition doesn't show calendar view #988
            // https://github.com/SubPointSolutions/spmeta2/issues/988

            var contentTypeDef = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = BuiltInContentTypeId.DocumentSet_Correct;
            });

            var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                //def.TemplateName = BuiltInListTemplates.DocumentLibrary.InternalName;
                //def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
                
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
                def.ContentTypesEnabled = true;
            });

            var listViewDef = ModelGeneratorService.GetRandomDefinition<ListViewDefinition>(def =>
            {
                def.IsDefault = true;
                //def.MobileDefaultView = true;

                def.RowLimit = 2147483647;
                def.Query = @"<Where>
                                <DateRangesOverlap>
                                    <FieldRef Name=""StartDate"" />
                                    <FieldRef Name=""_EndDate"" />
                                    <FieldRef Name=""RecurrenceID"" />
                                    <Value Type=""DateTime"">
                                        <Month />
                                    </Value>
                                </DateRangesOverlap>
                            </Where>";

                def.ViewData = @"<FieldRef Name=""Title"" Type=""CalendarMonthTitle""/>
<FieldRef Name=""Title"" Type=""CalendarWeekTitle""/>
<FieldRef Name=""Location"" Type=""CalendarWeekLocation""/>
<FieldRef Name=""Title"" Type=""CalendarDayTitle""/>
<FieldRef Name=""Location"" Type=""CalendarDayLocation""/>";

                def.IsPaged = false;
                def.IsDefault = false;

                def.Fields = new System.Collections.ObjectModel.Collection<string> {
                    BuiltInFieldDefinitions.Title.InternalName,
                    BuiltInFieldDefinitions.StartDate.InternalName,
                    BuiltInFieldDefinitions._EndDate.InternalName, 
                    BuiltInFieldDefinitions.Title.InternalName,

                    BuiltInFieldDefinitions.Location.InternalName ,
                    BuiltInFieldDefinitions.fAllDayEvent.InternalName ,
                    BuiltInFieldDefinitions.fRecurrence.InternalName ,
                    BuiltInFieldDefinitions.EventType.InternalName 
                };

                def.Type = @"Html";
                def.Types = new Collection<string> { 
                    "Calendar", 
                    "Recurrence" 
                };
            });


            var webpartPageDef = ModelGeneratorService.GetRandomDefinition<WebPartPageDefinition>(def =>
            {

            });

            var calendarWebPart = ModelGeneratorService.GetRandomDefinition<ListViewWebPartDefinition>(def =>
            {
                def.ListId = null;
                def.ListUrl = null;

                def.ListTitle = listDef.Title;
                def.ViewName = listViewDef.Title;
                def.ChromeType = @"Default";

                //def.ZoneId = @"LeftColumn",
                //ZoneIndex = 0,
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddContentType(contentTypeDef, contentType =>
                {
                    contentType
                        .AddContentTypeFieldLink(SPMeta2.BuiltInDefinitions.BuiltInFieldDefinitions.Title)
                        .AddContentTypeFieldLink(SPMeta2.BuiltInDefinitions.BuiltInFieldDefinitions.StartDate)
                        .AddContentTypeFieldLink(SPMeta2.BuiltInDefinitions.BuiltInFieldDefinitions._EndDate);
                });
            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web
                    .AddList(listDef, list =>
                    {
                        list
                            .AddListFieldLink(BuiltInFieldDefinitions.Location)
                            .AddListFieldLink(BuiltInFieldDefinitions.fAllDayEvent)
                            .AddListFieldLink(BuiltInFieldDefinitions.fRecurrence)
                            .AddListFieldLink(BuiltInFieldDefinitions.EventType)

                            .AddContentTypeLink(contentTypeDef)
                            .AddListView(listViewDef);
                    })
                    .AddHostList(BuiltInListDefinitions.SitePages, list =>
                    {
                        list
                            .AddWebPartPage(webpartPageDef, page =>
                            {
                                page.AddWebPart(calendarWebPart);
                            });
                    });
            });

            TestModel(siteModel, webModel);
        }

        #endregion
    }
}
