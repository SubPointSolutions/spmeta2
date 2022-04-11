using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Containers.Extensions;
using SPMeta2.Containers.Services;
using SPMeta2.Containers.Standard;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Definitions;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;
using SPMeta2.Validation.Validators.Relationships;
using SPMeta2.Models;
using SPMeta2.Syntax.Extended;
using SPMeta2.Utils;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class WebpartScenariosTest : SPMeta2RegresionScenarioTestBase
    {
        #region internal

        [ClassInitializeAttribute]
        public static void Init(TestContext context)
        {
            InternalInit();
        }

        [ClassCleanupAttribute]
        public static void Cleanup()
        {
            InternalCleanup();
        }

        #endregion

        #region v2-v3 validation

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.Versions")]
        public void CanDeploy_Random_V2_Webpart()
        {
            TestRandomDefinition<WebPartDefinition>(def =>
            {
                def.WebpartFileName = string.Empty;
                def.WebpartType = string.Empty;
                def.WebpartXmlTemplate = BuiltInWebPartTemplates.ContentEditorWebPart;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.Versions")]
        public void CanDeploy_Random_V3_Webpart()
        {
            TestRandomDefinition<WebPartDefinition>(def =>
            {
                def.WebpartFileName = string.Empty;
                def.WebpartType = string.Empty;
                def.WebpartXmlTemplate = BuiltInWebPartTemplates.ScriptEditorWebPart;
            });
        }

        #endregion


        #region v2-v3 chrome

        private static List<string> PartChromeTypes = new List<string>
        {
            "Default",
            "TitleAndBorder",
            "None",
            "TitleOnly",
            "BorderOnly"
        };

        private static List<string> FrameTypes = new List<string>
        {
           "None",
		   "Standard",
		   "TitleBarOnly",
		   "Default",
		   "BorderOnly"
        };

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.ChromeType")]
        public void CanDeploy_Random_V2_Webpart_WithAllChrome()
        {
            var allValues = new List<string>();

            allValues.AddRange(PartChromeTypes);
            allValues.AddRange(FrameTypes);

            foreach (var chromeType in allValues)
            {
                var type = chromeType;

                TestRandomDefinition<WebPartDefinition>(def =>
                {
                    def.ChromeType = type;

                    def.WebpartFileName = string.Empty;
                    def.WebpartType = string.Empty;
                    def.WebpartXmlTemplate = BuiltInWebPartTemplates.ContentEditorWebPart;
                });
            }
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.ChromeType")]
        public void CanDeploy_Random_V3_Webpart_WithAllChrome()
        {
            var allValues = new List<string>();

            allValues.AddRange(PartChromeTypes);
            allValues.AddRange(FrameTypes);

            foreach (var chromeType in allValues)
            {
                var type = chromeType;

                TestRandomDefinition<WebPartDefinition>(def =>
                {
                    def.ChromeType = type;

                    def.WebpartFileName = string.Empty;
                    def.WebpartType = string.Empty;
                    def.WebpartXmlTemplate = BuiltInWebPartTemplates.ScriptEditorWebPart;
                });
            }
        }

        #endregion

        #region  list views

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.ListViews")]
        [TestCategory("Regression.Scenarios.Webparts.Hosts")]
        public void CanDeploy_WebpartTo_UploadForm_InLibrary()
        {
            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddRandomDocumentLibrary(list =>
                        {
                            list.AddHostListView(BuiltInListViewDefinitions.Libraries.Upload, listView =>
                            {
                                listView.AddRandomWebpart();
                                listView.AddRandomWebpart();
                                listView.AddRandomWebpart();
                            });
                        });
                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.ListViews")]
        [TestCategory("Regression.Scenarios.Webparts.Hosts")]
        public void CanDeploy_WebpartTo_OOTBListViews_InLibrary()
        {
            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddRandomDocumentLibrary(list =>
                        {
                            list.AddHostListView(BuiltInListViewDefinitions.Libraries.AllItems, listView =>
                            {
                                listView.AddRandomWebpart();
                                listView.AddRandomWebpart();
                                listView.AddRandomWebpart();
                            });

                            list.AddHostListView(BuiltInListViewDefinitions.Libraries.DispForm, listView =>
                            {
                                listView.AddRandomWebpart();
                                listView.AddRandomWebpart();
                                listView.AddRandomWebpart();
                            });

                            list.AddHostListView(BuiltInListViewDefinitions.Libraries.EditForm, listView =>
                            {
                                listView.AddRandomWebpart();
                                listView.AddRandomWebpart();
                                listView.AddRandomWebpart();
                            });
                        });
                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.Hosts")]
        [TestCategory("Regression.Scenarios.Webparts.ListViews")]
        public void CanDeploy_WebpartTo_CustomListViews_InLibrary()
        {
            var model = SPMeta2Model
               .NewWebModel(web =>
               {
                   web
                       .AddRandomDocumentLibrary(list =>
                       {
                           // custom with title only
                           list.AddRandomListView(listView =>
                           {
                               var def = listView.Value as ListViewDefinition;
                               def.Url = string.Empty;
                               def.IsDefault = false;

                               listView.AddRandomWebpart();
                               listView.AddRandomWebpart();
                           });

                           // custom with UTL
                           list.AddRandomListView(listView =>
                           {
                               var def = listView.Value as ListViewDefinition;
                               def.Url = Rnd.AspxFileName();
                               def.IsDefault = false;

                               listView.AddRandomWebpart();
                               listView.AddRandomWebpart();
                           });
                       });

               });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.ListViews")]
        [TestCategory("Regression.Scenarios.Webparts.Hosts")]
        public void CanDeploy_WebpartTo_OOTBListViews_InList()
        {
            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddRandomList(list =>
                        {
                            list.AddHostListView(BuiltInListViewDefinitions.Lists.AllItems, listView =>
                            {
                                listView.AddRandomWebpart();
                                listView.AddRandomWebpart();
                                listView.AddRandomWebpart();
                            });

                            list.AddHostListView(BuiltInListViewDefinitions.Lists.EditForm, listView =>
                            {
                                listView.AddRandomWebpart();
                                listView.AddRandomWebpart();
                                listView.AddRandomWebpart();

                            });

                            list.AddHostListView(BuiltInListViewDefinitions.Lists.NewForm, listView =>
                            {
                                listView.AddRandomWebpart();
                                listView.AddRandomWebpart();
                                listView.AddRandomWebpart();
                            });

                            list.AddHostListView(BuiltInListViewDefinitions.Lists.DispForm, listView =>
                            {
                                listView.AddRandomWebpart();
                                listView.AddRandomWebpart();
                                listView.AddRandomWebpart();
                            });
                        });

                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.ListViews")]
        [TestCategory("Regression.Scenarios.Webparts.Hosts")]
        public void CanDeploy_WebpartTo_CustomListViews_InList()
        {
            var model = SPMeta2Model
               .NewWebModel(web =>
               {
                   web
                       .AddRandomList(list =>
                       {
                           // custom with title only
                           list.AddRandomListView(listView =>
                           {
                               var def = listView.Value as ListViewDefinition;
                               def.Url = string.Empty;
                               def.IsDefault = false;

                               listView.AddRandomWebpart();
                               listView.AddRandomWebpart();
                           });

                           // custom with UTL
                           list.AddRandomListView(listView =>
                           {
                               var def = listView.Value as ListViewDefinition;
                               def.Url = Rnd.AspxFileName();
                               def.IsDefault = false;

                               listView.AddRandomWebpart();
                               listView.AddRandomWebpart();
                           });
                       });

               });

            TestModel(model);
        }

        #endregion

        #region manual list forms

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.ListForms")]
        [TestCategory("Regression.Scenarios.Webparts.Hosts")]
        public void CanDeploy_WebpartTo_ListForm_InLibrary()
        {
            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddRandomDocumentLibrary(list =>
                        {
                            list.AddHostFolder(BuiltInFolderDefinitions.Forms, folder =>
                            {
                                folder.AddHostWebPartPage(new WebPartPageDefinition
                                {
                                    FileName = "AllItems.aspx",
                                    PageLayoutTemplate = BuiltInWebPartPageTemplates.spstd1,
                                    NeedOverride = false
                                }, page =>
                                {
                                    page.AddRandomWebpart();
                                    page.AddRandomWebpart();
                                    page.AddRandomWebpart();
                                });
                            });
                        });

                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.Hosts")]
        [TestCategory("Regression.Scenarios.Webparts.ListForms")]
        public void CanDeploy_WebpartTo_ListForm_InList()
        {
            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddRandomList(list =>
                        {
                            list.AddHostWebPartPage(new WebPartPageDefinition
                            {
                                FileName = "AllItems.aspx",
                                PageLayoutTemplate = BuiltInWebPartPageTemplates.spstd1,
                                NeedOverride = false
                            }, page =>
                            {
                                page.AddRandomWebpart();
                                page.AddRandomWebpart();
                                page.AddRandomWebpart();
                            });
                        });

                });

            TestModel(model);
        }

        #endregion

        #region webpart


        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.AuthorizationFilter")]
        public void CanDeploy_Webpart_With_AuthorizationFilter()
        {
            var securityGroupDef = ModelGeneratorService.GetRandomDefinition<SecurityGroupDefinition>(def =>
            {
                def.DefaultUser = string.Empty;
                def.Owner = string.Empty;
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSecurityGroup(securityGroupDef);
            });


            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWebFeature(BuiltInWebFeatures.WikiPageHomePage.Inherit(def =>
                {
                    def.Enable = true;
                }));

                web.AddHostList(BuiltInListDefinitions.SitePages, list =>
                {
                    list.AddRandomWebPartPage(page =>
                    {
                        page.AddRandomWebpart(webPart =>
                        {
                            var wpDef = webPart.Value as WebPartDefinition;
                            wpDef.AuthorizationFilter = string.Format(";;;;{0}", securityGroupDef.Name);
                        });
                    });
                });

            });

            TestModel(siteModel, webModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts")]
        [TestCategory("Regression.Scenarios.Webparts.Hosts")]
        public void CanDeploy_WebpartTo_WebpartPage()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWebFeature(BuiltInWebFeatures.WikiPageHomePage.Inherit(def =>
                {
                    def.Enable = true;
                }));

                web.AddHostList(BuiltInListDefinitions.SitePages, list =>
                {
                    list
                        .AddRandomWebPartPage(page =>
                        {
                            page
                                .AddRandomWebpart()
                                .AddRandomWebpart();
                        })
                        .AddRandomWebPartPage(page =>
                        {
                            page
                                .AddRandomWebpart()
                                .AddRandomWebpart();
                        });
                });

            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts")]
        [TestCategory("Regression.Scenarios.Webparts.Hosts")]
        public void CanDeploy_WebpartTo_WebpartPage_WithCustomPageLayout()
        {
            var webPartPage1 = ModelGeneratorService.GetRandomDefinition<WebPartPageDefinition>(def =>
            {
                def.CustomPageLayout = CustomWebPartPageLayoutTemplates.Custom1;
            });

            var webPartPage2 = ModelGeneratorService.GetRandomDefinition<WebPartPageDefinition>(def =>
            {
                def.CustomPageLayout = CustomWebPartPageLayoutTemplates.Custom1;
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWebFeature(BuiltInWebFeatures.WikiPageHomePage.Inherit(def =>
                {
                    def.Enable = true;
                }));

                web.AddHostList(BuiltInListDefinitions.SitePages, list =>
                {
                    list
                        .AddWebPartPage(webPartPage1, page =>
                        {
                            // the content will be changed
                            page.RegExcludeFromValidation();

                            page
                                .AddRandomWebpart()
                                .AddRandomWebpart();
                        })
                        .AddWebPartPage(webPartPage2, page =>
                        {
                            // the content will be changed
                            page.RegExcludeFromValidation();

                            page
                                .AddRandomWebpart()
                                .AddRandomWebpart();
                        });
                });

            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts")]
        public void CanDeploy_Webpart_WithTitleUrl_WithTokens()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWebFeature(BuiltInWebFeatures.WikiPageHomePage.Inherit(def =>
                {
                    def.Enable = true;
                }));

                web.AddHostList(BuiltInListDefinitions.SitePages, list =>
                {
                    list
                        .AddRandomWebPartPage(page =>
                        {
                            page
                                .AddRandomWebpart(w =>
                                {
                                    (w.Value as WebPartDefinition).TitleUrl =
                                        string.Format("~sitecollection/{0}.html", Rnd.String());
                                })
                                .AddRandomWebpart(w =>
                                {
                                    (w.Value as WebPartDefinition).TitleUrl =
                                        string.Format("~site/{0}.html", Rnd.String());

                                });
                        });
                });

            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts")]
        [TestCategory("Regression.Scenarios.Webparts.Hosts")]
        public void CanDeploy_WebpartTo_VideoContentType_VideoPlayerPage()
        {
            // aiming to deploy a web part to the following page with in a library
            // forms/videocontenttype/videoplayerpage.aspx

            var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.ContentTypesEnabled = true;
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
            });

            var contentTypeName = "Video";
            var contentTypeLinkDef = new ContentTypeLinkDefinition
            {
                ContentTypeName = contentTypeName,
                ContentTypeId = BuiltInSiteContentTypeId.Video
            };

            var formsFolder = new FolderDefinition
            {
                Name = "Forms"
            };

            var contentTypeFolder = new FolderDefinition
            {
                Name = contentTypeName
            };

            var pageDefinition = new WebPartPageDefinition
            {
                FileName = "videoplayerpage.aspx",
                PageLayoutTemplate = BuiltInWebPartPageTemplates.spstd1,
                NeedOverride = false
            };

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(listDef, list =>
                {
                    list.AddContentTypeLink(contentTypeLinkDef);

                    list.AddFolder(formsFolder, forms =>
                    {
                        forms.AddFolder(contentTypeFolder, folder =>
                        {
                            folder.AddHostWebPartPage(pageDefinition, page =>
                            {
                                page.AddRandomWebpart();
                                page.AddRandomWebpart();
                                page.AddRandomWebpart();
                            });
                        });
                    });
                });
            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts")]
        [TestCategory("Regression.Scenarios.Webparts.Hosts")]
        public void CanDeploy_WebpartTo_PublishingPageWebPartZone()
        {
            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSiteFeature(RegSiteFeatures.Publishing);
            });

            TestModel(siteModel);

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWebFeature(BuiltInWebFeatures.SharePointServerPublishing.Inherit(def =>
                {
                    def.Enable = true;
                }));

                web.AddHostList(BuiltInListDefinitions.Pages, list =>
                {
                    list
                        .AddRandomPublishingPage(page =>
                        {
                            page
                                .AddRandomWebpart()
                                .AddRandomWebpart();
                        })
                        .AddRandomPublishingPage(page =>
                        {
                            page
                                .AddRandomWebpart()
                                .AddRandomWebpart();
                        });
                });

            });

            TestModel(webModel);
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts")]
        [TestCategory("Regression.Scenarios.Webparts.Hosts")]
        public void CanDeploy_WebpartTo_PublishingPageContent()
        {
            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSiteFeature(BuiltInSiteFeatures.SharePointServerPublishingInfrastructure.Inherit(def =>
                {
                    def.Enable = true;
                }));
            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWebFeature(BuiltInWebFeatures.SharePointServerPublishing.Inherit(def =>
                {
                    def.Enable = true;
                }));

                web
                    .AddHostList(BuiltInListDefinitions.Pages, list =>
                    {
                        list
                            .AddRandomPublishingPage(page =>
                            {
                                var id_1 = "g_" + Guid.NewGuid().ToString("D").Replace('-', '_');
                                var id_2 = "g_" + Guid.NewGuid().ToString("D").Replace('-', '_');

                                var id_3 = "g_" + Guid.NewGuid().ToString("D").Replace('-', '_');

                                var wpId11 = id_1
                    .Replace("g_", string.Empty)
                    .Replace("_", "-");

                                var wpId22 = id_2
                    .Replace("g_", string.Empty)
                    .Replace("_", "-");

                                var pageTemplate = new StringBuilder();

                                pageTemplate.AppendFormat("​​​​​​​​​​​​​​​​​​​​​​<div class='ms-rtestate-read ms-rte-wpbox' contentEditable='false'>");
                                pageTemplate.AppendFormat("     <div class='ms-rtestate-read {0}' id='div_{0}'>", wpId11);
                                pageTemplate.AppendFormat("     </div>");
                                pageTemplate.AppendFormat("</div>");

                                pageTemplate.AppendFormat("<div>");
                                pageTemplate.AppendFormat(" SPMeta2 publishing page.");
                                pageTemplate.AppendFormat("</div>");

                                pageTemplate.AppendFormat("​​​​​​​​​​​​​​​​​​​​​​<div class='ms-rtestate-read ms-rte-wpbox' contentEditable='false'>");
                                pageTemplate.AppendFormat("     <div class='ms-rtestate-read {0}' id='div_{0}'>", wpId22);
                                pageTemplate.AppendFormat("     </div>");
                                pageTemplate.AppendFormat("</div>");

                                (page.Value as PublishingPageDefinition).Content = pageTemplate.ToString();

                                page
                                    .AddRandomWebpart(wpNode =>
                                    {
                                        var wp = wpNode.Value as WebPartDefinition;

                                        wp.ZoneId = "wpz";
                                        wp.Id = id_1;
                                        wp.AddToPageContent = true;
                                    })
                                     .AddRandomWebpart(wpNode =>
                                     {
                                         var wp = wpNode.Value as WebPartDefinition;

                                         wp.ZoneId = "wpz";
                                         wp.Id = id_2;
                                         wp.AddToPageContent = true;
                                     })
                                    .AddRandomWebpart(wpNode =>
                                    {
                                        var wp = wpNode.Value as WebPartDefinition;

                                        wp.ZoneId = "wpz";
                                        wp.Id = id_3;
                                        wp.AddToPageContent = true;
                                    });
                            });
                    });

            });

            TestModel(siteModel, webModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts")]
        [TestCategory("Regression.Scenarios.Webparts.Hosts")]
        public void CanDeploy_WebpartTo_WikiPageContent_AsItIs()
        {
            // Some web part provision on wiki page give empty markup
            // https://github.com/SubPointSolutions/spmeta2/issues/693

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWebFeature(BuiltInWebFeatures.WikiPageHomePage.Inherit(def =>
                {
                    def.Enable = true;
                }));

                web.AddHostList(BuiltInListDefinitions.SitePages, list =>
                {
                    list
                        .AddRandomWikiPage(page =>
                        {
                            page
                                .AddRandomWebpart()
                                .AddRandomWebpart()
                                .AddRandomWebpart();
                        });
                });
            });

            TestModel(webModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts")]
        public void CanDeploy_ScriptEditorTo_WikiPageContent_As_AddToPageContent()
        {
            // Some web part provision on wiki page give empty markup
            // https://github.com/SubPointSolutions/spmeta2/issues/693

            // web part ID, zone and AddToPageContent must be as such
            // you can move these params into your web part definition

            var scriptEditor1 = new ScriptEditorWebPartDefinition
            {
                Title = string.Format("ScriptEditorWebPartDefinition - {0}", Rnd.String()),
                Id = "g_" + Guid.NewGuid().ToString("D").Replace('-', '_'),
                ZoneIndex = 10,
                ZoneId = "wpz",
                AddToPageContent = true,
                Content = String.Format("<script> console.log('M2 loves wiki page web part provision - 1 - {0}'); </script>",
                                        Rnd.String())
            };

            var scriptEditor2 = new ScriptEditorWebPartDefinition
            {
                Title = string.Format("ScriptEditorWebPartDefinition - {0}", Rnd.String()),
                Id = "g_" + Guid.NewGuid().ToString("D").Replace('-', '_'),
                ZoneIndex = 10,
                ZoneId = "wpz",
                AddToPageContent = true,
                Content = String.Format("<script> console.log('M2 loves wiki page web part provision - 2 - {0}'); </script>",
                                        Rnd.String())
            };

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWebFeature(BuiltInWebFeatures.WikiPageHomePage.Inherit(def =>
                {
                    def.Enable = true;
                }));

                web
                    .AddHostList(BuiltInListDefinitions.SitePages, list =>
                    {
                        list
                            .AddRandomWikiPage(page =>
                            {
                                page.RegExcludeFromValidation();

                                page.AddScriptEditorWebPart(scriptEditor1);
                                page.AddScriptEditorWebPart(scriptEditor2);
                            });
                    });
            });

            TestModel(webModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts")]
        [TestCategory("Regression.Scenarios.Webparts.Hosts")]
        public void CanDeploy_WebpartTo_WikiPageContent_As_AddToPageContent()
        {
            // Some web part provision on wiki page give empty markup
            // https://github.com/SubPointSolutions/spmeta2/issues/693

            // web part ID, zone and AddToPageContent must be as such
            // you can move these params into your web part definition

            var listViewWebPartDef = new ListViewWebPartDefinition
            {
                Title = String.Format("ListViewWebPartDefinition - {0}", Rnd.String()),
                Id = "g_" + Guid.NewGuid().ToString("D").Replace('-', '_'),
                ZoneIndex = 10,
                ZoneId = "wpz",
                AddToPageContent = true,
                ListUrl = BuiltInListDefinitions.SitePages.CustomUrl
            };

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWebFeature(BuiltInWebFeatures.WikiPageHomePage.Inherit(def =>
                {
                    def.Enable = true;
                }));

                web.AddHostList(BuiltInListDefinitions.SitePages, list =>
                {
                    list
                        .AddRandomWikiPage(page =>
                        {
                            page.RegExcludeFromValidation();

                            page
                                .AddListViewWebPart(listViewWebPartDef)
                                .AddRandomWebpart(wpNode =>
                                {
                                    var wp = wpNode.Value as WebPartDefinition;

                                    wp.Id = "g_" + Guid.NewGuid().ToString("D").Replace('-', '_');
                                    wp.ZoneId = "wpz";
                                    wp.AddToPageContent = true;
                                })
                                .AddRandomWebpart(wpNode =>
                                {
                                    var wp = wpNode.Value as WebPartDefinition;

                                    wp.Id = "g_" + Guid.NewGuid().ToString("D").Replace('-', '_');
                                    wp.ZoneId = "wpz";
                                    wp.AddToPageContent = true;
                                })
                                .AddRandomWebpart(wpNode =>
                                {
                                    var wp = wpNode.Value as WebPartDefinition;

                                    wp.Id = "g_" + Guid.NewGuid().ToString("D").Replace('-', '_');
                                    wp.ZoneId = "wpz";
                                    wp.AddToPageContent = true;
                                })
                                ;
                        });
                });
            });

            TestModel(webModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts")]
        [TestCategory("Regression.Scenarios.Webparts.Hosts")]
        public void CanDeploy_WebpartTo_WikiPageContent()
        {
            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWebFeature(BuiltInWebFeatures.WikiPageHomePage.Inherit(def =>
                {
                    def.Enable = true;
                }));

                web.AddHostList(BuiltInListDefinitions.SitePages, list =>
                {
                    list
                        .AddRandomWikiPage(page =>
                        {
                            page.RegExcludeFromValidation();

                            var id_1 = "g_" + Guid.NewGuid().ToString("D").Replace('-', '_');
                            var id_2 = "g_" + Guid.NewGuid().ToString("D").Replace('-', '_');

                            var id_3 = "g_" + Guid.NewGuid().ToString("D").Replace('-', '_');

                            var wpId11 = id_1
                .Replace("g_", string.Empty)
                .Replace("_", "-");

                            var wpId22 = id_2
                .Replace("g_", string.Empty)
                .Replace("_", "-");

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
                                .AddRandomWebpart(wpNode =>
                                {
                                    var wp = wpNode.Value as WebPartDefinition;

                                    wp.ZoneId = "wpz";
                                    wp.Id = id_1;
                                    wp.AddToPageContent = true;
                                })
                                 .AddRandomWebpart(wpNode =>
                                 {
                                     var wp = wpNode.Value as WebPartDefinition;

                                     wp.ZoneId = "wpz";
                                     wp.Id = id_2;
                                     wp.AddToPageContent = true;
                                 })
                                .AddRandomWebpart(wpNode =>
                                {
                                    var wp = wpNode.Value as WebPartDefinition;

                                    wp.ZoneId = "wpz";
                                    wp.Id = id_3;
                                    wp.AddToPageContent = true;
                                });
                        });
                });

            });

            TestModel(webModel);
        }

        #endregion

        #region deleting

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.Deletion")]
        public void CanDeploy_DeleteWebpartFromWebpartPage()
        {
            var wpPage = ModelGeneratorService.GetRandomDefinition<WebPartPageDefinition>();

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddHostList(BuiltInListDefinitions.SitePages, list =>
                        {
                            list
                                .AddWebPartPage(wpPage, page =>
                                {
                                    page
                                        .AddRandomWebpart()
                                        .AddRandomWebpart();
                                });
                        });

                });

            var deleteModel = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddHostList(BuiltInListDefinitions.SitePages, list =>
                        {
                            list
                               .AddHostWebPartPage(wpPage, page =>
                               {
                                   page
                                       .AddDeleteWebParts(new DeleteWebPartsDefinition());
                               });
                        });

                });

            TestModel(model, deleteModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.Deletion")]
        public void CanDeploy_DeleteWebpartFromPublishingPage()
        {
            var wpPage = ModelGeneratorService.GetRandomDefinition<PublishingPageDefinition>();

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddHostList(BuiltInListDefinitions.Pages, list =>
                        {
                            list
                                .AddPublishingPage(wpPage, page =>
                                {
                                    page
                                        .AddRandomWebpart()
                                        .AddRandomWebpart();
                                });
                        });

                });

            var deleteModel = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddHostList(BuiltInListDefinitions.Pages, list =>
                        {
                            list
                               .AddHostPublishingPage(wpPage, page =>
                               {
                                   page
                                       .AddDeleteWebParts(new DeleteWebPartsDefinition());
                               });
                        });

                });

            TestModel(model, deleteModel);
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.Deletion")]
        public void CanDeploy_DeleteWebpartFromWikiPage()
        {
            var wpPage = ModelGeneratorService.GetRandomDefinition<WikiPageDefinition>();

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddHostList(BuiltInListDefinitions.SitePages, list =>
                        {
                            list
                                .AddWikiPage(wpPage, page =>
                                {
                                    page
                                        .AddRandomWebpart()
                                        .AddRandomWebpart();
                                });
                        });

                });

            var deleteModel = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddHostList(BuiltInListDefinitions.SitePages, list =>
                        {
                            list
                               .AddHostWikiPage(wpPage, page =>
                               {
                                   page
                                       .AddDeleteWebParts(new DeleteWebPartsDefinition());
                               });
                        });

                });

            TestModel(model, deleteModel);
        }
        #endregion

        #region removing by title

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.Deletion")]
        public void CanDeploy_DeleteWebpart_ByTitle()
        {
            var wpPage = ModelGeneratorService.GetRandomDefinition<WikiPageDefinition>();

            var title1 = "title1_" + Rnd.String();
            var title2 = "title2_" + Rnd.String();

            var wp1 = ModelGeneratorService.GetRandomDefinition<WebPartDefinition>(def =>
            {
                def.Title = title1;
            });

            var wp2 = ModelGeneratorService.GetRandomDefinition<WebPartDefinition>(def =>
            {
                def.Title = title2;
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddHostList(BuiltInListDefinitions.SitePages, list =>
                {
                    list.AddWikiPage(wpPage, page =>
                    {
                        page.AddWebParts(new[] { wp1, wp2 });
                    });
                });

            });

            var wpDeletionDef = new DeleteWebPartsDefinition
            {

            };

            wpDeletionDef.WebParts.Add(new WebPartMatch
            {
                Title = title1
            });

            var deleteModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddHostList(BuiltInListDefinitions.SitePages, list =>
                {
                    list.AddHostWikiPage(wpPage, page =>
                    {
                        page.AddDeleteWebParts(wpDeletionDef);
                    });
                });

            });

            TestModel(model, deleteModel);
        }

        #endregion

        #region properties

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.Properties")]
        public void CanDeploy_V2_Webpart_With_Properties()
        {
            var webpart = ModelGeneratorService.GetRandomDefinition<WebPartDefinition>(def =>
            {
                def.WebpartFileName = string.Empty;
                def.WebpartType = string.Empty;
                def.WebpartXmlTemplate = BuiltInWebPartTemplates.ContentEditorWebPart;
            });

            webpart.Properties.Add(new WebPartProperty
            {
                Name = "AllowEdit",
                Value = Rnd.Bool().ToString().ToLower()
            });

            webpart.Properties.Add(new WebPartProperty
            {
                Name = "Description",
                Value = Rnd.Bool().ToString().ToLower()
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web
                    .AddHostSitePagesList(list =>
                    {
                        list.AddRandomWebPartPage(page =>
                        {
                            page.AddWebPart(webpart);
                        });
                    });
            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.Properties")]
        public void CanDeploy_V3_Webpart_With_Properties()
        {
            var webpart = ModelGeneratorService.GetRandomDefinition<WebPartDefinition>(def =>
            {
                def.WebpartFileName = string.Empty;
                def.WebpartType = string.Empty;
                def.WebpartXmlTemplate = BuiltInWebPartTemplates.ScriptEditorWebPart;
            });

            webpart.Properties.Add(new WebPartProperty
            {
                Name = "AllowEdit",
                Value = Rnd.Bool().ToString().ToLower()
            });

            webpart.Properties.Add(new WebPartProperty
            {
                Name = "Description",
                Value = Rnd.Bool().ToString().ToLower()
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web
                    .AddHostSitePagesList(list =>
                    {
                        list.AddRandomWebPartPage(page =>
                        {
                            page.AddWebPart(webpart);
                        });
                    });
            });

            TestModel(model);
        }


        #endregion


    }
}
