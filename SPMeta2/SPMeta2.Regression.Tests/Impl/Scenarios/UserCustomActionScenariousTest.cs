using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Containers.Standard;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Syntax.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class UserCustomActionScenariousTest : SPMeta2RegresionScenarioTestBase
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
        [TestCategory("Regression.Scenarios.UserCustomAction.Scopes")]
        public void CanDeploy_UserCustomAction_ForSite()
        {
            var model = SPMeta2Model
               .NewSiteModel(site =>
               {
                   site.AddRandomUserCustomAction();
               });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.UserCustomAction.Scopes")]
        public void CanDeploy_UserCustomAction_ForWeb()
        {
            var model = SPMeta2Model
               .NewWebModel(web =>
               {
                   web.AddRandomUserCustomAction();
               });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.UserCustomAction.Scopes")]
        public void CanDeploy_UserCustomAction_ForList()
        {
            var customAction = ModelGeneratorService.GetRandomDefinition<UserCustomActionDefinition>();

            // should be for !ScriptLink for list scope
            customAction.Location = BuiltInCustomActionLocationId.EditControlBlock.Location;
            
            customAction.ScriptBlock = null;
            customAction.ScriptSrc = null;
            
            customAction.RegistrationType = BuiltInRegistrationTypes.List;

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddRandomList(list =>
                    {
                        list.AddUserCustomAction(customAction);
                    });
                });

            TestModel(model);
        }


        #endregion
    }
}
