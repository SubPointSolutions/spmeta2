using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Containers.Standard;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Definitions;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Syntax.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Syntax.Default.Modern;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class SecurityScenariosTest : SPMeta2RegresionScenarioTestBase
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

        #region break role inheritance

        [TestMethod]
        [TestCategory("Regression.Scenarios.Security")]
        public void CanDeploy_BreakRoleInheritance_OnWeb()
        {
            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddRandomWeb(rndWeb =>
                    {
                        rndWeb.OnProvisioning<object>(context =>
                        {
                            TurnOffValidation(rndWeb);
                        });

                        rndWeb.AddBreakRoleInheritance(GetCleanInheritance());
                    });
                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Security")]
        public void CanDeploy_BreakRoleInheritance_OnList()
        {
            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddRandomWeb(rndWeb =>
                    {
                        rndWeb.AddRandomList(rndList =>
                        {
                            rndList.OnProvisioning<object>(context =>
                            {
                                TurnOffValidation(rndList);
                            });

                            rndList.AddBreakRoleInheritance(GetCleanInheritance());
                        });
                    });
                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Security")]
        public void CanDeploy_BreakRoleInheritance_OnListFolder()
        {
            var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddRandomWeb(rndWeb =>
                    {
                        rndWeb.AddList(listDef, rndList =>
                        {
                            rndList.AddRandomFolder(rndFolder =>
                            {
                                rndFolder.OnProvisioning<object>(context =>
                                {
                                    TurnOffValidation(rndFolder);
                                });

                                rndFolder.AddBreakRoleInheritance(GetCleanInheritance());
                            });
                        });
                    });
                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Security")]
        public void CanDeploy_BreakRoleInheritance_OnLibraryFolder()
        {
            var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddRandomWeb(rndWeb =>
                    {
                        rndWeb.AddList(listDef, rndList =>
                        {
                            rndList.AddRandomFolder(rndFolder =>
                            {
                                rndFolder.OnProvisioning<object>(context =>
                                {
                                    TurnOffValidation(rndFolder);
                                });

                                rndFolder.AddBreakRoleInheritance(GetCleanInheritance());
                            });
                        });
                    });
                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Security")]
        public void CanDeploy_BreakRoleInheritance_OnListItem()
        {
            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddRandomWeb(rndWeb =>
                    {
                        rndWeb.AddRandomList(rndList =>
                        {
                            rndList.AddRandomListItem(rndListItem =>
                            {
                                rndListItem.OnProvisioning<object>(context =>
                                {
                                    TurnOffValidation(rndListItem);
                                });

                                rndListItem.AddBreakRoleInheritance(GetCleanInheritance());
                            });
                        });
                    });
                });

            TestModel(model);
        }

        #endregion

        #region add security group link

        [TestMethod]
        [TestCategory("Regression.Scenarios.Security")]
        public void CanDeploy_SecurityGroupLink_OnWeb()
        {
            var securityGroup = ModelGeneratorService.GetRandomDefinition<SecurityGroupDefinition>();

            var siteModel = SPMeta2Model
                                .NewSiteModel(site =>
                                {
                                    site.AddSecurityGroup(securityGroup);
                                });

            var webModel = SPMeta2Model
               .NewWebModel(web =>
               {
                   web.AddRandomWeb(rndWeb =>
                   {
                       rndWeb.OnProvisioning<object>(context =>
                       {
                           TurnOffValidation(rndWeb);
                       });

                       rndWeb.AddSecurityGroupLink(securityGroup);
                   });
               });

            TestModels(new[] { siteModel, webModel });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Security")]
        public void CanDeploy_SecurityGroupLink_OnList()
        {
            var securityGroup = ModelGeneratorService.GetRandomDefinition<SecurityGroupDefinition>();

            var siteModel = SPMeta2Model
                                .NewSiteModel(site =>
                                {
                                    site.AddSecurityGroup(securityGroup);
                                });

            var webModel = SPMeta2Model
               .NewWebModel(web =>
               {
                   web.AddRandomWeb(rndWeb =>
                   {
                       rndWeb.AddRandomList(rndList =>
                       {
                           rndList.OnProvisioning<object>(context =>
                           {
                               TurnOffValidation(rndList);
                           });

                           rndList.AddSecurityGroupLink(securityGroup);
                       });
                   });
               });

            TestModels(new[] { siteModel, webModel });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Security")]
        public void CanDeploy_SecurityGroupLink_OnListFolder()
        {
            var securityGroup = ModelGeneratorService.GetRandomDefinition<SecurityGroupDefinition>();

            var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });

            var siteModel = SPMeta2Model
                                .NewSiteModel(site =>
                                {
                                    site.AddSecurityGroup(securityGroup);
                                });

            var webModel = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddRandomWeb(rndWeb =>
                    {
                        rndWeb.AddList(listDef, rndList =>
                        {
                            rndList.AddRandomFolder(rndFolder =>
                            {
                                rndFolder.OnProvisioning<object>(context =>
                                {
                                    TurnOffValidation(rndFolder);
                                });

                                rndFolder.AddSecurityGroupLink(securityGroup);
                            });
                        });
                    });
                });

            TestModels(new[] { siteModel, webModel });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Security")]
        public void CanDeploy_SecurityGroupLink_OnLibraryFolder()
        {
            var securityGroup = ModelGeneratorService.GetRandomDefinition<SecurityGroupDefinition>();

            var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
            });

            var siteModel = SPMeta2Model
                                .NewSiteModel(site =>
                                {
                                    site.AddSecurityGroup(securityGroup);
                                });

            var webModel = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddRandomWeb(rndWeb =>
                    {
                        rndWeb.AddList(listDef, rndList =>
                        {
                            rndList.AddRandomFolder(rndFolder =>
                            {
                                rndFolder.OnProvisioning<object>(context =>
                                {
                                    TurnOffValidation(rndFolder);
                                });

                                rndFolder.AddSecurityGroupLink(securityGroup);
                            });
                        });
                    });
                });

            TestModels(new[] { siteModel, webModel });

        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.Security")]
        public void CanDeploy_SecurityGroupLink_OnListItem()
        {
            var securityGroup = ModelGeneratorService.GetRandomDefinition<SecurityGroupDefinition>();

            var siteModel = SPMeta2Model
                                .NewSiteModel(site =>
                                {
                                    site.AddSecurityGroup(securityGroup);
                                });

            var webModel = SPMeta2Model
                 .NewWebModel(web =>
                 {
                     web.AddRandomWeb(rndWeb =>
                     {
                         rndWeb.AddRandomList(rndList =>
                         {
                             rndList.AddRandomListItem(rndListItem =>
                             {
                                 rndListItem.OnProvisioning<object>(context =>
                                 {
                                     TurnOffValidation(rndListItem);
                                 });

                                 rndListItem.AddSecurityGroupLink(securityGroup);
                             });
                         });
                     });
                 });


            TestModels(new[] { siteModel, webModel });
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.Security.Pages")]
        public void CanDeploy_SecurityGroupLink_OnWikiPage()
        {
            var securityGroup = ModelGeneratorService.GetRandomDefinition<SecurityGroupDefinition>();

            var siteModel = SPMeta2Model
                                .NewSiteModel(site =>
                                {
                                    site.AddSecurityGroup(securityGroup);
                                });

            var webModel = SPMeta2Model
                 .NewWebModel(web =>
                 {
                     web.AddRandomWeb(rndWeb =>
                     {
                         rndWeb.AddHostList(BuiltInListDefinitions.SitePages, sitePages =>
                         {
                             sitePages.AddRandomWikiPage(page =>
                             {
                                 page.OnProvisioning<object>(context =>
                                 {
                                     TurnOffValidation(page);
                                 });

                                 page.AddSecurityGroupLink(securityGroup);
                             });
                         });
                     });
                 });


            TestModels(new[] { siteModel, webModel });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Security.Pages")]
        public void CanDeploy_SecurityGroupLink_OnWebPartPage()
        {
            var securityGroup = ModelGeneratorService.GetRandomDefinition<SecurityGroupDefinition>();

            var siteModel = SPMeta2Model
                                .NewSiteModel(site =>
                                {
                                    site.AddSecurityGroup(securityGroup);
                                });

            var webModel = SPMeta2Model
                 .NewWebModel(web =>
                 {
                     web.AddRandomWeb(rndWeb =>
                     {
                         rndWeb.AddHostList(BuiltInListDefinitions.SitePages, sitePages =>
                         {
                             sitePages.AddRandomWebPartPage(page =>
                             {
                                 page.OnProvisioning<object>(context =>
                                 {
                                     TurnOffValidation(page);
                                 });

                                 page.AddSecurityGroupLink(securityGroup);
                             });
                         });
                     });
                 });


            TestModels(new[] { siteModel, webModel });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Security.Pages")]
        public void CanDeploy_SecurityGroupLink_OnPublishingPage()
        {
            var securityGroup = ModelGeneratorService.GetRandomDefinition<SecurityGroupDefinition>();

            var siteModel = SPMeta2Model
                                .NewSiteModel(site =>
                                {
                                    site.AddSecurityGroup(securityGroup);
                                    site.AddSiteFeature(BuiltInSiteFeatures.SharePointServerPublishingInfrastructure.Inherit(f =>
                                    {
                                        f.Enable = true;
                                    }));
                                });

            var webModel = SPMeta2Model
                 .NewWebModel(web =>
                 {
                     web.AddRandomWeb(rndWeb =>
                     {
                         rndWeb.AddSiteFeature(BuiltInWebFeatures.SharePointServerPublishing.Inherit(f =>
                         {
                             f.Enable = true;
                         }));

                         rndWeb.AddHostList(BuiltInListDefinitions.Pages, sitePages =>
                         {
                             sitePages.AddRandomPublishingPage(page =>
                             {
                                 page.OnProvisioning<object>(context =>
                                 {
                                     TurnOffValidation(page);
                                 });

                                 page.AddSecurityGroupLink(securityGroup);
                             });
                         });
                     });
                 });


            TestModels(new[] { siteModel, webModel });
        }

        #endregion

        #region role links options

        [TestMethod]
        [TestCategory("Regression.Scenarios.Security.RoleLinks")]
        public void CanDeploy_SecurityRoleLink_ByName()
        {
            var securityGroup = ModelGeneratorService.GetRandomDefinition<SecurityGroupDefinition>();
            var securityRole = ModelGeneratorService.GetRandomDefinition<SecurityRoleDefinition>();

            var siteModel = SPMeta2Model
                                .NewSiteModel(site =>
                                {
                                    site
                                        .AddSecurityGroup(securityGroup)
                                        .AddSecurityRole(securityRole);
                                });

            var webModel = SPMeta2Model
                 .NewWebModel(web =>
                 {
                     web.AddRandomWeb(rndWeb =>
                     {
                         rndWeb.AddRandomList(rndList =>
                         {
                             // rndList.AddRandomListItem(rndListItem =>
                             // {
                             rndList.AddBreakRoleInheritance(new BreakRoleInheritanceDefinition(),
                                 secureListItem =>
                                 {
                                     secureListItem.AddSecurityGroupLink(securityGroup, group =>
                                     {
                                         group.AddSecurityRoleLink(new SecurityRoleLinkDefinition
                                         {
                                             SecurityRoleName = securityRole.Name
                                         });
                                     });

                                 });


                             // });
                         });
                     });
                 });


            TestModels(new[] { siteModel, webModel });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Security.RoleLinks")]
        public void CanDeploy_SecurityRoleLink_BySecurityRoleType()
        {
            var securityGroup = ModelGeneratorService.GetRandomDefinition<SecurityGroupDefinition>();

            var siteModel = SPMeta2Model
                                .NewSiteModel(site =>
                                {
                                    site
                                        .AddSecurityGroup(securityGroup);
                                });

            var webModel = SPMeta2Model
                 .NewWebModel(web =>
                 {
                     web.AddRandomWeb(rndWeb =>
                     {
                         rndWeb.AddRandomList(rndList =>
                         {
                             rndList.AddRandomListItem(rndListItem =>
                             {
                                 rndListItem.AddSecurityGroupLink(securityGroup, group =>
                                 {
                                     group.AddSecurityRoleLink(new SecurityRoleLinkDefinition
                                     {
                                         SecurityRoleType = BuiltInSecurityRoleTypes.Editor
                                     });
                                 });
                             });
                         });
                     });
                 });


            TestModels(new[] { siteModel, webModel });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Security.RoleLinks")]
        public void CanDeploy_SecurityRoleLink_BySecurityRoleId()
        {
            var securityGroup = ModelGeneratorService.GetRandomDefinition<SecurityGroupDefinition>();

            var siteModel = SPMeta2Model
                                .NewSiteModel(site =>
                                {
                                    site
                                        .AddSecurityGroup(securityGroup);
                                });

            var webModel = SPMeta2Model
                 .NewWebModel(web =>
                 {
                     web.AddRandomWeb(rndWeb =>
                     {
                         rndWeb.AddRandomList(rndList =>
                         {
                             rndList.AddRandomListItem(rndListItem =>
                             {
                                 rndListItem.AddSecurityGroupLink(securityGroup, group =>
                                 {
                                     group.AddSecurityRoleLink(new SecurityRoleLinkDefinition
                                     {
                                         // bits of magic
                                         // should be 'Full Control' role
                                         SecurityRoleId = 1073741829
                                     });
                                 });
                             });
                         });
                     });
                 });


            TestModels(new[] { siteModel, webModel });
        }


        #endregion

        #region group link options

        [TestMethod]
        [TestCategory("Regression.Scenarios.Security.GroupLinks")]
        public void CanDeploy_SecurityGroupLink_ByName()
        {
            var securityGroup = ModelGeneratorService.GetRandomDefinition<SecurityGroupDefinition>();

            var siteModel = SPMeta2Model
                                .NewSiteModel(site =>
                                {
                                    site.AddSecurityGroup(securityGroup);
                                });

            var webModel = SPMeta2Model
                 .NewWebModel(web =>
                 {
                     web.AddRandomWeb(rndWeb =>
                     {
                         rndWeb.AddRandomList(rndList =>
                         {
                             rndList.AddRandomListItem(rndListItem =>
                             {
                                 rndListItem.AddSecurityGroupLink(new SecurityGroupLinkDefinition
                                 {
                                     SecurityGroupName = securityGroup.Name
                                 });
                             });
                         });
                     });
                 });


            TestModels(new[] { siteModel, webModel });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Security.GroupLinks")]
        public void CanDeploy_SecurityGroupLink_ByIsAssociatedMemberGroup()
        {
            var securityGroup = ModelGeneratorService.GetRandomDefinition<SecurityGroupDefinition>();

            var webModel = SPMeta2Model
                 .NewWebModel(web =>
                 {
                     web.AddRandomWeb(rndWeb =>
                     {
                         rndWeb.AddRandomList(rndList =>
                         {
                             rndList.AddRandomListItem(rndListItem =>
                             {
                                 rndListItem.AddSecurityGroupLink(new SecurityGroupLinkDefinition
                                 {
                                     IsAssociatedMemberGroup = true
                                 });
                             });
                         });
                     });
                 });


            TestModels(new[] { webModel });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Security.GroupLinks")]
        public void CanDeploy_SecurityGroupLink_ByIsAssociatedOwnerGroup()
        {
            var webModel = SPMeta2Model
                 .NewWebModel(web =>
                 {
                     web.AddRandomWeb(rndWeb =>
                     {
                         rndWeb.AddRandomList(rndList =>
                         {
                             rndList.AddRandomListItem(rndListItem =>
                             {
                                 rndListItem.AddSecurityGroupLink(new SecurityGroupLinkDefinition
                                 {
                                     IsAssociatedOwnerGroup = true
                                 });
                             });
                         });
                     });
                 });

            TestModels(new[] { webModel });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Security.GroupLinks")]
        public void CanDeploy_SecurityGroupLink_ByIsAssociatedVisitorGroup()
        {
            var webModel = SPMeta2Model
                 .NewWebModel(web =>
                 {
                     web.AddRandomWeb(rndWeb =>
                     {
                         rndWeb.AddRandomList(rndList =>
                         {
                             rndList.AddRandomListItem(rndListItem =>
                             {
                                 rndListItem.AddSecurityGroupLink(new SecurityGroupLinkDefinition
                                 {
                                     IsAssociatedVisitorGroup = true
                                 });
                             });
                         });
                     });
                 });

            TestModels(new[] { webModel });
        }

        #endregion

        #region utils

        protected void TurnOffValidation(ModelNode node)
        {
            node.Value.RequireSelfProcessing = false;
            node.Options.RequireSelfProcessing = false;
        }

        protected BreakRoleInheritanceDefinition GetCleanInheritance()
        {
            return new BreakRoleInheritanceDefinition
            {
                ClearSubscopes = true,
                CopyRoleAssignments = false,
                ForceClearSubscopes = true
            };
        }

        #endregion
    }
}
