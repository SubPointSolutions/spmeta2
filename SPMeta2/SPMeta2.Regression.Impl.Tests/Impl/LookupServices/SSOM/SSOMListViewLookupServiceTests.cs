using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.Services;
using SPMeta2.Services;
using System.Reflection;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.CSOM;
using SPMeta2.Containers.Services;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Containers.SSOM;
using SPMeta2.Regression.Impl.Tests.Impl.Services.Base;
using SPMeta2.SSOM.Services;
using SPMeta2.Containers.Standard.DefinitionGenerators;
using SPMeta2.Syntax.Default;
using SPMeta2.Containers;
using SPMeta2.Definitions.Webparts;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Definitions;
using SPMeta2.Containers.Services.Rnd;
using SPMeta2.Enumerations;
using SPMeta2.Definitions.ContentTypes;

namespace SPMeta2.Regression.Impl.Tests.Impl.Services
{

    [TestClass]
    public class SSOMListViewLookupServiceTests
    {
        public SSOMListViewLookupServiceTests()
        {
            Service = new SSOMListViewLookupService();
            ProvisionRunner = new SSOMProvisionRunner();
            Rnd = new DefaultRandomService();

            ModelGeneratorService = new ModelGeneratorService();
            ModelGeneratorService.RegisterDefinitionGenerators(typeof(ImageRenditionDefinitionGenerator).Assembly);

            ProvisionRunner.EnableDefinitionProvision = true;
            ProvisionRunner.EnableDefinitionValidation = false;
        }

        #region init

        [ClassInitialize]
        public static void Init(TestContext context)
        {

        }

        [ClassCleanup]
        public static void Cleanup()
        {

        }

        #endregion

        #region properties

        public SSOMListViewLookupService Service { get; set; }

        public ModelGeneratorService ModelGeneratorService { get; set; }
        public SSOMProvisionRunner ProvisionRunner { get; set; }

        public DefaultRandomService Rnd { get; set; }

        #endregion

        #region tests

        #region classes

        public static class Lists
        {
            public static ListDefinition TestList1 = new ListDefinition
            {
                Title = "Тестовый спискок 1",
                CustomUrl = "Lists/TestList1",
                Description = "",
                TemplateType = BuiltInListTemplateTypeId.GenericList,
                ContentTypesEnabled = true,
                EnableFolderCreation = false,
                EnableAttachments = false,
                OnQuickLaunch = true
            };

            public static ListDefinition TestList2 = new ListDefinition
            {
                Title = "Тестовый спискок 2",
                CustomUrl = "Lists/TestList2",
                Description = "",
                TemplateType = BuiltInListTemplateTypeId.GenericList,
                ContentTypesEnabled = true,
                EnableFolderCreation = false,
                EnableAttachments = false,
                OnQuickLaunch = true
            };

            public static ListDefinition TestList3 = new ListDefinition
            {
                Title = "Тестовый спискок 3",
                CustomUrl = "Lists/TestList3",
                Description = "",
                TemplateType = BuiltInListTemplateTypeId.GenericList,
                ContentTypesEnabled = true,
                EnableFolderCreation = false,
                EnableAttachments = false,
                OnQuickLaunch = true
            };
        }

        public static class Views
        {
            public static ListViewDefinition View1 = new ListViewDefinition
            {
                Title = "Все элементы1",
                Url = "AllItems1.aspx",
                IsDefault = true,
                Fields = new System.Collections.ObjectModel.Collection<string>
            {
                BuiltInInternalFieldNames.LinkTitle,
            }
            };

            public static ListViewDefinition View2 = new ListViewDefinition
            {
                Title = "Все элементы2",
                Url = "AllItems2.aspx",
                IsDefault = true,
                Fields = new System.Collections.ObjectModel.Collection<string>
            {
                BuiltInInternalFieldNames.LinkTitle,
            }
            };

            public static ListViewDefinition View3 = new ListViewDefinition
            {
                Title = "Все элементы3",
                Url = "AllItems3.aspx",
                IsDefault = true,
                Fields = new System.Collections.ObjectModel.Collection<string>
            {
                BuiltInInternalFieldNames.LinkTitle,
            }
            };
        }

        public static class WebParts
        {
            public static XsltListViewWebPartDefinition TestWebPart1 = new XsltListViewWebPartDefinition
            {
                Id = "lvTestWebPart1",
                Title = "TestWebPart1",
                ListUrl = Lists.TestList1.CustomUrl,
                ViewUrl = Views.View1.Url,
                //Toolbar = BuiltInToolbarType.Freeform,
                //ChromeType = BuiltInPartChromeType.TitleOnly,
                ZoneId = "Main",
                ZoneIndex = 10,

            };

            public static XsltListViewWebPartDefinition TestWebPart2 = new XsltListViewWebPartDefinition
            {
                Id = "lvTestWebPart2",
                Title = "TestWebPart2",
                ListUrl = Lists.TestList2.CustomUrl,
                ViewUrl = Views.View2.Url,
                //Toolbar = BuiltInToolbarType.Freeform,
                //ChromeType = BuiltInPartChromeType.TitleOnly,
                ZoneId = "Main",
                ZoneIndex = 11,

            };

            public static XsltListViewWebPartDefinition TestWebPart3 = new XsltListViewWebPartDefinition
            {
                Id = "lvTestWebPart3",
                Title = "TestWebPart3",
                ListUrl = Lists.TestList3.CustomUrl,
                ViewUrl = Views.View3.Url,
                //Toolbar = BuiltInToolbarType.Freeform,
                //ChromeType = BuiltInPartChromeType.TitleOnly,
                ZoneId = "Main",
                ZoneIndex = 12,

            };

        }

        public class ContentTypes
        {
            public static ContentTypeDefinition MyType1 = new ContentTypeDefinition
            {
                Id = new Guid("{65855224-CB09-46B2-845B-F64F276BF21E}"),
                Name = "Мой тип контента 1",
                ParentContentTypeId = BuiltInContentTypeId.Item,
                Group = "Test"
            };

            public static ContentTypeDefinition MyType2 = new ContentTypeDefinition
            {
                Id = new Guid("{A61F2AC2-30EB-455B-B0CE-6E98A058696C}"),
                Name = "Мой тип контента 2",
                ParentContentTypeId = BuiltInContentTypeId.Item,
                Group = "Test"
            };

            public static ContentTypeDefinition MyType3 = new ContentTypeDefinition
            {
                Id = new Guid("{94111874-4B72-4BBD-BE52-6DF5690FFAEB}"),
                Name = "Мой тип контента 3",
                ParentContentTypeId = BuiltInContentTypeId.Item,
                Group = "Test"
            };
        }

        #endregion

        [TestMethod]
        [TestCategory("Regression.Impl.SSOMListViewLookupService")]
        public void Can_Find_ListView_WithInList()
        {
            // Problem with webpart deployment to display form #891
            // https://github.com/SubPointSolutions/spmeta2/issues/891

            var hasHit = false;
            var listViewDef = ModelGeneratorService.GetRandomDefinition<ListViewDefinition>();

            // once model is deployed with the XsltLIstViewWebPart pointing to StyleLibrary
            // we expect to have the following views on the collection:

            //  targetList.RootFolder.Url   "Lists/TestList3"   string
            //  targetList.Views[0].ServerRelativeUrl   "/Lists/TestList3/AllItems.aspx"    string
            //  targetList.Views[1].ServerRelativeUrl   "/Lists/TestList1/DispForm.aspx"    string
            //  targetList.Views[2].ServerRelativeUrl   "/Lists/TestList3/AllItems3.aspx"   string

            // so that the issue is that DispForm.aspx does not belong to the TestList3!
            // listv view lookup service must not find that view!

            var webDef = ModelGeneratorService.GetRandomDefinition<WebDefinition>();

            var contentTypesModel = SPMeta2Model
                          .NewSiteModel(site =>
                          {
                              site
                                    .AddContentType(ContentTypes.MyType1)
                                    .AddContentType(ContentTypes.MyType2)
                                    .AddContentType(ContentTypes.MyType3);
                          });

            var listsModel = SPMeta2Model.NewWebModel(rootWeb =>
            {
                rootWeb.AddWeb(webDef, web =>
                {
                    web
                                   .AddList(Lists.TestList1)
                                   .AddList(Lists.TestList2)
                                   .AddList(Lists.TestList3);
                });

            });

            var webModel = SPMeta2Model.NewWebModel(rootWeb =>
            {
                rootWeb.AddWebFeature(BuiltInWebFeatures.WikiPageHomePage.Inherit());

                rootWeb.AddWeb(webDef, web =>
                {
                    web.AddWebFeature(BuiltInWebFeatures.WikiPageHomePage.Inherit());

                    web
                        .AddList(Lists.TestList1, list =>
                        {
                            list
                                .AddHideContentTypeLinks(new HideContentTypeLinksDefinition
                                {
                                    ContentTypes = { new ContentTypeLinkValue() { ContentTypeName = "Item" }, new ContentTypeLinkValue() { ContentTypeName = "Элемент" } }
                                })
                                .AddContentTypeLink(ContentTypes.MyType1)
                                .AddListView(Views.View1)
                                .AddHostListView(BuiltInListViewDefinitions.Lists.DispForm, view =>
                                {
                                    view.AddWebPart(WebParts.TestWebPart3);
                                });
                        })
                        .AddList(Lists.TestList2, list =>
                        {
                            list
                                .AddHideContentTypeLinks(new HideContentTypeLinksDefinition
                                {
                                    ContentTypes = { new ContentTypeLinkValue() { ContentTypeName = "Item" }, new ContentTypeLinkValue() { ContentTypeName = "Элемент" } }
                                })
                                .AddContentTypeLink(ContentTypes.MyType2)
                                .AddListView(Views.View2)
                                .AddHostListView(BuiltInListViewDefinitions.Lists.AllItems, view =>
                                {
                                    view.AddWebPart(WebParts.TestWebPart1);
                                });
                        })
                        .AddList(Lists.TestList3, list =>
                        {
                            list
                                .AddHideContentTypeLinks(new HideContentTypeLinksDefinition
                                {
                                    ContentTypes = { new ContentTypeLinkValue() { ContentTypeName = "Item" }, new ContentTypeLinkValue() { ContentTypeName = "Элемент" } }
                                })
                                .AddContentTypeLink(ContentTypes.MyType3)
                                .AddListView(Views.View3)
                                .AddHostListView(BuiltInListViewDefinitions.Lists.DispForm, view =>
                                {
                                    view.AddWebPart(WebParts.TestWebPart2);
                                });
                        });
                });
            });

            ProvisionRunner.DeploySiteModel(contentTypesModel);
            ProvisionRunner.DeployWebModel(listsModel);

            ProvisionRunner.DeployWebModel(webModel);
            ProvisionRunner.DeployWebModel(webModel);

            ProvisionRunner.WithSSOMSiteAndWebContext((site, rootWeb) =>
            {
                using (var web = site.OpenWeb(webDef.Url))
                {
                    hasHit = true;

                    //  targetList.RootFolder.Url   "Lists/TestList3"   string
                    //  targetList.Views[0].ServerRelativeUrl   "/Lists/TestList3/AllItems.aspx"    string
                    //  targetList.Views[1].ServerRelativeUrl   "/Lists/TestList1/DispForm.aspx"    string
                    //  targetList.Views[2].ServerRelativeUrl   "/Lists/TestList3/AllItems3.aspx"   string

                    var listTitle = Lists.TestList3.Title;

                    var currentList = web.Lists[listTitle];
                    var currentListUrl = currentList.RootFolder.Url;

                    var ownViews = currentList.Views.OfType<SPView>()
                                              .Where(v => v.Url.ToUpper().StartsWith(currentListUrl.ToUpper()));

                    var alienViews = currentList.Views.OfType<SPView>()
                                                .Where(v => !(v.Url.ToUpper().StartsWith(currentListUrl.ToUpper())));

                    // there must be a few correct views and the other one from the othe list (by URL)
                    Assert.IsTrue(ownViews.Count() > 0);
                    Assert.IsTrue(alienViews.Count() > 0);

                    // lookup service must not find the DispForm.aspx view 
                    // it does not belong to a list
                    var view = Service.FindView(currentList, BuiltInListViewDefinitions.Lists.DispForm);

                    Assert.IsNull(view);
                }
            });

            Assert.IsTrue(hasHit);
        }

        #endregion
    }
}
