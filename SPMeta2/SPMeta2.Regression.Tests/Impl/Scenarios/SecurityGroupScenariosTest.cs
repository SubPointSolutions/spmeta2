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

using SPMeta2.Syntax.Default.Modern;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class SecurityGroupScenariosTest : SPMeta2RegresionScenarioTestBase
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

        #region add security group link

        [TestMethod]
        [TestCategory("Regression.Scenarios.SecurityGroup")]
        public void CanDeploy_SecurityGroup_Owner_AsUser()
        {
            var securityGroup = ModelGeneratorService.GetRandomDefinition<SecurityGroupDefinition>();

            var siteModel = SPMeta2Model
                                .NewSiteModel(site =>
                                {
                                    securityGroup.Owner = Rnd.UserLogin();
                                    site.AddSecurityGroup(securityGroup);
                                });

            TestModels(new[] { siteModel });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.SecurityGroup")]
        public void CanDeploy_SecurityGroup_Owner_AsSharePointGroup()
        {
            var ownerSecurityGroup = ModelGeneratorService.GetRandomDefinition<SecurityGroupDefinition>();
            var securityGroup = ModelGeneratorService.GetRandomDefinition<SecurityGroupDefinition>(def =>
            {
                def.Owner = ownerSecurityGroup.Name;
            });

            var siteModel = SPMeta2Model
                                .NewSiteModel(site =>
                                {
                                    site.AddSecurityGroup(ownerSecurityGroup);
                                    site.AddSecurityGroup(securityGroup);
                                });

            TestModels(new[] { siteModel });
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.SecurityGroup")]
        public void CanDeploy_SecurityGroup_Owner_AsSelfGroup()
        {
            var securityGroup = ModelGeneratorService.GetRandomDefinition<SecurityGroupDefinition>(def =>
            {
                def.Owner = def.Name;
            });

            var siteModel = SPMeta2Model
                                .NewSiteModel(site =>
                                {
                                    site.AddSecurityGroup(securityGroup);
                                });

            TestModels(new[] { siteModel });
        }

        #endregion
    }
}
