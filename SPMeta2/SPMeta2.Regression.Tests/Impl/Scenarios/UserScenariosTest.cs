using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers.Services;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Definitions;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Syntax.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Containers.Services;
using SPMeta2.Containers;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class UserScenariosTest : SPMeta2RegresionScenarioTestBase
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

        #region default

        [TestMethod]
        [TestCategory("Regression.Scenarios.User")]
        public void CanDeploy_User_UnderGroups_By_LoginName()
        {
            var userDef = ModelGeneratorService.GetRandomDefinition<UserDefinition>(def =>
            {
                def.LoginName = Rnd.UserLogin();
                def.Email = string.Empty;
            });

            var groupDef = ModelGeneratorService.GetRandomDefinition<SecurityGroupDefinition>(def =>
            {

            });

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSecurityGroup(groupDef, group =>
                {
                    group.AddUser(userDef);
                });
            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.User")]
        public void CanDeploy_User_UnderGroups_By_Email()
        {
            var userDef = ModelGeneratorService.GetRandomDefinition<UserDefinition>(def =>
            {
                def.LoginName = string.Empty;
                def.Email = Rnd.DomainUserEmail();
            });

            var groupDef = ModelGeneratorService.GetRandomDefinition<SecurityGroupDefinition>(def =>
            {

            });

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSecurityGroup(groupDef, group =>
                {
                    group.AddUser(userDef);
                });
            });

            TestModel(model);
        }


        #endregion
    }
}
