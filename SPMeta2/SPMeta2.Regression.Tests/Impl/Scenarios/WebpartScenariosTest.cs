using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Tests.Definitions;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
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

        #region default

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts")]
        public void CanDeploy_WebpartToWebpartPage()
        {
            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddList(BuiltInListDefinitions.SitePages, list =>
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
                       .AddList(BuiltInListDefinitions.Pages, list =>
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
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts")]
        public void CanDeploy_WebpartToWikiPageContent()
        {
            var webModel = SPMeta2Model
               .NewWebModel(web =>
               {
                   web
                       .AddList(BuiltInListDefinitions.SitePages, list =>
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

                                   var wikiTemplate = new StringBuilder();

                                   wikiTemplate.AppendFormat("​​​​​​​​​​​​​​​​​​​​​​<div class='ms-rtestate-read ms-rte-wpbox' contentEditable='false'>");
                                   wikiTemplate.AppendFormat("     <div class='ms-rtestate-read {0}' id='div_{0}'>", wpId11);
                                   wikiTemplate.AppendFormat("     </div>");
                                   wikiTemplate.AppendFormat("</div>");

                                   wikiTemplate.AppendFormat("<div>");
                                   wikiTemplate.AppendFormat(" HA HA HA HA HA");
                                   wikiTemplate.AppendFormat("</div>");

                                   wikiTemplate.AppendFormat("​​​​​​​​​​​​​​​​​​​​​​<div class='ms-rtestate-read ms-rte-wpbox' contentEditable='false'>");
                                   wikiTemplate.AppendFormat("     <div class='ms-rtestate-read {0}' id='div_{0}'>", wpId22);
                                   wikiTemplate.AppendFormat("     </div>");
                                   wikiTemplate.AppendFormat("</div>");

                                   //(page.Value as WikiPageDefinition).Content = wikiTemplate.ToString();

                                   page
                                       .AddRandomWebpart(wp =>
                                       {
                                           (wp.Value as WebPartDefinition).ZoneId = "wpz";
                                           (wp.Value as WebPartDefinition).Id = id_1;
                                       })
                                        .AddRandomWebpart(wp =>
                                        {
                                            (wp.Value as WebPartDefinition).ZoneId = "wpz";
                                            (wp.Value as WebPartDefinition).Id = id_2;
                                        })
                                       .AddRandomWebpart(wp =>
                                       {
                                           (wp.Value as WebPartDefinition).ZoneId = "wpz";
                                           (wp.Value as WebPartDefinition).Id = id_3;
                                       });
                               });
                       });

               });

            TestModel(webModel);
        }

        #endregion
    }

}
