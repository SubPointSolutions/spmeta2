using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Containers.Standard;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Definitions;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;
using SPMeta2.Validation.Validators.Relationships;

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


        #region default

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts")]
        public void CanDeploy_WebpartToWebpartPage()
        {
            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddHostList(BuiltInListDefinitions.SitePages, list =>
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
        public void CanDeploy_WebpartToPublishingPageWebPartZone()
        {
            var siteModel = SPMeta2Model
                                .NewSiteModel(site =>
                                {
                                    site
                                        .AddSiteFeature(RegSiteFeatures.Publishing);
                                });

            TestModel(siteModel);

            var webModel = SPMeta2Model
               .NewWebModel(web =>
               {
                   web
                       .AddFeature(RegWebFeatures.Publishing)
                       .AddHostList(BuiltInListDefinitions.Pages, list =>
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
        public void CanDeploy_WebpartToPublishingPageContent()
        {
            var webModel = SPMeta2Model
               .NewWebModel(web =>
               {
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

            TestModel(webModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts")]
        public void CanDeploy_WebpartToWikiPageContent()
        {
            var webModel = SPMeta2Model
               .NewWebModel(web =>
               {
                   web
                       .AddHostList(BuiltInListDefinitions.SitePages, list =>
                       {
                           list
                               .AddRandomWikiPage(page =>
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
    }

}
