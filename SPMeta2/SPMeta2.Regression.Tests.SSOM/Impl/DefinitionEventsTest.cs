using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Definitions;
using SPMeta2.Regression.Model.Definitions;
using SPMeta2.Regression.Tests.Common;
using SPMeta2.Regression.Tests.Impl.Events;
using SPMeta2.Syntax.Default;
using Microsoft.SharePoint.Navigation;
using Microsoft.SharePoint.WorkflowServices;

namespace SPMeta2.Regression.Tests.SSOM.Impl
{
    [TestClass]
    public class DefinitionEventsTest : DefinitionEventsTestBase
    {
        #region contructors

        public DefinitionEventsTest()
        {
            EnableDefinitionValidation = false;
        }

        #endregion

        #region methods

        protected override void InitRunnerTypes()
        {
            ProvisionRunnerAssemblies.Clear();

            // should be run only on-premis
            ProvisionRunnerAssemblies.Add("SPMeta2.Regression.Runners.SSOM.dll");
        }

        protected override void WithEventHooks(Action<EventHooks> hooks)
        {
            if (ProvisionRunnerAssemblies.Count > 0)
                base.WithEventHooks(hooks);
        }

        #endregion

        #region tests

        [TestInitialize]
        public void TestInit()
        {
            InitLazyRunnerConnection();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            DisposeLazyRunnerConnection();
        }

        [TestMethod]
        [TestCategory("Regression.Events.SSOM")]
        public override void CanRaiseEvents_FieldDefinition()
        {
            WithEventHooks(hooks =>
            {
                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site
                        .AddField(RegSiteFields.BooleanField, field =>
                        {
                            AssertEventHooks<SPField>(field, hooks);
                        });
                });

                WithProvisionRunners(runner => runner.DeploySiteModel(model));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.SSOM")]
        public override void CanRaiseEvents_ContentTypeDefinition()
        {
            WithEventHooks(hooks =>
            {
                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site
                        .AddContentType(RegContentTypes.CustomItem, contentType =>
                        {
                            AssertEventHooks<SPContentType>(contentType, hooks);
                        });
                });

                WithProvisionRunners(runner => runner.DeploySiteModel(model));
            });
        }

        #endregion

        [TestMethod]
        [TestCategory("Regression.Events.SSOM")]
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
                                AssertEventHooks<SPFieldLink>(link, hooks);
                            });
                        });
                });

                WithProvisionRunners(runner => runner.DeploySiteModel(model));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.SSOM")]
        public override void CanRaiseEvents_ContentTypeLinkDefinition()
        {
            WithEventHooks(hooks =>
            {
                // ensure content type
                var siteModel = SPMeta2Model.NewSiteModel(site =>
                {
                    site
                        .AddContentType(RegContentTypes.CustomDocument);
                });

                WithProvisionRunners(runner => runner.DeploySiteModel(siteModel));

                var model = SPMeta2Model.NewWebModel(web =>
                {
                    web
                        .AddList(RegLists.DocumentLibrary, list =>
                        {
                            list
                                .AddContentTypeLink(RegContentTypes.CustomDocument, link =>
                                {
                                    AssertEventHooks<SPContentType>(link, hooks);
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
                                    AssertEventHooks<SPContentType>(link, hooks);
                                });
                        });


                });

                WithProvisionRunners(runner => runner.DeployWebModel(model));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.SSOM")]
        [ExpectedException(typeof(NotImplementedException))]
        public override void CanRaiseEvents_FarmDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.SSOM")]
        public override void CanRaiseEvents_FeatureDefinition()
        {
            WithEventHooks(hooks =>
            {
                var siteModel = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddSiteFeature(RegSiteFeatures.PublishingSite, feature =>
                    {
                        AssertEventHooks<SPFeature>(feature, hooks);
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
                        AssertEventHooks<SPFeature>(feature, hooks);
                    });
                });

                WithProvisionRunners(runner => runner.DeployWebModel(webModel));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.SSOM")]
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
                                AssertEventHooks<SPFolder>(folder, hooks);
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
                                AssertEventHooks<SPFolder>(folder, hooks);
                            });
                    });
                });

                WithProvisionRunners(runner => runner.DeployWebModel(webModel));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.SSOM")]
        public override void CanRaiseEvents_ListDefinition()
        {
            WithEventHooks(hooks =>
            {
                var webModel = SPMeta2Model.NewWebModel(site =>
                {
                    site.AddList(RegLists.GenericList, list =>
                    {
                        AssertEventHooks<SPList>(list, hooks);
                    });
                });

                WithProvisionRunners(runner => runner.DeployWebModel(webModel));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.SSOM")]
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
                                AssertEventHooks<SPListItem>(item, hooks);
                            });
                    });
                });

                WithProvisionRunners(runner => runner.DeployWebModel(webModel));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.SSOM")]
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
                                        AssertEventHooks<SPListItem>(valueItem, hooks);
                                    });
                            });
                    });
                });

                WithProvisionRunners(runner => runner.DeployWebModel(webModel));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.SSOM")]
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
                                AssertEventHooks<SPView>(view, hooks);
                            });
                    });
                });

                WithProvisionRunners(runner => runner.DeployWebModel(webModel));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.SSOM")]
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
                                AssertEventHooks<SPFile>(moduleFile, hooks);
                            });
                    });
                });

                WithProvisionRunners(runner => runner.DeployWebModel(webModel));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.SSOM")]
        [ExpectedException(typeof(NotImplementedException))]
        public override void CanRaiseEvents_PropertyDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.SSOM")]
        [ExpectedException(typeof(NotImplementedException))]
        public override void CanRaiseEvents_PublishingPageDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.SSOM")]
        public override void CanRaiseEvents_QuickLunchNavigationNodeDefinition()
        {
            WithEventHooks(hooks =>
            {
                var model = SPMeta2Model.NewWebModel(web =>
                {
                    web
                        .AddQuickLaunchNavigationNode(RegQuickLaunchNavigation.GoogleLink, link =>
                        {
                            AssertEventHooks<SPNavigationNode>(link, hooks);
                        });
                });

                WithProvisionRunners(runner => runner.DeployWebModel(model));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.SSOM")]
        public override void CanRaiseEvents_SecurityGroupDefinition()
        {
            WithEventHooks(hooks =>
            {
                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddSecurityGroup(RegSecurityGroups.SecurityGroup1, securityGroup =>
                    {
                        AssertEventHooks<SPGroup>(securityGroup, hooks);
                    });
                });

                WithProvisionRunners(runner => runner.DeploySiteModel(model));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.SSOM")]
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

                                AssertEventHooks<SPRoleAssignment>(securityGroupLink, hooks);
                            });
                    });
                });

                WithProvisionRunners(runner => runner.DeployWebModel(webModel));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.SSOM")]
        public override void CanRaiseEvents_SecurityRoleDefinition()
        {
            WithEventHooks(hooks =>
            {
                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddSecurityRole(RegSecurityRoles.SecurityRole1, securityRole =>
                    {
                        AssertEventHooks<SPRoleDefinition>(securityRole, hooks);
                    });
                });

                WithProvisionRunners(runner => runner.DeploySiteModel(model));
            });

        }

        [TestMethod]
        [TestCategory("Regression.Events.SSOM")]
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
                                        AssertEventHooks<SPRoleDefinition>(securityRoleLink, hooks);
                                    });
                            });
                    });
                });

                WithProvisionRunners(runner => runner.DeployWebModel(webModel));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.SSOM")]
        [ExpectedException(typeof(NotImplementedException))]
        public override void CanRaiseEvents_SiteDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.SSOM")]
        public override void CanRaiseEvents_SP2013WorkflowDefinition()
        {
            WithEventHooks(hooks =>
            {
                var model = SPMeta2Model.NewWebModel(web =>
                 {
                     web.AddSP2013Workflow(RegSP2013Workflows.WriteToHistoryList, workflow =>
                     {
                         AssertEventHooks<WorkflowDefinition>(workflow, hooks);
                     });
                 });

                WithProvisionRunners(runner => runner.DeployWebModel(model));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.SSOM")]
        public override void CanRaiseEvents_SP2013WorkflowSubscriptionDefinition()
        {
            WithEventHooks(hooks =>
            {
                // ensure workflow and list
                var model = SPMeta2Model.NewWebModel(web =>
                {
                    web
                        .AddSP2013Workflow(RegSP2013Workflows.WriteToHistoryList)
                        .AddList(RegLists.GenericList, list =>
                        {
                            list.AddSP2013WorkflowSubscription(RegSP2013WorkflowSubscriptions.WriteToHistoryList,
                                workflowSubscription =>
                                {
                                    AssertEventHooks<WorkflowSubscription>(workflowSubscription, hooks);
                                });
                        });
                });

                WithProvisionRunners(runner => runner.DeployWebModel(model));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.SSOM")]
        public override void CanRaiseEvents_UserCustomActionDefinition()
        {
            WithEventHooks(hooks =>
            {
                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddUserCustomAction(RegUserCustomActions.jQueryScript, customAction =>
                    {
                        AssertEventHooks<SPUserCustomAction>(customAction, hooks);
                    });
                });

                WithProvisionRunners(runner => runner.DeploySiteModel(model));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.SSOM")]
        public override void CanRaiseEvents_WebDefinition()
        {
            WithEventHooks(hooks =>
            {
                var model = SPMeta2Model.NewWebModel(parentWeb =>
                {
                    parentWeb.AddWeb(RegWebs.BlankWeb, web =>
                    {
                        AssertEventHooks<SPWeb>(web, hooks);
                    });
                });

                WithProvisionRunners(runner => runner.DeployWebModel(model));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.SSOM")]
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
                                            AssertEventHooks<System.Web.UI.WebControls.WebParts.WebPart>(webpart, hooks);
                                        });
                                });
                        });
                });

                WithProvisionRunners(runner => runner.DeployWebModel(model));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.SSOM")]
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
                                    AssertEventHooks<SPFile>(page, hooks);
                                });
                        });
                });

                WithProvisionRunners(runner => runner.DeployWebModel(model));
            });
        }

        [TestMethod]
        [TestCategory("Regression.Events.SSOM")]
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
                                    AssertEventHooks<SPFile>(page, hooks);
                                });
                        });
                });

                WithProvisionRunners(runner => runner.DeployWebModel(model));
            });
        }
    }
}
