using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Regression.Models.DynamicModels;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.ModelBuilders
{
    public class DynamicModelBuilder
    {
        #region methods

        #region security groups

        public virtual ModelNode GetWebWithDefaultSecurityGroups()
        {
            var webDefinition = new ModelNode { Value = new WebDefinition() };

            webDefinition
                .WithSecurityGroups(groups =>
                {
                    groups
                        .AddSecurityGroup(DynamicSecurityGroupModels.Contractors)
                        .AddSecurityGroup(DynamicSecurityGroupModels.Students)
                        .AddSecurityGroup(DynamicSecurityGroupModels.Workers);
                });


            return webDefinition;
        }

        #endregion

        #region security roles

        public virtual ModelNode GetWebWithDefaultSecurityRoles()
        {
            var webDefinition = new ModelNode { Value = new WebDefinition() };

            webDefinition
                .WithSecurityRoles(roles =>
                {
                    roles
                        .AddSecurityRole(DynamicSecurityRoleModels.ContractorRole)
                        .AddSecurityRole(DynamicSecurityRoleModels.StudentRole)
                        .AddSecurityRole(DynamicSecurityRoleModels.WorkerRole);
                });


            return webDefinition;
        }

        #endregion

        #region fields

        public virtual ModelNode GetSiteWithDefaultFields()
        {
            var siteDefinition = new ModelNode { Value = new SiteDefinition() };

            siteDefinition
                .WithFields(fields =>
                {
                    fields
                        .AddField(DynamicFieldModels.BooleanField)
                        .AddField(DynamicFieldModels.CurrencyField)
                        .AddField(DynamicFieldModels.DateTimeField)
                        .AddField(DynamicFieldModels.GuidField)
                        .AddField(DynamicFieldModels.LookupField)
                        .AddField(DynamicFieldModels.MultiChoiceField)
                        .AddField(DynamicFieldModels.NoteField)
                        .AddField(DynamicFieldModels.NumberField)
                        .AddField(DynamicFieldModels.TextField)
                        .AddField(DynamicFieldModels.URLField)
                        .AddField(DynamicFieldModels.UserField);
                });

            return siteDefinition;
        }

        #endregion

        #region content types

        public virtual ModelNode GetSiteWithDefaultFieldsAndContentTypes()
        {
            var siteWithFieldsModel = GetSiteWithDefaultFields();

            siteWithFieldsModel
                .WithContentTypes(contentTypes =>
                {
                    contentTypes
                        .AddContentType(DynamicContentTypeModels.ItemContentType)
                        .AddContentType(DynamicContentTypeModels.DocumentContentType)
                        .AddContentType(DynamicContentTypeModels.TaskContentType)
                        .AddContentType(DynamicContentTypeModels.LinkContentType);
                });

            return siteWithFieldsModel;
        }

        public virtual ModelNode GetSiteWithDefaultFieldsAndFilledContentTypes()
        {
            var siteWithFieldsModel = GetSiteWithDefaultFields();

            siteWithFieldsModel
                .WithContentTypes(contentTypes =>
                {
                    contentTypes
                        .AddContentType(DynamicContentTypeModels.ItemContentType, contentType =>
                        {
                            contentType
                                .AddContentTypeFieldLink(DynamicFieldModels.BooleanField)
                                .AddContentTypeFieldLink(DynamicFieldModels.CurrencyField)
                                .AddContentTypeFieldLink(DynamicFieldModels.DateTimeField);
                        })
                        .AddContentType(DynamicContentTypeModels.DocumentContentType, contentType =>
                        {
                            contentType
                                .AddContentTypeFieldLink(DynamicFieldModels.GuidField)
                                .AddContentTypeFieldLink(DynamicFieldModels.LookupField)
                                .AddContentTypeFieldLink(DynamicFieldModels.MultiChoiceField);
                        })
                        .AddContentType(DynamicContentTypeModels.TaskContentType, contentType =>
                        {
                            contentType
                                .AddContentTypeFieldLink(DynamicFieldModels.NoteField)
                                .AddContentTypeFieldLink(DynamicFieldModels.NumberField)
                                .AddContentTypeFieldLink(DynamicFieldModels.TextField);
                        })
                        .AddContentType(DynamicContentTypeModels.LinkContentType, contentType =>
                        {
                            contentType
                                .AddContentTypeFieldLink(DynamicFieldModels.URLField)
                                .AddContentTypeFieldLink(DynamicFieldModels.UserField);
                        });
                });

            return siteWithFieldsModel;
        }

        #endregion

        #region lists

        public virtual ModelNode GetWebWithDefaultLists()
        {
            var webWithLists = new ModelNode { Value = new WebDefinition() };

            webWithLists
                .AddList(DynamicListModels.DocumentLibrary)
                .AddList(DynamicListModels.AnnouncementsList)
                .AddList(DynamicListModels.ContactsList)
                .AddList(DynamicListModels.EventsList)
                .AddList(DynamicListModels.GenericList)
                .AddList(DynamicListModels.LinksList)
                .AddList(DynamicListModels.TasksList);

            return webWithLists;
        }

        public virtual ModelNode GetWebWithDefaultListsAndContentTypes()
        {
            var webWithLists = new ModelNode { Value = new WebDefinition() };

            webWithLists
                .AddList(DynamicListModels.DocumentLibrary, list =>
                {
                    list
                       .AddContentTypeLink(DynamicContentTypeModels.DocumentContentType);
                })
                .AddList(DynamicListModels.AnnouncementsList)
                .AddList(DynamicListModels.ContactsList)
                .AddList(DynamicListModels.EventsList)
                .AddList(DynamicListModels.GenericList, list =>
                {
                    list
                       .AddContentTypeLink(DynamicContentTypeModels.ItemContentType)
                       .AddContentTypeLink(DynamicContentTypeModels.LinkContentType);
                })
                .AddList(DynamicListModels.LinksList, list =>
                {
                    list
                       .AddContentTypeLink(DynamicContentTypeModels.LinkContentType);
                })
                .AddList(DynamicListModels.TasksList, list =>
                {
                    list
                        .AddContentTypeLink(DynamicContentTypeModels.TaskContentType);
                });

            return webWithLists;
        }

        public virtual ModelNode GetWebWithDefaultListsContentTypesAndViews()
        {
            var webWithLists = new ModelNode { Value = new WebDefinition() };

            webWithLists
                .AddList(DynamicListModels.DocumentLibrary, list =>
                {
                    list
                       .AddContentTypeLink(DynamicContentTypeModels.DocumentContentType)
                       .AddView(DynamicListViewModels.AllDocuments);
                })
                .AddList(DynamicListModels.AnnouncementsList)
                .AddList(DynamicListModels.ContactsList)
                .AddList(DynamicListModels.EventsList)
                .AddList(DynamicListModels.GenericList, list =>
                {
                    list
                       .AddContentTypeLink(DynamicContentTypeModels.ItemContentType)
                       .AddContentTypeLink(DynamicContentTypeModels.LinkContentType)
                       .AddView(DynamicListViewModels.AllItems);
                })
                .AddList(DynamicListModels.LinksList, list =>
                {
                    list
                       .AddContentTypeLink(DynamicContentTypeModels.LinkContentType)
                       .AddView(DynamicListViewModels.AllItems);
                })
                .AddList(DynamicListModels.TasksList, list =>
                {
                    list
                    .AddContentTypeLink(DynamicContentTypeModels.TaskContentType)
                    .AddView(DynamicListViewModels.AllTasks);
                });

            return webWithLists;
        }

        public virtual ModelNode GetWebWithDefaultListsAndSecurityGroups()
        {
            var webWithLists = new ModelNode { Value = new WebDefinition() };

            webWithLists
                .AddList(DynamicListModels.DocumentLibrary, list =>
                {
                    list
                       .AddSecurityGroupLink(DynamicSecurityGroupModels.Contractors, group =>
                       {
                           group
                               .AddSecurityRoleLink(DynamicSecurityRoleModels.ContractorRole)
                               .AddSecurityRoleLink(DynamicSecurityRoleModels.StudentRole);
                       });
                })
                .AddList(DynamicListModels.AnnouncementsList)
                .AddList(DynamicListModels.ContactsList)
                .AddList(DynamicListModels.EventsList)
                .AddList(DynamicListModels.GenericList, list =>
                {
                    list
                       .AddSecurityGroupLink(DynamicSecurityGroupModels.Students, group =>
                       {
                           group
                               .AddSecurityRoleLink(DynamicSecurityRoleModels.WorkerRole)
                               .AddSecurityRoleLink(DynamicSecurityRoleModels.StudentRole);
                       });
                })
                .AddList(DynamicListModels.LinksList, list =>
                {
                    list
                      .AddSecurityGroupLink(DynamicSecurityGroupModels.Workers, group =>
                      {
                          group
                              .AddSecurityRoleLink(DynamicSecurityRoleModels.WorkerRole)
                              .AddSecurityRoleLink(DynamicSecurityRoleModels.StudentRole);
                      });
                })
                .AddList(DynamicListModels.TasksList, list =>
                {

                });

            return webWithLists;
        }

        #endregion

        #region web part pages

        public virtual ModelNode GetWebWithDefaultWebPartPages()
        {
            var webModel = new ModelNode { Value = new WebDefinition() };

            webModel
                .WithLists(lists =>
                {
                    lists
                        .AddList(DynamicListModels.SitePages, list =>
                        {
                            list
                                .AddWebPartPage(DynamicWebPartPageModels.SitePages.WebPartPage1)
                                .AddWebPartPage(DynamicWebPartPageModels.SitePages.WebPartPage2)
                                .AddWebPartPage(DynamicWebPartPageModels.SitePages.WebPartPage3)
                                .AddWebPartPage(DynamicWebPartPageModels.SitePages.WebPartPage4)
                                .AddWebPartPage(DynamicWebPartPageModels.SitePages.WebPartPage5)
                                .AddWebPartPage(DynamicWebPartPageModels.SitePages.WebPartPage6)
                                .AddWebPartPage(DynamicWebPartPageModels.SitePages.WebPartPage7)
                                .AddWebPartPage(DynamicWebPartPageModels.SitePages.WebPartPage8);
                        });

                });

            return webModel;
        }

        public virtual ModelNode GetWebWithDefaultWebPartPagesWithSecurity()
        {
            var webModel = new ModelNode { Value = new WebDefinition() };

            webModel
                .WithLists(lists =>
                {
                    lists
                        .AddList(DynamicListModels.SitePages, list =>
                        {
                            list
                                .AddWebPartPage(DynamicWebPartPageModels.SitePages.WebPartPage1, page =>
                                {
                                    page
                                        .AddSecurityGroupLink(DynamicSecurityGroupModels.Contractors, group =>
                                        {
                                            group
                                                .AddSecurityRoleLink(DynamicSecurityRoleModels.ContractorRole)
                                                .AddSecurityRoleLink(DynamicSecurityRoleModels.StudentRole);
                                        });
                                })
                                .AddWebPartPage(DynamicWebPartPageModels.SitePages.WebPartPage2)
                                .AddWebPartPage(DynamicWebPartPageModels.SitePages.WebPartPage3)
                                .AddWebPartPage(DynamicWebPartPageModels.SitePages.WebPartPage4)
                                .AddWebPartPage(DynamicWebPartPageModels.SitePages.WebPartPage5)
                                .AddWebPartPage(DynamicWebPartPageModels.SitePages.WebPartPage6)
                                .AddWebPartPage(DynamicWebPartPageModels.SitePages.WebPartPage7)
                                .AddWebPartPage(DynamicWebPartPageModels.SitePages.WebPartPage8);
                        });

                });

            return webModel;
        }

        #endregion

        #region wiki pages

        public virtual ModelNode GetWebWithDefaultWikiPages()
        {
            var webModel = new ModelNode { Value = new WebDefinition() };

            webModel
                .WithLists(lists =>
                {
                    lists
                        .AddList(DynamicListModels.SitePages, list =>
                        {
                            list
                                .AddWikiPage(DynamicWebPartPageModels.SitePages.WikiPage1)
                                .AddWikiPage(DynamicWebPartPageModels.SitePages.WikiPage2)
                                .AddWikiPage(DynamicWebPartPageModels.SitePages.WikiPage3)
                                .AddWikiPage(DynamicWebPartPageModels.SitePages.WikiPage4)
                                .AddWikiPage(DynamicWebPartPageModels.SitePages.WikiPage5);
                        });

                });

            return webModel;
        }

        #endregion

        #region publishing pages

        //public virtual WebDefinition GetWebWithDefaultPublishingPages()
        //{
        //    var webModel = new WebDefinition ();

        //    webModel
        //        .WithLists(lists =>
        //        {
        //            lists
        //                .AddList(DynamicListModels.SitePages, list =>
        //                {
        //                    list.RequireSelfProcessing = false;

        //                    list
        //                        .AddPuPage(DynamicWebPartPageModels.SitePages.PublishingArticleLeft)
        //                        .AddWikiPage(DynamicWebPartPageModels.SitePages.PublishingArticleRight)
        //                        .AddWikiPage(DynamicWebPartPageModels.SitePages.PublishingBlankWebPartPage)
        //                        .AddWikiPage(DynamicWebPartPageModels.SitePages.PublishingEnterpriseWikiPage)
        //                        .AddWikiPage(DynamicWebPartPageModels.SitePages.PublishingProjectPage);
        //                });

        //        });

        //    return webModel;
        //}

        #endregion


        #endregion
    }
}
