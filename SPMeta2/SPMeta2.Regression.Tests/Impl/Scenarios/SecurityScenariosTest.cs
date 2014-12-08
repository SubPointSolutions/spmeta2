using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
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
