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
using SPMeta2.CSOM.DefaultSyntax;
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
                    def.ListId = Guid.Empty;
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

        #endregion
    }
}
