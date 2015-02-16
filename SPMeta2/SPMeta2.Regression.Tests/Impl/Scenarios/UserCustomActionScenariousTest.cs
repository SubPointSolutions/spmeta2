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

        #region command ui extension

        [TestMethod]
        [TestCategory("Regression.Scenarios.UserCustomAction.Properties")]
        public void CanDeploy_UserCustomAction_WithCommandUIExtension_ForList()
        {


            var customAction = ModelGeneratorService.GetRandomDefinition<UserCustomActionDefinition>(def =>
            {
                def.Location = "CommandUI.Ribbon.DisplayForm";

                def.ScriptBlock = null;
                def.ScriptSrc = null;

                def.CommandUIExtension = @"<CommandUIExtension xmlns=""http://schemas.microsoft.com/sharepoint/"">
                    <CommandUIDefinitions>
                        <CommandUIDefinition Location=""Ribbon.ListForm.Display.Manage.Controls._children"">
                            <Button Id=""DiaryAction.Button"" TemplateAlias=""o1"" Command=""DiaryCommand"" CommandType=""General"" LabelText=""Add Diary Entry"" Image32by32=""/_layouts/15/images/DateRangeLast1Day_32x32.png"" />
                        </CommandUIDefinition>
                    </CommandUIDefinitions>
                    <CommandUIHandlers>
                        <CommandUIHandler Command =""DiaryCommand"" CommandAction=""javascript:alert('{SelectedItemId}');"" EnabledScript=""javascript:SP.ListOperation.Selection.getSelectedItems().length == 1;"" />
                    </CommandUIHandlers>
                </CommandUIExtension>";

                def.RegistrationType = BuiltInRegistrationTypes.List;
            });

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

        #region scopes

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
