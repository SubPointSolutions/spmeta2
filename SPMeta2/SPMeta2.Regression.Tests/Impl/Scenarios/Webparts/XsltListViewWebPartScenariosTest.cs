using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Containers.Extensions;
using SPMeta2.Containers.Services;
using SPMeta2.Containers.Standard;

using SPMeta2.Definitions;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Regression.Tests.Definitions;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Standard.Definitions;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Modern;
using SPMeta2.Exceptions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Extensions;
using System.Collections.Generic;
using SPMeta2.Definitions.Fields;
using SPMeta2.Regression.Definitions;
using SPMeta2.Services;
using SPMeta2.ModelHandlers;
using SPMeta2.Definitions.Base;
using SPMeta2.Models;
using SPMeta2.Regression.Definitions.Extended;

namespace SPMeta2.Regression.Tests.Impl.Scenarios.Webparts
{
    [TestClass]
    public class XsltListViewWebPartScenariosTest : ListViewWebPartScenariosTestBase
    {
        #region constructors



        #endregion

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

        #region dedicated tests for cross related props

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.XsltListViewWebPart.Props")]
        public void CanDeploy_XsltListViewWebPart_Prop_DisableSaveAsNewViewButton()
        {
            var wp1 = GetDefaultXsltListViewWebPartDefinition(def =>
            {
                def.Title = "DisableSaveAsNewViewButton - true";
                def.DisableSaveAsNewViewButton = true;
                def.ExportMode = "All";
            });

            var wp2 = GetDefaultXsltListViewWebPartDefinition(def =>
            {
                def.Title = "DisableSaveAsNewViewButton - false";
                def.DisableSaveAsNewViewButton = false;
                def.ExportMode = "All";
            });

            var model = GenerateTestPropertyModel(new[] { wp1, wp2 });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.XsltListViewWebPart.Props")]
        public void CanDeploy_XsltListViewWebPart_Prop_DisableViewSelectorMenu()
        {
            var wp1 = GetDefaultXsltListViewWebPartDefinition(def =>
            {
                def.Title = "DisableViewSelectorMenu - true";
                def.DisableViewSelectorMenu = true;
                def.ExportMode = "All";
            });

            var wp2 = GetDefaultXsltListViewWebPartDefinition(def =>
            {
                def.Title = "DisableViewSelectorMenu - false";
                def.DisableViewSelectorMenu = false;
                def.ExportMode = "All";
            });

            var model = GenerateTestPropertyModel(new[] { wp1, wp2 });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.XsltListViewWebPart.Props")]
        public void CanDeploy_XsltListViewWebPart_Prop_InplaceSearchEnabled()
        {
            var wp1 = GetDefaultXsltListViewWebPartDefinition(def =>
            {
                def.Title = "InplaceSearchEnabled - true";
                def.InplaceSearchEnabled = true;
                def.ExportMode = "All";
            });

            var wp2 = GetDefaultXsltListViewWebPartDefinition(def =>
            {
                def.Title = "InplaceSearchEnabled - false";
                def.InplaceSearchEnabled = false;
                def.ExportMode = "All";
            });

            var model = GenerateTestPropertyModel(new[] { wp1, wp2 });

            TestModel(model);
        }

        //[TestMethod]
        //[TestCategory("Regression.Scenarios.Webparts.XsltListViewWebPart.Props")]
        //public void CanDeploy_XsltListViewWebPart_Prop_DisableColumnFiltering()
        //{
        //    var wp1 = GetDefaultXsltListViewWebPartDefinition(def =>
        //    {
        //        def.Title = "DisableColumnFiltering - true";
        //        def.DisableColumnFiltering = true;
        //        def.ExportMode = "All";
        //    });

        //    var wp2 = GetDefaultXsltListViewWebPartDefinition(def =>
        //    {
        //        def.Title = "DisableColumnFiltering - false";
        //        def.DisableColumnFiltering = false;
        //        def.ExportMode = "All";
        //    });

        //    var model = GenerateTestPropertyModel(new[] { wp1, wp2 });

        //    TestModel(model);
        //}

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.XsltListViewWebPart.Props")]
        public void CanDeploy_XsltListViewWebPart_Prop_ShowTimelineIfAvailable()
        {
            var wp1 = GetDefaultXsltListViewWebPartDefinition(def =>
            {
                def.ShowTimelineIfAvailable = true;
            });

            var wp2 = GetDefaultXsltListViewWebPartDefinition(def =>
            {
                def.ShowTimelineIfAvailable = false;
            });

            var model = GenerateTestPropertyModel(new[] { wp1, wp2 });

            TestModel(model);
        }

        private XsltListViewWebPartDefinition GetDefaultXsltListViewWebPartDefinition(
            Action<XsltListViewWebPartDefinition> action)
        {
            var result = ModelGeneratorService.GetRandomDefinition<XsltListViewWebPartDefinition>(def =>
            {
                def.ListId = Guid.Empty;
                def.ListTitle = BuiltInDefinitions.BuiltInListDefinitions.StyleLibrary.Title;
                def.ListUrl = string.Empty;

                def.ViewName = string.Empty;
                def.ViewId = null;

                action(def);
            });

            return result;
        }

        private ModelNode GenerateTestPropertyModel(IEnumerable<XsltListViewWebPartDefinition> webParts)
        {
            var sourceList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {

            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        //.AddList(sourceList)
                        .AddHostList(BuiltInListDefinitions.SitePages, list =>
                        {
                            list
                                .AddRandomWebPartPage(page =>
                                {
                                    page.AddXsltListViewWebParts(webParts);
                                });
                        });

                });

            return model;
        }

        #endregion

        #region list binding tests

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.XsltListViewWebPart")]
        public void CanDeploy_XsltListViewWebPart_ByListTitle()
        {
            var sourceList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def => { });
            var xsltListViewWebpart = ModelGeneratorService.GetRandomDefinition<XsltListViewWebPartDefinition>(def =>
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
                                    page.AddXsltListViewWebPart(xsltListViewWebpart);
                                });
                        });

                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.XsltListViewWebPart")]
        public void CanDeploy_XsltListViewWebPart_ByListUrl()
        {
            var sourceList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def => { });
            var xsltListViewWebpart = ModelGeneratorService.GetRandomDefinition<XsltListViewWebPartDefinition>(def =>
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
                                    page.AddXsltListViewWebPart(xsltListViewWebpart);
                                });
                        });

                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.XsltListViewWebPart")]
        public void CanDeploy_XsltListViewWebPart_ByListId()
        {
            WithDisabledDefinitionImmutabilityValidation(() =>
            {
                var sourceList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def => { });
                var xsltListViewWebpart =
                    ModelGeneratorService.GetRandomDefinition<XsltListViewWebPartDefinition>(def =>
                    {
                        // list id will be updated later in OnProvisioned
                        // we need to set something here 
                        // to pass property validation which requires one of the properties set
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
                                    xsltListViewWebpart.ListId = ExtractListId(context);
                                });
                            })
                            .AddHostList(BuiltInListDefinitions.SitePages, list =>
                            {
                                list
                                    .AddRandomWebPartPage(page =>
                                    {
                                        page.AddXsltListViewWebPart(xsltListViewWebpart);
                                    });
                            });

                    });

                TestModel(model);
            });
        }

        #endregion

        #region list view binding tests

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.XsltListViewWebPart")]
        public void CanDeploy_XsltListViewWebPart_ByViewId()
        {
            WithDisabledDefinitionImmutabilityValidation(() =>
            {
                WithDisabledPropertyUpdateValidation(() =>
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

                    var xsltListViewWebpart =
                        ModelGeneratorService.GetRandomDefinition<XsltListViewWebPartDefinition>(def =>
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
                                            xsltListViewWebpart.ViewId = ExtractViewId(context);
                                        });
                                    });
                                })
                                .AddHostList(BuiltInListDefinitions.SitePages, list =>
                                {
                                    list
                                        .AddRandomWebPartPage(page =>
                                        {
                                            page.AddXsltListViewWebPart(xsltListViewWebpart);
                                        });
                                });

                        });

                    TestModel(model);
                });
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.XsltListViewWebPart")]
        public void CanDeploy_XsltListViewWebPart_ByViewName()
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

            var xsltListViewWebpart = ModelGeneratorService.GetRandomDefinition<XsltListViewWebPartDefinition>(def =>
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
                                    page.AddXsltListViewWebPart(xsltListViewWebpart);
                                });
                        });

                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.XsltListViewWebPart")]
        public void CanDeploy_XsltListViewWebPart_ByViewUrl()
        {
            // Add 'ViewURL' property in 'XsltListViewWebPartDefinition' class #862
            // https://github.com/SubPointSolutions/spmeta2/issues/862

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

            var xsltListViewWebpart = ModelGeneratorService.GetRandomDefinition<XsltListViewWebPartDefinition>(def =>
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
                                    page.AddXsltListViewWebPart(xsltListViewWebpart);
                                });
                        });

                });

            TestModel(model);
        }

        #endregion

        #region xml - xslt

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.XsltListViewWebPart.XmlAndXslt")]
        public void CanDeploy_XsltListViewWebPart_WithXmlDefinition()
        {
            var xsltListViewWebpart = ModelGeneratorService.GetRandomDefinition<XsltListViewWebPartDefinition>(def =>
            {
                def.ListId = Guid.Empty;
                def.ListTitle = BuiltInListDefinitions.SitePages.Title;
                def.ListUrl = string.Empty;

                def.ViewName = string.Empty;
                def.ViewId = null;

                def.XmlDefinition = string.Format("<View BaseViewID=\"{0}\" />",
                        Rnd.RandomFromArray(new[] { 1, 2, 40 }));
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddHostList(BuiltInListDefinitions.SitePages, list =>
                        {
                            list
                                .AddRandomWebPartPage(page =>
                                {
                                    page.AddXsltListViewWebPart(xsltListViewWebpart);
                                });
                        });

                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.XsltListViewWebPart.XmlAndXslt")]
        public void CanDeploy_XsltListViewWebPart_WithXmlDefinitionLink()
        {
            var xsltListViewWebpart = ModelGeneratorService.GetRandomDefinition<XsltListViewWebPartDefinition>(def =>
            {
                def.ListId = Guid.Empty;
                def.ListTitle = BuiltInListDefinitions.SitePages.Title;
                def.ListUrl = string.Empty;

                def.ViewName = string.Empty;
                def.ViewId = null;

                def.XmlDefinitionLink = Rnd.HttpUrl();
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddHostList(BuiltInListDefinitions.SitePages, list =>
                        {
                            list
                                .AddRandomWebPartPage(page =>
                                {
                                    page.AddXsltListViewWebPart(xsltListViewWebpart);
                                });
                        });

                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.XsltListViewWebPart.XmlAndXslt")]
        public void CanDeploy_XsltListViewWebPart_WithXslLink()
        {
            var xslFileName = string.Format("m2.{0}.xsl", Rnd.String());

            var xsltListViewWebpart = ModelGeneratorService.GetRandomDefinition<XsltListViewWebPartDefinition>(def =>
            {
                def.ListId = Guid.Empty;
                def.ListTitle = BuiltInListDefinitions.SitePages.Title;
                def.ListUrl = string.Empty;

                def.ViewName = string.Empty;
                def.ViewId = null;

                // this needs to be set to get XslLink working
                // either SP1 issue or http context = null 
                def.BaseXsltHashKey = Guid.NewGuid().ToString();
                def.XslLink = "/Style Library/" + xslFileName;
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddHostList(BuiltInListDefinitions.StyleLibrary, list =>
                        {
                            list.AddModuleFile(new ModuleFileDefinition
                            {
                                Content = Encoding.UTF8.GetBytes(XsltStrings.XsltListViewWebPart_TestXsl),
                                FileName = xslFileName
                            });
                        })
                        .AddHostList(BuiltInListDefinitions.SitePages, list =>
                        {
                            list
                                .AddRandomWebPartPage(page =>
                                {
                                    page.AddXsltListViewWebPart(xsltListViewWebpart);
                                });
                        });

                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.XsltListViewWebPart.XmlAndXslt")]
        public void CanDeploy_XsltListViewWebPart_WithXsl()
        {
            var xsltListViewWebpart = ModelGeneratorService.GetRandomDefinition<XsltListViewWebPartDefinition>(def =>
            {
                def.ListId = Guid.Empty;
                def.ListTitle = BuiltInListDefinitions.SitePages.Title;
                def.ListUrl = string.Empty;

                def.ViewName = string.Empty;
                def.ViewId = null;

                // this needs to be set to get XslLink working
                // either SP1 issue or http context = null 
                def.BaseXsltHashKey = Guid.NewGuid().ToString();
                def.Xsl = XsltStrings.XsltListViewWebPart_TestXsl;
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddHostList(BuiltInListDefinitions.SitePages, list =>
                        {
                            list
                                .AddRandomWebPartPage(page =>
                                {
                                    page.AddXsltListViewWebPart(xsltListViewWebpart);
                                });
                        });

                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.XsltListViewWebPart.XmlAndXslt")]
        public void CanDeploy_XsltListViewWebPart_WithGhostedXslLink()
        {
            var xsltListViewWebpart = ModelGeneratorService.GetRandomDefinition<XsltListViewWebPartDefinition>(def =>
            {
                def.ListId = Guid.Empty;
                def.ListTitle = BuiltInListDefinitions.SitePages.Title;
                def.ListUrl = string.Empty;

                def.ViewName = string.Empty;
                def.ViewId = null;

                def.GhostedXslLink = "blog.xsl";
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddHostList(BuiltInListDefinitions.SitePages, list =>
                        {
                            list
                                .AddRandomWebPartPage(page =>
                                {
                                    page.AddXsltListViewWebPart(xsltListViewWebpart);
                                });
                        });

                });

            TestModel(model);
        }

        #endregion

        #region wiki page cases

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.XsltListViewWebPart.WikiPage")]
        public void CanDeploy_XsltListViewWebPart_On_WikiPage()
        {
            WithDisabledPropertyUpdateValidation(() =>
            {
                var library = ModelGeneratorService.GetRandomDefinition<ListDefinition>();
                var wikiPage = ModelGeneratorService.GetRandomDefinition<WikiPageDefinition>(def =>
                {
                    def.NeedOverride = true;
                });

                var ceWebPart = ModelGeneratorService.GetRandomDefinition<ContentEditorWebPartDefinition>(def =>
                {
                    def.ChromeType = BuiltInPartChromeType.TitleOnly;

                    def.ZoneId = "wpz";
                    def.ZoneIndex = 1;
                });

                var xsltWebPart = ModelGeneratorService.GetRandomDefinition<XsltListViewWebPartDefinition>(def =>
                {
                    def.ChromeType = BuiltInPartChromeType.TitleOnly;

                    def.ZoneId = "wpz";
                    def.ZoneIndex = 1;

                    def.ListTitle = library.Title;
                    def.AddToPageContent = true;
                });


                var model = SPMeta2Model.NewWebModel(web =>
                {
                    web
                        .AddList(library)
                        .AddHostList(BuiltInListDefinitions.SitePages, list =>
                        {
                            var id_1 = "g_" + Guid.NewGuid().ToString("D").Replace('-', '_');
                            var id_2 = "g_" + Guid.NewGuid().ToString("D").Replace('-', '_');

                            ceWebPart.Id = id_1;
                            xsltWebPart.Id = id_2;

                            list.AddWikiPage(wikiPage, page =>
                            {
                                // content is changed for CSOM
                                // validation won't pass it, so turn off
                                page.RegExcludeFromValidation();

                                var wpId11 = id_1.Replace("g_", string.Empty).Replace("_", "-");
                                var wpId22 = id_2.Replace("g_", string.Empty).Replace("_", "-");

                                var pageTemplate = new StringBuilder();

                                pageTemplate.AppendFormat("​​​​​​​​​​​​​​​​​​​​​​<div class='ms-rtestate-read ms-rte-wpbox' contentEditable='false'>");
                                pageTemplate.AppendFormat("     <div class='ms-rtestate-read {0}' id='div_{0}'>", wpId11);
                                pageTemplate.AppendFormat("     </div>");
                                pageTemplate.AppendFormat("</div>");

                                pageTemplate.AppendFormat("<div>");
                                pageTemplate.AppendFormat(" SPMeta2 wiki page.");
                                pageTemplate.AppendFormat("</div>");

                                pageTemplate.AppendFormat("​​​​​​​​​​​​​​​​​​​​​​<div class='ms-rtestate-read ms-rte-wpbox' contentEditable='false'>");
                                pageTemplate.AppendFormat("     <div class='ms-rtestate-read {0}' id='div_{0}'>", wpId22);
                                pageTemplate.AppendFormat("     </div>");
                                pageTemplate.AppendFormat("</div>");

                                (page.Value as WikiPageDefinition).Content = pageTemplate.ToString();


                                page
                                    .AddWebPart(ceWebPart)
                                    .AddXsltListViewWebPart(xsltWebPart);
                            })
                            // we need to ensure that mentioned web part are on the target page, literally
                            .AddDefinitionNode(new WebpartPresenceOnPageDefinition
                            {
                                PageFileName = wikiPage.FileName,
                                WebPartDefinitions = new List<WebPartDefinitionBase>(new WebPartDefinition[]
                            {
                                    ceWebPart,
                                    xsltWebPart
                            })
                            }, def =>
                            {
                                def.RegExcludeFromEventsValidation();
                            });
                        });
                });

                TestModel(model);
            });
        }

        #endregion

        #region special cases

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.XsltListViewWebPart")]
        public void CanDeploy_XsltListViewWebPart_With_Thumbnails_View()
        {
            var sourceList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = 0;
                def.TemplateName = BuiltInListTemplates.AssetLibrary.InternalName;
            });

            var xsltListViewWebpart = ModelGeneratorService.GetRandomDefinition<XsltListViewWebPartDefinition>(def =>
            {
                def.ListId = Guid.Empty;
                def.ListTitle = string.Empty;
#pragma warning disable 618
                def.ListUrl = sourceList.GetListUrl();
#pragma warning restore 618

                def.ViewName = "Thumbnails";
                def.ViewId = null;

                def.JSLink = string.Empty;

                def.XmlDefinition = "<View BaseViewID='40'/>";
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web
                    .AddList(sourceList)
                    .AddHostList(BuiltInListDefinitions.SitePages, list =>
                    {
                        list
                            .AddRandomWebPartPage(page =>
                            {
                                page.AddXsltListViewWebPart(xsltListViewWebpart);
                            });
                    });
            });

            TestModel(model);
        }

        #endregion

        #region scopes

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.XsltListViewWebPart.Scopes")]
        public void CanDeploy_XsltListViewWebPart_At_RootWeb_FromRootWeb()
        {
            // Add regression tests for XsltListViewWebPart provision on sub-webs #810
            // https://github.com/SubPointSolutions/spmeta2/issues/810

            // list     : root web 
            // web part : root web

            var sourceList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def => { });

            var xsltListViewWebpart = ModelGeneratorService.GetRandomDefinition<XsltListViewWebPartDefinition>(def =>
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
                        list
                            .AddRandomWebPartPage(page =>
                            {
                                page.AddXsltListViewWebPart(xsltListViewWebpart);
                            });
                    });

            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.XsltListViewWebPart.Scopes")]
        public void CanDeploy_XsltListViewWebPart_At_RootWeb_FromSubWeb()
        {
            // Add regression tests for XsltListViewWebPart provision on sub-webs #810
            // https://github.com/SubPointSolutions/spmeta2/issues/810

            // list     : sub web 
            // web part : root web

            var sourceList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {

            });

            var sourceListWeb = ModelGeneratorService.GetRandomDefinition<WebDefinition>(def =>
            {

            });

            var xsltListViewWebpart = ModelGeneratorService.GetRandomDefinition<XsltListViewWebPartDefinition>(def =>
            {
                def.ListId = Guid.Empty;
                def.ListTitle = sourceList.Title;
                def.WebUrl = "~sitecollection/" + sourceListWeb.Url;

                def.ViewName = string.Empty;
                def.ViewId = null;
            });

            var listModel = SPMeta2Model.NewWebModel(web =>
            {
                web
                  .AddWeb(sourceListWeb, subWeb =>
                  {
                      subWeb.AddList(sourceList);
                  });
            });

            var webPartModel = SPMeta2Model.NewWebModel(web =>
            {
                web
                  .AddHostList(BuiltInListDefinitions.SitePages, list =>
                  {
                      list
                          .AddRandomWebPartPage(page =>
                          {
                              page.AddXsltListViewWebPart(xsltListViewWebpart);
                          });
                  });
            });

            TestModel(listModel, webPartModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.XsltListViewWebPart.Scopes")]
        public void CanDeploy_XsltListViewWebPart_At_SubWeb_FromSubWeb()
        {
            // Add regression tests for XsltListViewWebPart provision on sub-webs #810
            // https://github.com/SubPointSolutions/spmeta2/issues/810

            // list     : sub web 
            // web part : sub web

            var sourceList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {

            });

            var sourceListWeb = ModelGeneratorService.GetRandomDefinition<WebDefinition>(def =>
            {

            });

            var targetWebPartWeb = ModelGeneratorService.GetRandomDefinition<WebDefinition>(def =>
            {

            });

            var xsltListViewWebpart = ModelGeneratorService.GetRandomDefinition<XsltListViewWebPartDefinition>(def =>
            {
                def.ListId = Guid.Empty;
                def.ListTitle = sourceList.Title;
                def.WebUrl = "~sitecollection/" + sourceListWeb.Url;

                def.ViewName = string.Empty;
                def.ViewId = null;
            });


            var listModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWeb(sourceListWeb, subWeb =>
                {
                    subWeb.AddList(sourceList);
                });
            });

            var webPartModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWeb(targetWebPartWeb, targetWeb =>
                {
                    targetWeb.AddHostList(BuiltInListDefinitions.SitePages, list =>
                    {
                        list
                            .AddRandomWebPartPage(page =>
                            {
                                page.AddXsltListViewWebPart(xsltListViewWebpart);
                            });
                    });
                });
            });

            TestModel(listModel, webPartModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.XsltListViewWebPart.Scopes")]
        public void CanDeploy_XsltListViewWebPart_At_SubWeb_FromRootWeb()
        {
            // Add regression tests for XsltListViewWebPart provision on sub-webs #810
            // https://github.com/SubPointSolutions/spmeta2/issues/810

            // list     : sub web 
            // web part : sub web

            var sourceList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {

            });

            var targetWebPartWeb = ModelGeneratorService.GetRandomDefinition<WebDefinition>(def =>
            {

            });

            var xsltListViewWebpart = ModelGeneratorService.GetRandomDefinition<XsltListViewWebPartDefinition>(def =>
            {
                def.ListId = Guid.Empty;
                def.ListTitle = sourceList.Title;
                def.WebUrl = "~sitecollection";

                def.ViewName = string.Empty;
                def.ViewId = null;
            });


            var listModel = SPMeta2Model.NewWebModel(web =>
            {
                web
                  .AddList(sourceList);
            });

            var webPartModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWeb(targetWebPartWeb, targetWeb =>
                {
                    targetWeb.AddHostList(BuiltInListDefinitions.SitePages, list =>
                    {
                        list
                            .AddRandomWebPartPage(page =>
                            {
                                page.AddXsltListViewWebPart(xsltListViewWebpart);
                            });
                    });
                });
            });

            TestModel(listModel, webPartModel);
        }

        #endregion

        #region parameter bindings

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.XsltListViewWebPart.ParameterBindings")]
        public void CanDeploy_XsltListViewWebPart_With_ParameterBindings_ID_Filtering()
        {
            var dstList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });

            var sourceList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });

            var sourceListView = ModelGeneratorService.GetRandomDefinition<ListViewDefinition>(def =>
            {
                def.Query = "<Eq><FieldRef Name=\"ID\" /><Value Type=\"Counter\">{ID}</Value></Eq>";
                def.Hidden = true;
            });

            // this web part would be binded to list view
            // it will also be performing 'filtering' via query string - ID
            var xsltListViewWebpart = ModelGeneratorService.GetRandomDefinition<XsltListViewWebPartDefinition>(def =>
            {
                def.ListId = Guid.Empty;
                def.ListTitle = sourceList.Title;
                def.ListUrl = string.Empty;

                def.ViewName = sourceListView.Title;
                def.ViewId = null;

                def.JSLink = string.Empty;

                def.ParameterBindings.Add(new ParameterBindingValue
                {
                    Name = "ID",
                    Location = "QueryString(ID)"
                });
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web
                    .AddList(sourceList, list =>
                    {
                        list.AddListView(sourceListView);

                        list
                            .AddRandomListItem()
                            .AddRandomListItem()
                            .AddRandomListItem();
                    })
                    .AddList(dstList, list =>
                    {
                        list
                            .AddRandomListItem()
                            .AddRandomListItem();

                        list.AddHostListView(BuiltInListViewDefinitions.Lists.EditForm, view =>
                        {
                            view.AddXsltListViewWebPart(xsltListViewWebpart);
                        });
                    });
            });

            TestModel(model);
        }

        #endregion
    }
}
