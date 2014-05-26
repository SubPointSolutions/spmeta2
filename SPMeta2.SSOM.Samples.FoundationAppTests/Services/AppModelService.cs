using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.SSOM.Behaviours;
using SPMeta2.SSOM.DefaultSyntax;
using SPMeta2.SSOM.Samples.FoundationAppTests.Models;
using SPMeta2.SSOM.Services;
using SPMeta2.Syntax.Default;

namespace SPMeta2.SSOM.Samples.FoundationAppTests.Services
{
    public class AppModelService : SSOMProvisionService
    {
        #region properties

        public static readonly AppModelService Instance = new AppModelService();

        #endregion

        #region methods

        #region metadata

        public void DeployIntranetMetadataFields(SPSite site)
        {
            var siteModel = new ModelNode { Value = new SiteDefinition() };

            BuildFields(siteModel);
            BuildContentTypes(siteModel);

            DeployModel(site, siteModel);
        }

        private void BuildFields(ModelNode siteModel)
        {
            siteModel
                .WithFields(fields =>
                {
                    fields
                        .AddField(AppFieldModels.ShowOnTheMainPage)
                        .AddField(AppFieldModels.DepartmentRef);
                    //.AddField(AppFieldModels.DepartmentCode, field =>
                    //{
                    //    //field.OnCreated((fieldModel, spField) =>
                    //    //{
                    //    //    spField
                    //    //        .MakeRequired()
                    //    //        .MakeTitle("Department ID");
                    //    //});
                    //});
                });
        }

        private void BuildContentTypes(ModelNode siteModel)
        {
            siteModel
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
        }

        #endregion

        #region root site

        public void DeployRootLists(SPWeb web)
        {
            var webModel = new ModelNode { Value = new SiteDefinition { RequireSelfProcessing = false } }
                                    .WithLists(lists =>
                                    {
                                        lists
                                            .AddList(AppListModels.CompanyAnnouncements, list =>
                                            {
                                                list
                                                    .AddContentTypeLink(AppContentTypeModels.FoundationAnnouncement)
                                                    .AddView(AppListViewModels.RootWeb.CompanyAnnouncements.MainPageView)
                                                    .AddSecurityGroupLink(AppSecurityGroupModels.AnnounsmentsEditors, group =>
                                                    {
                                                        group
                                                            .AddSecurityRoleLink(AppSecurityRoleModels.AnnouncmentsRole);
                                                    });
                                            })
                                            .AddList(AppListModels.CompanyDepartments, list =>
                                            {
                                                list
                                                    .AddContentTypeLink(AppContentTypeModels.CompanyDepartment)
                                                    .AddView(AppListViewModels.RootWeb.Departments.AboutUsPageView)
                                                    .AddSecurityGroupLink(AppSecurityGroupModels.Contractors, group =>
                                                    {
                                                        group
                                                            .AddSecurityRoleLink(AppSecurityRoleModels.ContractorRole);
                                                    });
                                            })
                                            .AddList(AppListModels.DepartmentTasks, list =>
                                            {
                                                list
                                                    .AddContentTypeLink(AppContentTypeModels.DepartmentTask);
                                            });
                                    });

            DeployModel(web, webModel);
        }

        public void DeployRootSiteWebPartPages(SPWeb web)
        {
            var sitePagesLibrary = web.Lists["Site Pages"];

            var listModel = new ModelNode { Value = new SiteDefinition { RequireSelfProcessing = false } }
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
                                                                .MakeContentEditorText("Some about us text.")
                                                                        .MakeWidth(800)
                                                                        .MakeTitleUrl("http://www.microsoft.com/australia/about")
                                                                        .MakeTitle("About Microsoft");
                                                        });
                                                    })
                                                    .AddWebPart(AppWebPartsModels.RootWeb.SitePages.AboutFounationAppPage.OurValuesWebPart, webpart =>
                                                    {
                                                        webpart.OnCreated((webpartModel, spWebPart) =>
                                                        {
                                                            spWebPart
                                                                .MakeContentEditorText("Some coolvalue we have!")
                                                                .MakeWidth(250);
                                                        });
                                                    })
                                                    .AddWebPart(AppWebPartsModels.RootWeb.SitePages.AboutFounationAppPage.OurDepartmentsWebPart, webpart =>
                                                    {
                                                        webpart.OnCreated((webpartModel, spWebPart) =>
                                                        {
                                                            var departmentList = web.GetList(SPUrlUtility.CombineUrl(web.ServerRelativeUrl, AppListModels.CompanyDepartments.GetListUrl()));
                                                            var departmentAboutUsView = departmentList.Views[AppListViewModels.RootWeb.Departments.AboutUsPageView.Title];

                                                            spWebPart.MakeXsltListViewBindingToView(departmentAboutUsView);
                                                        });
                                                    })
                                                    .AddWebPart(AppWebPartsModels.RootWeb.SitePages.AboutFounationAppPage.OurServicesWebPart)
                                                    .AddWebPart(AppWebPartsModels.RootWeb.SitePages.AboutFounationAppPage.ContactUsWebPart);
                                            });
                                    });

            DeployModel(sitePagesLibrary, listModel);
        }

        public void FixRootWebLookupFields(SPSite site)
        {
            // TODO

            var siteModel = new ModelNode { Value = new SiteDefinition { RequireSelfProcessing = false } };
            var rootWeb = site.RootWeb;

            var companyDepartmenList = rootWeb.GetList(SPUrlUtility.CombineUrl(rootWeb.ServerRelativeUrl,
                                                                               AppListModels.CompanyDepartments.GetListUrl()));

            siteModel
                .WithFields(fields =>
                {
                    //fields
                    //    .AddField(AppFieldModels.DepartmentRef, field =>
                    //    {
                    //        //field.OnCreated((fieldModel, spField) =>
                    //        //{
                    //        //    spField.MakeLookupBinding(companyDepartmenList, "Title");
                    //        //});
                    //    });
                });

            DeployModel(site, siteModel);
        }

        #endregion

        #endregion
    }
}
