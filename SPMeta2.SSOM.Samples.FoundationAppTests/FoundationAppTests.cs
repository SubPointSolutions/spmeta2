using System;
using System.Globalization;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.SSOM.Behaviours;
using SPMeta2.SSOM.DefaultSyntax;
using SPMeta2.SSOM.Samples.FoundationAppTests.Common;
using SPMeta2.SSOM.Samples.FoundationAppTests.Models;
using SPMeta2.Syntax.Default;

namespace SPMeta2.SSOM.Samples.FoundationAppTests
{
    //[TestClass]
    public class FoundationAppTests : FoundationAppTestBase
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

        [TestMethod]
        [TestCategory("SSOM")]
        public void CanDeployRootQuickLaunchNavigation()
        {
            var webModel = new ModelNode { Value = new WebDefinition { RequireSelfProcessing = false } }
                .WithQuickLaunchNavigation(nav =>
                {
                    nav
                        .AddQuickLaunchNavigationNode(AppQuickLaunchNavigationNodeModels.RootHome)
                        .AddQuickLaunchNavigationNode(AppQuickLaunchNavigationNodeModels.Help, help =>
                        {
                            help
                                .AddQuickLaunchNavigationNode(AppQuickLaunchNavigationNodeModels.ItHelp)
                                .AddQuickLaunchNavigationNode(AppQuickLaunchNavigationNodeModels.MedHelp);
                        });
                });

            WithStaticSPSiteSandbox(TestSiteCollectionTitle, (site, web) => ModelService.DeployModel(web, webModel));
        }

        [TestMethod]
        [TestCategory("SSOM")]
        public void CanDelopyFieldToList()
        {
            var webModel = new ModelNode { Value = new WebDefinition { RequireSelfProcessing = false } }
                .WithLists(lists =>
                {
                    lists
                        .AddList(AppListModels.CompanyAnnouncements, list =>
                        {
                            list
                                .AddField(AppFieldModels.DepartmentCode);

                        });
                });

            WithStaticSPSiteSandbox(TestSiteCollectionTitle, (site, web) => ModelService.DeployModel(web, webModel));
        }


        [TestMethod]
        [TestCategory("SSOM")]
        public void CanDelopyFoldersToList()
        {
            var webModel = new ModelNode { Value = new WebDefinition { RequireSelfProcessing = false } }
              .WithLists(lists =>
              {
                  lists
                      .AddList(AppListModels.CompanyAnnouncements, list =>
                      {
                          list
                              .AddFolder(AppFolders.Year2012, yearFolder =>
                              {
                                  yearFolder
                                      .AddFolder(AppFolders.January)
                                      .AddFolder(AppFolders.Febuary)
                                      .AddFolder(AppFolders.March);
                              })
                              .AddFolder(AppFolders.Year2013, yearFolder =>
                              {
                                  yearFolder
                                     .AddFolder(AppFolders.January)
                                     .AddFolder(AppFolders.Febuary)
                                     .AddFolder(AppFolders.March);
                              })
                              .AddFolder(AppFolders.Year2014, yearFolder =>
                              {
                                  yearFolder
                                     .AddFolder(AppFolders.January)
                                     .AddFolder(AppFolders.Febuary)
                                     .AddFolder(AppFolders.March);
                              });
                      });
              });

            WithStaticSPSiteSandbox(TestSiteCollectionTitle, (site, web) => ModelService.DeployModel(web, webModel));
        }


        [TestMethod]
        [TestCategory("SSOM")]
        public void CanDelopyFoldersToLibrary()
        {
            var webModel = new ModelNode { Value = new WebDefinition { RequireSelfProcessing = false } }
              .WithLists(lists =>
              {
                  lists
                      .AddList(AppListModels.SitePages, list =>
                      {
                          list
                              .AddFolder(AppFolders.Year2012, yearFolder =>
                              {
                                  yearFolder
                                      .AddFolder(AppFolders.January)
                                      .AddFolder(AppFolders.Febuary)
                                      .AddFolder(AppFolders.March);
                              })
                              .AddFolder(AppFolders.Year2013, yearFolder =>
                              {
                                  yearFolder
                                     .AddFolder(AppFolders.January)
                                     .AddFolder(AppFolders.Febuary)
                                     .AddFolder(AppFolders.March);
                              })
                              .AddFolder(AppFolders.Year2014, yearFolder =>
                              {
                                  yearFolder
                                     .AddFolder(AppFolders.January)
                                     .AddFolder(AppFolders.Febuary)
                                     .AddFolder(AppFolders.March);
                              });
                      });
              });

            WithStaticSPSiteSandbox(TestSiteCollectionTitle, (site, web) => ModelService.DeployModel(web, webModel));
        }

        [TestMethod]
        [TestCategory("SSOM")]
        public void CanDeployResourceFilesToContentType()
        {
            var siteModel = new ModelNode { Value = new SiteDefinition { RequireSelfProcessing = false } }
                .WithContentTypes(contentTypes =>
                {
                    contentTypes
                        .AddContentType(AppContentTypeModels.FoundationAnnouncement, contentType =>
                        {
                            contentType
                                .AddModuleFile(AppModuleFiles.JQuery);
                        });
                });

            WithStaticSPSiteSandbox(TestSiteCollectionTitle, (site, web) => ModelService.DeployModel(site, siteModel));
        }

        [TestMethod]
        [TestCategory("SSOM")]
        public void CanDeployResourceFiles()
        {
            var webModel = new ModelNode { Value = new WebDefinition { RequireSelfProcessing = false } }
               .WithLists(lists =>
               {
                   lists
                       .AddList(AppListModels.StyleLibrary, list =>
                       {
                           list
                               .AddModuleFile(AppModuleFiles.JQuery);
                       });
               });

            WithStaticSPSiteSandbox(TestSiteCollectionTitle, (site, web) => ModelService.DeployModel(web, webModel));
        }

        [TestMethod]
        [TestCategory("SSOM")]
        public void CanDeploySiteScriptRegistration()
        {
            var webModel = new ModelNode { Value = new SiteDefinition { RequireSelfProcessing = false } }
               .WithUserCustomActions(actions =>
               {
                   actions
                       .AddUserCustomAction(AppCustomActionModels.SiteScriptJQuery);
               });

            WithStaticSPSiteSandbox(TestSiteCollectionTitle, (site, web) => ModelService.DeployModel(site, webModel));
        }

        [TestMethod]
        [TestCategory("SSOM")]
        public void CanDeploySiteFeatures()
        {
            var webModel = new ModelNode { Value = new SiteDefinition { RequireSelfProcessing = false } }
               .WithSiteFeatures(siteFeatures =>
               {
                   siteFeatures
                       .AddFeature(AppSiteFeatureModels.Workflows);
               });

            WithStaticSPSiteSandbox(TestSiteCollectionTitle, (site, web) => ModelService.DeployModel(site, webModel));
        }

        [TestMethod]
        [TestCategory("SSOM")]
        public void CanDeployWebFeatures()
        {
            var webModel = new ModelNode { Value = new SiteDefinition { RequireSelfProcessing = false } }
               .WithSiteFeatures(siteFeatures =>
               {
                   siteFeatures
                       .AddFeature(AppWebFeatureModels.WorkflowsCanUseAppPermissions);
               });

            WithStaticSPSiteSandbox(TestSiteCollectionTitle, (site, web) => ModelService.DeployModel(web, webModel));
        }

        [TestMethod]
        [TestCategory("SSOM")]
        public void CanDeploySubWebs()
        {
            var webModel = new ModelNode { Value = new SiteDefinition { RequireSelfProcessing = false } }
               .WithSubWebs(webs =>
               {
                   webs
                       .AddWeb(AppWebModels.Projects, projectWeb =>
                       {
                           projectWeb
                               .AddWeb(AppWebModels.P1)
                               .AddWeb(AppWebModels.P2);
                       })
                       .AddWeb(AppWebModels.Teams);
               });

            WithStaticSPSiteSandbox(TestSiteCollectionTitle, (site, web) => ModelService.DeployModel(web, webModel));
        }

        [TestMethod]
        [TestCategory("SSOM")]
        public void CanDeployRootSiteSecurityRoles()
        {
            var webModel = new ModelNode { Value = new SiteDefinition { RequireSelfProcessing = false } }
               .WithSecurityRoles(roles =>
               {
                   roles
                       .AddSecurityRole(AppSecurityRoleModels.AnnouncmentsRole)
                       .AddSecurityRole(AppSecurityRoleModels.ContractorRole);
               });

            WithStaticSPSiteSandbox(TestSiteCollectionTitle, (site, web) => ModelService.DeployModel(web, webModel));
        }

        [TestMethod]
        [TestCategory("SSOM")]
        public void CanDeployRootSiteSecurityGroups()
        {
            var webModel = new ModelNode { Value = new SiteDefinition { RequireSelfProcessing = false } }
                .WithSecurityRoles(roles =>
                {
                    roles
                        .AddSecurityRole(AppSecurityRoleModels.AnnouncmentsRole)
                        .AddSecurityRole(AppSecurityRoleModels.ContractorRole);
                }).WithSecurityGroups(groups =>
                {
                    groups
                        .AddSecurityGroup(AppSecurityGroupModels.AnnounsmentsEditors, group =>
                        {
                            group
                                .AddSecurityRoleLink(AppSecurityRoleModels.AnnouncmentsRole);
                        })
                        .AddSecurityGroup(AppSecurityGroupModels.Contractors, group =>
                        {
                            group
                                .AddSecurityRoleLink(AppSecurityRoleModels.ContractorRole);
                        })
                        .AddSecurityGroup(AppSecurityGroupModels.Students);
                });

            WithStaticSPSiteSandbox(TestSiteCollectionTitle, (site, web) => ModelService.DeployModel(web, webModel));
        }

        [TestMethod]
        [TestCategory("SSOM")]
        public void CanDeploySiteMetadata()
        {
            WithStaticSPSiteSandbox(TestSiteCollectionTitle, (site, web) => ModelService.DeployIntranetMetadataFields(site));
        }

        [TestMethod]
        [TestCategory("SSOM")]
        public void CanDeployRootWebLists()
        {
            WithStaticSPSiteSandbox(TestSiteCollectionTitle, (site, web) =>
            {
                DeployFieldModel(site, web);
                DeployContentTypeModel(site, web);

                ModelService.DeployRootLists(web);
            });
        }

        [TestMethod]
        [TestCategory("SSOM")]
        public void CanDeployRootWebPartPages()
        {
            WithStaticSPSiteSandbox(TestSiteCollectionTitle, (site, web) =>
            {
                ModelService.DeployRootSiteWebPartPages(web);
            });
        }

        [TestMethod]
        [TestCategory("SSOM")]
        public void CanDeployRootLookupFixs()
        {
            WithStaticSPSiteSandbox(TestSiteCollectionTitle, (site, web) => ModelService.FixRootWebLookupFields(site));
        }

        [TestMethod]
        [TestCategory("SSOM")]
        public void CanDeployFieldModel()
        {
            WithStaticSPSiteSandbox(TestSiteCollectionTitle, (site, web) =>
            {
                DeployFieldModel(site, web);
            });
        }

        private void DeployFieldModel(SPSite site, SPWeb web)
        {
            var siteModel = new ModelNode { Value = new SiteDefinition { RequireSelfProcessing = false } }
                      .WithFields(fields =>
                      {
                          fields
                              .AddField(AppFieldModels.ShowOnTheMainPage)
                              .AddField(AppFieldModels.DepartmentRef)
                              .AddField(AppFieldModels.DepartmentCode, field =>
                              {
                                  field
                                      .OnCreated((FieldDefinition fieldDefition, SPField spField) =>
                                      {
                                          spField
                                              .MakeHidden(true)
                                              .MakeTitle("Test sdfsd")
                                              .MakeRequired()
                                              .MakeItPretty()
                                              .MakeDefaultValues("5")
                                              .MakeChoices(new string[]{
                                                  "sd",
                                                  "sdfdsfs",
                                                  "sdfsdfsd"
                                              });

                                      });
                              });
                      });

            ModelService.DeployModel(site, siteModel);
        }

        [TestMethod]
        [TestCategory("SSOM")]
        public void CanDeployContentTypeModel()
        {
            WithStaticSPSiteSandbox(TestSiteCollectionTitle, (site, web) =>
            {
                DeployContentTypeModel(site, web);
            });
        }

        private void DeployContentTypeModel(SPSite site, SPWeb web)
        {
            var siteModel = new ModelNode { Value = new SiteDefinition { RequireSelfProcessing = false } }
                .WithContentTypes(contentTypes =>
                {
                    contentTypes
                        .AddContentType(AppContentTypeModels.FoundationAnnouncement, contentType =>
                        {
                            contentType
                                .AddContentTypeFieldLink(AppFieldModels.ShowOnTheMainPage);
                        })
                        .AddContentType(AppContentTypeModels.CompanyDepartment, contentType =>
                        {
                            contentType
                                .AddContentTypeFieldLink(AppFieldModels.DepartmentCode);
                        })
                        .AddContentType(AppContentTypeModels.DepartmentTask, contentType =>
                        {
                            contentType
                                .AddContentTypeFieldLink(AppFieldModels.DepartmentRef);
                        });
                });
            ModelService.DeployModel(site, siteModel);
        }

        [TestMethod]
        [TestCategory("SSOM")]
        public void CanDeployAllMedtadataModel()
        {
            WithStaticSPSiteSandbox(TestSiteCollectionTitle, (site, web) =>
            {
                DeployFieldModel(site, web);
                DeployContentTypeModel(site, web);
            });
        }

        [TestMethod]
        [TestCategory("SSOM")]
        public void CanDeployAllModel()
        {
            WithStaticSPSiteSandbox(TestSiteCollectionTitle, (site, web) =>
            {
                var webModel = new ModelNode { Value = new SiteDefinition { RequireSelfProcessing = false } }
                                    .WithSecurityRoles(roles => roles
                                                                    .AddSecurityRole(AppSecurityRoleModels.AnnouncmentsRole)
                                                                    .AddSecurityRole(AppSecurityRoleModels.ContractorRole))
                                    .WithSecurityGroups(groups => groups
                                                                      .AddSecurityGroup(AppSecurityGroupModels.AnnounsmentsEditors, group => @group
                                                                                                                                                 .AddSecurityRoleLink(AppSecurityRoleModels.AnnouncmentsRole))
                                                                      .AddSecurityGroup(AppSecurityGroupModels.Contractors, group => @group
                                                                                                                                         .AddSecurityRoleLink(AppSecurityRoleModels.ContractorRole))
                                                                      .AddSecurityGroup(AppSecurityGroupModels.Students))


                                   .WithLists(lists =>
                                   {
                                       lists
                                           .AddList(AppListModels.SitePages, list =>
                                           {
                                               list
                                                   .AddContentTypeLink(AppContentTypeModels.FoundationAnnouncement)
                                                   .AddView(AppListViewModels.RootWeb.CompanyAnnouncements.MainPageView)
                                                   .AddSecurityGroupLink(AppSecurityGroupModels.AnnounsmentsEditors, group => @group.AddSecurityRoleLink(AppSecurityRoleModels.AnnouncmentsRole))
                                                   .WithPages(pages =>
                                                   {
                                                       pages
                                                           .AddWebPartPage(AppWebPartPageModels.RootWeb.SitePages.AboutFounationApp, page =>
                                                           {
                                                               page
                                                                   .AddWebPart(AppWebPartsModels.RootWeb.SitePages.AboutFounationAppPage.AboutUsWebPart, webpart =>
                                                                   {
                                                                       webpart.OnCreated((webpartModel, spWebPart) =>
                                                                       {
                                                                           spWebPart
                                                                               .MakeContentEditorText("About us text here!")
                                                                                       .MakeWidth(800)
                                                                                       .MakeTitleUrl("http://www.microsoft.com/australia/about")
                                                                                       .MakeTitle("About Microsoft");
                                                                       });
                                                                   });
                                                           });
                                                   });
                                           });
                                   });


                ModelService.DeployModel(web, webModel);
            });
        }

        [TestMethod]
        [TestCategory("SSOM")]
        public void CanDeployWikiPages()
        {
            var webModel = new ModelNode { Value = new SiteDefinition { RequireSelfProcessing = false } }
                                .WithLists(lists =>
                                {
                                    lists
                                        .AddList(AppListModels.SitePages, list =>
                                        {
                                            list
                                                .WithWikiPages(pages =>
                                                {
                                                    pages
                                                        .AddWikiPage(WikiPageModels.SitePages.ProducstPage)
                                                        .AddWikiPage(WikiPageModels.SitePages.TermsPage);
                                                });

                                        });

                                });

            WithStaticSPSiteSandbox(TestSiteCollectionTitle, (site, web) => ModelService.DeployModel(web, webModel));
        }

        //[TestMethod]
        // not yet
        public void CanAddWebpartToWikiPage()
        {
            // http://blog.mastykarz.nl/programmatically-adding-web-parts-rich-content-sharepoint-2010/
            // http://passionatetechie.blogspot.com.au/2011/11/how-to-add-web-part-on-sharepoint-2010.html

            // okay, technically, it is possible to add web part to the wiki page
            // however, unification for that needs to be done - (1) token replace or (2) first/particular zone in layout?

            // TODO

            return;

            //

            //WithStaticSPSiteSandbox(TestSiteCollectionTitle, (site, web) =>
            //{
            //    SPFile page = web.GetFile("SitePages/Products.aspx");

            //    using (var wpmgr = page.GetLimitedWebPartManager(PersonalizationScope.Shared))
            //    {
            //        Guid storageKey = Guid.NewGuid();
            //        string wpId = String.Format("g_{0}", storageKey.ToString().Replace('-', '_'));

            //        XmlElement p = new XmlDocument().CreateElement("p");
            //        p.InnerText = "Hello World";

            //        ContentEditorWebPart cewp = new ContentEditorWebPart
            //        {
            //            Content = p,
            //            ID = wpId
            //        };

            //        wpmgr.AddWebPart(cewp, "wpz", 0);

            //        string marker = String.Format(CultureInfo.InvariantCulture,
            //            "<div class=\"ms-rtestate-read ms-rte-wpbox\" contentEditable=\"false\"><div class=\"ms-rtestate-read {0}\" id=\"div_{0}\"></div><div style='display:none' id=\"vid_{0}\"></div></div>",
            //            new object[] { storageKey.ToString("D") });

            //        SPListItem item = page.Item;

            //        //var zoneMarker = "class=\"ms-rte-layoutszone-outer\"";
            //        var wikiContent = item[SPBuiltInFieldId.WikiField] as string;

            //        item[SPBuiltInFieldId.WikiField] = wikiContent.Replace("$webpart$", marker);
            //        item.Update();
            //    }
            //});
        }

        #region new model tree

        [TestMethod]
        [TestCategory("SSOM")]
        public void CanBuildUpnewModel()
        {
            var siteModel = new ModelNode { Value = new SiteDefinition() };

            siteModel
                .WithFields(fields =>
                {
                    fields
                        .AddField(AppFieldModels.DepartmentCode)
                        .AddField(AppFieldModels.DepartmentRef)
                        .AddField(AppFieldModels.ShowOnTheMainPage);

                    fields
                        .AddFields(AppFieldModels.DepartmentCode,
                                   AppFieldModels.DepartmentRef,
                                   AppFieldModels.ShowOnTheMainPage);

                })
                .WithContentTypes(contentTypes =>
                {
                    contentTypes
                        .AddContentType(AppContentTypeModels.CompanyDepartment, contentType =>
                        {
                            contentType
                                .AddContentTypeFieldLinks(AppFieldModels.DepartmentCode,
                                                          AppFieldModels.DepartmentRef);
                        })
                        .AddContentType(AppContentTypeModels.FoundationAnnouncement, contentType =>
                        {
                            contentType.AddContentTypeFieldLinks(new[] { AppFieldModels.DepartmentCode, AppFieldModels.DepartmentCode });
                        })
                        .AddContentType(AppContentTypeModels.DepartmentTask);
                })
                .WithSecurityRoles(securityRoles =>
                {
                    securityRoles
                        .AddSecurityRole(AppSecurityRoleModels.AnnouncmentsRole)
                        .AddSecurityRole(AppSecurityRoleModels.ContractorRole);
                })
                .WithSecurityGroups(securityGroups =>
                {
                    securityGroups
                        .AddSecurityGroup(AppSecurityGroupModels.AnnounsmentsEditors, group =>
                        {

                        });

                });
        }

        #endregion
    }
}
