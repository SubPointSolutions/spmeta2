using System;
using System.Linq;
using Microsoft.SharePoint.Client.WebParts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Common.Utils;
using SPMeta2.CSOM.DefaultSyntax;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Tests.Base;
using SPMeta2.CSOM.Tests.Models;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Extensions;
using SPMeta2.SSOM.DefaultSyntax;
using SPMeta2.Syntax.Default;
using WebPartDefinition = SPMeta2.Definitions.WebPartDefinition;
using System.Collections.Generic;

namespace SPMeta2.CSOM.Tests
{

    public static class HomePage
    {
        public static WebPartDefinition Tiles = new WebPartDefinition
        {
            Title = "Tiles",
            Id = "hpTiles",
            ZoneId = "Header",
            ZoneIndex = 10,
            WebpartXmlTemplate = WebPartMakup.HomePage_Tiles
        };

        public static WebPartDefinition Newsfeed = new WebPartDefinition
        {
            Title = "Newsfeed",
            Id = "hpNewsfeed",
            ZoneId = "Header",
            ZoneIndex = 20,
            WebpartXmlTemplate = WebPartMakup.HomePage_Newsfeed
        };

        public static WebPartDefinition Links = new WebPartDefinition
        {
            Title = "Links",
            Id = "hpLinks",
            ZoneId = "Header",
            ZoneIndex = 30,
            WebpartXmlTemplate = WebPartMakup.HomePage_QuickLinks
        };
    }

    //[TestClass]
    public class WebPartModelTests : ClientOMSharePointTestBase
    {
        #region resources

        [TestInitialize]
        public void Setup()
        {
            InitTestSettings();
        }

        [TestCleanup]
        public void Cleanup()
        {
            CleanupResources();
        }

        #endregion

        #region tests

        public string WebUrl { get; set; }

        [TestMethod]
        [TestCategory("CSOM")]
        public void CanAddWebPartsOnThePage()
        {


            var model = new ModelNode { Value = new WebDefinition { RequireSelfProcessing = false } }
                                  .WithLists(lists =>
                                  {
                                      lists
                                          .AddList(ListModels.SitePages, list =>
                                          {
                                              list
                                                  .AddWebPartPage(WebPartPageModels.SitePages.Page5, page =>
                                                  {
                                                      page
                                                       .AddWebPart(HomePage.Tiles)
                                                       .AddWebPart(HomePage.Newsfeed)
                                                       .AddWebPart(HomePage.Links);
                                                  });
                                          });

                                  });

            WithStaticSharePointClientContext(context =>
            {
                model
                    .WithNode(HomePage.Tiles, tileWebPart =>
                    {
                        tileWebPart.OnCreating((WebPartDefinition def, WebPart wp) =>
                        {
                            var site = context.Site;
                            var rootWeb = site.RootWeb;
                            var currentWeb = context.Web;

                            var linkList = currentWeb.Lists.GetByTitle("Tiles");

                            context.Load(linkList);
                            context.Load(linkList.RootFolder, r => r.ServerRelativeUrl);

                            context.ExecuteQuery();

                            def.WebpartXmlTemplate =
                                  WebpartXmlExtensions.LoadWebpartXmlDocument(HomePage.Links.WebpartXmlTemplate)
                                          .BindXsltListViewWebPartToList(linkList.Id.ToString())
                                          .SetTitle("Links")
                                          .SetTitleUrl(linkList.RootFolder.ServerRelativeUrl)
                                          .ToString();

                            Console.Write("");
                        });
                    })
                    .WithNode(HomePage.Newsfeed, newsFeed =>
                    {
                        newsFeed.OnCreating((WebPartDefinition def, WebPart wp) =>
                        {
                            def.WebpartXmlTemplate =
                                  WebpartXmlExtensions.LoadWebpartXmlDocument(def.WebpartXmlTemplate)
                                          .SetTitle("NewsFeed")
                                          .ToString();
                        });
                    })
                    .WithNode(HomePage.Links, linkWebPart =>
                    {
                        linkWebPart.OnCreating((WebPartDefinition def, WebPart wp) =>
                        {
                            var site = context.Site;
                            var rootWeb = site.RootWeb;
                            var currentWeb = context.Web;

                            var linkList = currentWeb.Lists.GetByTitle("Quick Links");

                            context.Load(linkList);
                            context.Load(linkList.RootFolder, r => r.ServerRelativeUrl);

                            context.ExecuteQuery();

                            def.WebpartXmlTemplate =
                                  WebpartXmlExtensions.LoadWebpartXmlDocument(HomePage.Links.WebpartXmlTemplate)
                                          .BindXsltListViewWebPartToList(linkList.Id.ToString())
                                          .SetTitle("Links")
                                          .SetTitleUrl(linkList.RootFolder.ServerRelativeUrl)
                                          .ToString();

                            Console.Write("");
                        });
                    });

                ServiceFactory.DeployModel(WebModelHost.FromClientContext(context), model);
                ServiceFactory.DeployModel(WebModelHost.FromClientContext(context), model);
            });
        }



        #endregion
    }
}
