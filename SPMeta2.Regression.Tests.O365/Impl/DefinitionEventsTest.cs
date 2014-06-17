using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Regression.Model.Definitions;
using SPMeta2.Regression.Tests.Impl.Events;
using SPMeta2.Syntax.Default;

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
                var siteModel = SPMeta2Model.NewWebModel(site =>
                {
                    site.AddFeature(RegSiteFeatures.PublishingSite, feature =>
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
                    web.AddFeature(RegWebFeatures.PublishingWeb, feature =>
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_ListItemFieldValueDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_ListViewDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_ModuleFileDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_PropertyDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_PublishingPageDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_QuickLunchNavigationNodeDefinition()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_WebPartPageDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_WikiPageDefinition()
        {
            throw new NotImplementedException();
        }
    }
}
