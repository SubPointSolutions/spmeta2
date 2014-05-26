using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Models;
using SPMeta2.SSOM.Samples.FoundationAppTests.Common;
using SPMeta2.SSOM.Samples.FoundationAppTests.Models;
using SPMeta2.Syntax.Default;

namespace SPMeta2.SSOM.Samples.FoundationAppTests.Syntax
{
    // here is a big TODO as Resharper shows a lot of recommendations to cut lambdas 

    [TestClass]
    public class BaseSyntaxDemoSandboxTests : FoundationAppTestBase
    {
        #region resources

        [TestInitialize]
        public void Setup()
        {
            // it is a good place to change TestSetting
            InitTestSettings();
        }

        [TestCleanup]
        public void Cleanup()
        {
            CleanupResources();
        }

        #endregion

        #region tests

        [TestMethod]
        [TestCategory("TODO")]
        public void SyntaxSecurityGroupsAndRolesDemo()
        {
            WithStaticSPSiteSandbox(TestSiteCollectionTitle, (site, web) =>
            {
                var model = new ModelNode()
                                    .WithSecurityRoles(securityRoles =>
                                    {
                                        securityRoles
                                            .AddSecurityRole(AppSecurityRoleModels.ContractorRole)
                                            .AddSecurityRole(AppSecurityRoleModels.AnnouncmentsRole);
                                    })
                                    .WithSecurityGroups(securityGroups =>
                                    {
                                        securityGroups
                                            .AddSecurityGroup(AppSecurityGroupModels.Contractors, group =>
                                            {
                                                group.AddSecurityRoleLink(AppSecurityRoleModels.ContractorRole);
                                            })
                                            .AddSecurityGroup(AppSecurityGroupModels.AnnounsmentsEditors, group =>
                                            {
                                                group.AddSecurityRoleLink(AppSecurityRoleModels.AnnouncmentsRole);
                                            });
                                    });

               // ServiceFactory.ModelService.DeployModel(site, model);
            });
        }

        [TestMethod]
        [TestCategory("TODO")]
        public void SyntaxFieldAndContentTypesDemo()
        {
            WithStaticSPSiteSandbox(TestSiteCollectionTitle, (site, web) =>
            {
                var model = new ModelNode()
                                .WithFields(fields =>
                                {
                                    fields
                                        //.AddField(AppFieldModels.DepartmentCode, field =>
                                        //{
                                        //    //field.OnCreated((FieldDefinition fieldModel, SPField spField) =>
                                        //    //{
                                        //    //    spField
                                        //    //        .MakeRequired()
                                        //    //        .MakeTitle("Department ID");
                                        //    //});
                                        //})
                                        .AddField(AppFieldModels.DepartmentRef)
                                        .AddField(AppFieldModels.ShowOnTheMainPage);
                                })
                                .WithContentTypes(contentTypes =>
                                {
                                    contentTypes
                                        .AddContentType(AppContentTypeModels.FoundationAnnouncement)
                                        .AddContentType(AppContentTypeModels.DepartmentTask)
                                        .AddContentType(AppContentTypeModels.CompanyDepartment, contentType =>
                                        {
                                            contentType
                                                .AddContentTypeFieldLink(AppFieldModels.DepartmentCode)
                                                .AddContentTypeFieldLink(AppFieldModels.DepartmentRef)
                                                .AddContentTypeFieldLink(AppFieldModels.ShowOnTheMainPage);
                                        });
                                });

                //ServiceFactory.ModelService.DeployModel(site, model);
            });
        }

        [TestMethod]
        [TestCategory("TODO")]
        public void ListsContentTypesPagesAndWebPartsSyntaxDemo()
        {
            WithStaticSPSiteSandbox(TestSiteCollectionTitle, (site, web) =>
            {
                var model = new ModelNode()
                     .WithLists(siteLists =>
                     {
                         siteLists
                         .AddList(AppListModels.CompanyDepartments)
                         .AddList(AppListModels.DepartmentTasks)
                         .AddList(AppListModels.CompanyDepartments, list =>
                         {
                             list
                                 .AddContentTypeLink(AppContentTypeModels.CompanyDepartment)
                                 .AddContentTypeLink(AppContentTypeModels.DepartmentTask)
                                 .AddWebPartPage(AppWebPartPageModels.RootWeb.SitePages.AboutFounationApp,
                                     page =>
                                     {
                                         page
                                             .AddWebPart(AppWebPartsModels.AboutFounationAppPage.AboutUsWebPart)
                                             .AddWebPart(AppWebPartsModels.AboutFounationAppPage.ContactUsWebPart)
                                             .AddWebPart(AppWebPartsModels.AboutFounationAppPage.OurValuesWebPart,
                                              webpart =>
                                              {
                                                  //webpart
                                                  //    .OnCreated((webPartMode, spWebPart) =>
                                                  //    {
                                                  //        spWebPart
                                                  //            .MakeChromeType(PartChromeType.None)
                                                  //            .MakeWidth(450)
                                                  //            .MakeHeight(300);
                                                  //    });
                                              });
                                     });
                         });
                     });

                //ServiceFactory.ModelService.DeployModel(web, model);
            });
        }

        [TestMethod]
        [TestCategory("TODO")]
        public void BaseSyntaxGeneralDemo()
        {
            var model = new ModelNode()
                                .WithSecurityRoles(securityRoles =>
                                {
                                    securityRoles
                                        .AddSecurityRole(AppSecurityRoleModels.ContractorRole)
                                        .AddSecurityRole(AppSecurityRoleModels.AnnouncmentsRole);
                                })
                                .WithSecurityGroups(securityGroups =>
                                {
                                    securityGroups
                                        .AddSecurityGroup(AppSecurityGroupModels.Contractors, group =>
                                        {
                                            group.AddSecurityRoleLink(AppSecurityRoleModels.ContractorRole);
                                        })
                                        .AddSecurityGroup(AppSecurityGroupModels.AnnounsmentsEditors, group =>
                                        {
                                            group.AddSecurityRoleLink(AppSecurityRoleModels.AnnouncmentsRole);
                                        });
                                })
                                .WithFields(siteFields =>
                                {
                                    siteFields
                                        //.AddField(AppFieldModels.DepartmentCode, field =>
                                        //{
                                        //    //field.OnCreated((FieldDefinition fiedlModel, SPField spField) =>
                                        //    //{
                                        //    //    spField
                                        //    //        .MakeRequired()
                                        //    //        .MakeTitle("Department ID");
                                        //    //});
                                        //})
                                        .AddField(AppFieldModels.DepartmentRef)
                                        .AddField(AppFieldModels.DepartmentCode)
                                        .AddField(AppFieldModels.DepartmentCode);
                                })
                                .WithContentTypes(contentTypes =>
                                {
                                    contentTypes
                                        .AddContentType(AppContentTypeModels.CompanyDepartment)
                                        .AddContentType(AppContentTypeModels.CompanyDepartment)
                                        .AddContentType(AppContentTypeModels.CompanyDepartment, contentType =>
                                        {
                                            contentType
                                                .AddContentTypeFieldLink(AppFieldModels.DepartmentCode)
                                                .AddContentTypeFieldLink(AppFieldModels.DepartmentRef)
                                                .AddContentTypeFieldLink(AppFieldModels.ShowOnTheMainPage);
                                        });
                                })
                                .WithLists(siteLists =>
                                {
                                    siteLists
                                        .AddList(AppListModels.CompanyDepartments)
                                        .AddList(AppListModels.DepartmentTasks)
                                        .AddList(AppListModels.CompanyDepartments, list =>
                                        {
                                            list
                                                .AddContentTypeLink(AppContentTypeModels.CompanyDepartment)
                                                .AddContentTypeLink(AppContentTypeModels.DepartmentTask)
                                                .AddWebPartPage(AppWebPartPageModels.RootWeb.SitePages.AboutFounationApp, page =>
                                                {
                                                    page
                                                    .AddWebPart(AppWebPartsModels.RootWeb.SitePages.AboutFounationAppPage.AboutUsWebPart)
                                                    .AddWebPart(AppWebPartsModels.RootWeb.SitePages.AboutFounationAppPage.OurDepartmentsWebPart)
                                                    .AddWebPart(AppWebPartsModels.RootWeb.SitePages.AboutFounationAppPage.OurServicesWebPart);
                                                });
                                        });
                                });

            // there might be a model lllokup from siteModel/SiteDefinition
            // not sure yet

            //AppFieldModels.DepartmentCode.OnCreated((FieldDefinition model, SPField spField) =>
            //{
            //    spField
            //        .MakeNotRequired()
            //        .MakeRequired();
            //});

            //AppWebPartsModels.RootWeb.SitePages.AboutFounationAppPage.AboutUsWebPart.OnCreated((WebPartDefinition model, WebPart webpart) =>
            //{
            //    webpart
            //        .MakeTitle("sd")
            //        .MakeXsltListViewBindingToView((SPView)null);
            //});
        }

        #endregion
    }
}
