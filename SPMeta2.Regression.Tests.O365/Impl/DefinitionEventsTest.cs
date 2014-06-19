using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Regression.Model.Definitions;
using SPMeta2.Regression.Tests.Impl.Events;
using SPMeta2.Syntax.Default;
using Microsoft.SharePoint.Client.WebParts;

namespace SPMeta2.Regression.Tests.O365.Impl
{
    [TestClass]
    public class DefinitionEventsTest : DefinitionEventsTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_FieldDefinition()
        {
            WithEventHooks(hooks =>
            {
                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site
                        .AddField(RegSiteFields.BooleanField, field =>
                        {
                            AssertEventHooks<Field>(field, hooks);
                        });
                });

                WithProvisionRunners(runner => runner.DeploySiteModel(model));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_ContentTypeDefinition()
        {
            WithEventHooks(hooks =>
            {
                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site
                        .AddContentType(RegContentTypes.CustomItem, contentType =>
                        {
                            AssertEventHooks<ContentType>(contentType, hooks);
                        });
                });

                WithProvisionRunners(runner => runner.DeploySiteModel(model));
            });
        }

        #endregion

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_ContentTypeFieldLinkDefinition()
        {
            WithEventHooks(hooks =>
            {
                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site
                        .AddField(RegSiteFields.BooleanField)
                        .AddContentType(RegContentTypes.CustomItem, ct =>
                        {
                            ct.AddContentTypeFieldLink(RegSiteFields.BooleanField, link =>
                            {
                                AssertEventHooks<FieldLink>(link, hooks);
                            });
                        });
                });

                WithProvisionRunners(runner => runner.DeploySiteModel(model));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_ContentTypeLinkDefinition()
        {
            WithEventHooks(hooks =>
            {
                var model = SPMeta2Model.NewWebModel(web =>
                {
                    web
                        .AddList(RegLists.DocumentLibrary, list =>
                        {
                            list
                                .AddContentTypeLink(RegContentTypes.CustomDocument, link =>
                                {
                                    AssertEventHooks<ContentType>(link, hooks);
                                });
                        });


                });

                WithProvisionRunners(runner => runner.DeployWebModel(model));
            });

            WithEventHooks(hooks =>
            {
                var model = SPMeta2Model.NewWebModel(web =>
                {
                    web
                        .AddList(RegLists.GenericList, list =>
                        {
                            list
                                .AddContentTypeLink(RegContentTypes.CustomItem, link =>
                                {
                                    AssertEventHooks<ContentType>(link, hooks);
                                });
                        });


                });

                WithProvisionRunners(runner => runner.DeployWebModel(model));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        [ExpectedException(typeof(NotImplementedException))]
        public override void CanRaiseEvents_FarmDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_FeatureDefinition()
        {
            WithEventHooks(hooks =>
            {
                var siteModel = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddSiteFeature(RegSiteFeatures.PublishingSite, feature =>
                    {
                        AssertEventHooks<Feature>(feature, hooks);
                    });
                });

                WithProvisionRunners(runner => runner.DeploySiteModel(siteModel));
            });

            WithEventHooks(hooks =>
            {
                var webModel = SPMeta2Model.NewWebModel(web =>
                {
                    web.AddWebFeature(RegWebFeatures.PublishingWeb, feature =>
                    {
                        AssertEventHooks<Feature>(feature, hooks);
                    });
                });

                WithProvisionRunners(runner => runner.DeployWebModel(webModel));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_FolderDefinition()
        {
            WithEventHooks(hooks =>
            {
                var webModel = SPMeta2Model.NewWebModel(site =>
                {
                    site.AddList(RegLists.GenericList, list =>
                    {
                        list
                            .AddFolder(RegFolders.Folder1, folder =>
                            {
                                AssertEventHooks<Folder>(folder, hooks);
                            });
                    });
                });

                WithProvisionRunners(runner => runner.DeployWebModel(webModel));
            });

            WithEventHooks(hooks =>
            {
                var webModel = SPMeta2Model.NewWebModel(site =>
                {
                    site.AddList(RegLists.DocumentLibrary, list =>
                    {
                        list
                            .AddFolder(RegFolders.Folder1, folder =>
                            {
                                AssertEventHooks<Folder>(folder, hooks);
                            });
                    });
                });

                WithProvisionRunners(runner => runner.DeployWebModel(webModel));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_ListDefinition()
        {
            WithEventHooks(hooks =>
            {
                var webModel = SPMeta2Model.NewWebModel(site =>
                {
                    site.AddList(RegLists.GenericList, list =>
                    {
                        AssertEventHooks<List>(list, hooks);
                    });
                });

                WithProvisionRunners(runner => runner.DeployWebModel(webModel));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_ListItemDefinition()
        {
            WithEventHooks(hooks =>
            {
                var webModel = SPMeta2Model.NewWebModel(site =>
                {
                    site.AddList(RegLists.GenericList, list =>
                    {
                        list
                            .AddListItem(new ListItemDefinition
                            {
                                Title = "test item"
                            },
                            item =>
                            {
                                AssertEventHooks<ListItem>(item, hooks);
                            });
                    });
                });

                WithProvisionRunners(runner => runner.DeployWebModel(webModel));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_ListItemFieldValueDefinition()
        {
            var titleValue = "test item";

            WithEventHooks(hooks =>
            {
                var webModel = SPMeta2Model.NewWebModel(site =>
                {
                    site.AddList(RegLists.GenericList, list =>
                    {
                        list
                            .AddListItem(new ListItemDefinition
                            {
                                Title = titleValue
                            },
                            item =>
                            {
                                item
                                    .AddListItemFieldValue("Title", titleValue, valueItem =>
                                    {
                                        AssertEventHooks<ListItem>(valueItem, hooks);
                                    });
                            });
                    });
                });

                WithProvisionRunners(runner => runner.DeployWebModel(webModel));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_ListViewDefinition()
        {
            WithEventHooks(hooks =>
            {
                var webModel = SPMeta2Model.NewWebModel(site =>
                {
                    site.AddList(RegLists.GenericList, list =>
                    {
                        list
                            .AddView(RegListViews.View1, view =>
                            {
                                AssertEventHooks<View>(view, hooks);
                            });
                    });
                });

                WithProvisionRunners(runner => runner.DeployWebModel(webModel));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_ModuleFileDefinition()
        {
            WithEventHooks(hooks =>
            {
                var webModel = SPMeta2Model.NewWebModel(site =>
                {
                    site.AddList(RegLists.DocumentLibrary, library =>
                    {
                        library
                            .AddModuleFile(RegModuleFiles.HelloSharePoint, moduleFile =>
                            {
                                AssertEventHooks<File>(moduleFile, hooks);
                            });
                    });
                });

                WithProvisionRunners(runner => runner.DeployWebModel(webModel));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        [ExpectedException(typeof(NotImplementedException))]
        public override void CanRaiseEvents_PropertyDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        [ExpectedException(typeof(NotImplementedException))]
        public override void CanRaiseEvents_PublishingPageDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_QuickLunchNavigationNodeDefinition()
        {
            WithEventHooks(hooks =>
            {
                var model = SPMeta2Model.NewWebModel(web =>
                {
                    web
                        .AddQuickLaunchNavigationNode(RegQuickLaunchNavigation.GoogleLink, link =>
                    {
                        AssertEventHooks<NavigationNode>(link, hooks);
                    });
                });

                WithProvisionRunners(runner => runner.DeployWebModel(model));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_SecurityGroupDefinition()
        {
            WithEventHooks(hooks =>
            {
                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddSecurityGroup(RegSecurityGroups.SecurityGroup1, securityGroup =>
                    {
                        AssertEventHooks<Group>(securityGroup, hooks);
                    });
                });

                WithProvisionRunners(runner => runner.DeploySiteModel(model));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_SecurityGroupLinkDefinition()
        {
            WithEventHooks(hooks =>
            {
                // ensure group
                var siteModel = SPMeta2Model.NewSiteModel(site => site.AddSecurityGroup(RegSecurityGroups.SecurityGroup1));
                WithProvisionRunners(runner => runner.DeploySiteModel(siteModel));

                var webModel = SPMeta2Model.NewWebModel(site =>
                {
                    site.AddList(RegLists.GenericSecurableList, list =>
                    {
                        list
                            .AddSecurityGroupLink(RegSecurityGroups.SecurityGroup1, securityGroupLink =>
                            {
                                securityGroupLink
                                    .AddSecurityRoleLink(RegSecurityRoles.SecurityRole1);

                                AssertEventHooks<RoleAssignment>(securityGroupLink, hooks);
                            });
                    });
                });

                WithProvisionRunners(runner => runner.DeployWebModel(webModel));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_SecurityRoleDefinition()
        {
            WithEventHooks(hooks =>
            {
                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddList(RegSecurityRoles.SecurityRole1, securityRole =>
                    {
                        AssertEventHooks<RoleDefinition>(securityRole, hooks);
                    });
                });

                WithProvisionRunners(runner => runner.DeploySiteModel(model));
            });

        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_SecurityRoleLinkDefinition()
        {
            WithEventHooks(hooks =>
            {
                // ensure group
                var siteModel = SPMeta2Model.NewSiteModel(site => site.AddSecurityGroup(RegSecurityGroups.SecurityGroup1));
                WithProvisionRunners(runner => runner.DeploySiteModel(siteModel));

                var webModel = SPMeta2Model.NewWebModel(site =>
                {
                    site.AddList(RegLists.GenericSecurableList, list =>
                    {
                        list
                            .AddSecurityGroupLink(RegSecurityGroups.SecurityGroup1, securityGroupLink =>
                            {
                                securityGroupLink
                                    .AddSecurityRoleLink(RegSecurityRoles.SecurityRole1, securityRoleLink =>
                                    {
                                        AssertEventHooks<RoleDefinition>(securityRoleLink, hooks);
                                    });
                            });
                    });
                });

                WithProvisionRunners(runner => runner.DeployWebModel(webModel));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_SiteDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_SP2013WorkflowDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_SP2013WorkflowSubscriptionDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_UserCustomActionDefinition()
        {
            WithEventHooks(hooks =>
            {
                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddUserCustomAction(RegUserCustomActions.jQueryScript, customAction =>
                    {
                        AssertEventHooks<UserCustomAction>(customAction, hooks);
                    });
                });

                WithProvisionRunners(runner => runner.DeploySiteModel(model));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_WebDefinition()
        {
            WithEventHooks(hooks =>
            {
                var model = SPMeta2Model.NewWebModel(parentWeb =>
                {
                    parentWeb.AddWeb(RegWebs.BlankWeb, web =>
                    {
                        AssertEventHooks<Web>(web, hooks);
                    });
                });

                WithProvisionRunners(runner => runner.DeployWebModel(model));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_WebPartDefinition()
        {
            WithEventHooks(hooks =>
            {
                var model = SPMeta2Model.NewWebModel(web =>
                {
                    web
                        .AddList(BuiltInListDefinitions.SitePages, sitePages =>
                        {
                            sitePages
                                .AddWebPartPage(RegWebPartPages.WebPartContainerPage, page =>
                                {
                                    page
                                        .AddWebPart(RegWebParts.ContentEditorWebPart, webpart =>
                                        {
                                            AssertEventHooks<WebPart>(webpart, hooks);
                                        });
                                });
                        });
                });

                WithProvisionRunners(runner => runner.DeployWebModel(model));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_WebPartPageDefinition()
        {
            WithEventHooks(hooks =>
            {
                var model = SPMeta2Model.NewWebModel(web =>
                {
                    web
                        .AddList(BuiltInListDefinitions.SitePages, sitePages =>
                        {
                            sitePages
                                .AddWebPartPage(RegWebPartPages.Page1, page =>
                                {
                                    AssertEventHooks<File>(page, hooks);
                                });
                        });
                });

                WithProvisionRunners(runner => runner.DeployWebModel(model));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_WikiPageDefinition()
        {
            WithEventHooks(hooks =>
            {
                var model = SPMeta2Model.NewWebModel(web =>
                {
                    web
                        .AddList(BuiltInListDefinitions.SitePages, sitePages =>
                        {
                            sitePages
                                .AddWikiPage(RegWikiPages.Page1, page =>
                                {
                                    AssertEventHooks<File>(page, hooks);
                                });
                        });
                });

                WithProvisionRunners(runner => runner.DeployWebModel(model));
            });
        }
    }
}
