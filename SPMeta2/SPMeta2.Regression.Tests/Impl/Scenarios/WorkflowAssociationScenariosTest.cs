using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers.DefinitionGenerators;
using SPMeta2.Containers.Services;
using SPMeta2.CSOM.DefaultSyntax;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Modern;
using SPMeta2.Syntax.Default.Utils;
using SPMeta2.Utils;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class WorkflowAssociationScenariosTest : SPMeta2RegresionScenarioTestBase
    {
        #region common

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

        //[TestMethod]
        //[TestCategory("Regression.Scenarios. WorkflowAssociation")]
        //public void CanDeploy_WorkflowAssociation_UnderSite()
        //{
        //    var workflowDef = ModelGeneratorService.GetRandomDefinition<WorkflowAssociationDefinition>(def =>
        //    {

        //    });

        //    var model = SPMeta2Model.NewSiteModel(site =>
        //    {
        //        site.AddWorkflowAssociation(workflowDef);
        //    });

        //    TestModel(model);
        //}

        [TestMethod]
        [TestCategory("Regression.Scenarios. WorkflowAssociation")]
        public void CanDeploy_WorkflowAssociation_UnderWeb()
        {
            var workflowDef = ModelGeneratorService.GetRandomDefinition<WorkflowAssociationDefinition>(def =>
            {

            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWorkflowAssociation(workflowDef);
            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios. WorkflowAssociation")]
        public void CanDeploy_WorkflowAssociation_UnderList()
        {
            var taskList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.Hidden = true;
                def.TemplateType = BuiltInListTemplateTypeId.Tasks;
            });

            var historyList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.Hidden = true;
                def.TemplateType = BuiltInListTemplateTypeId.WorkflowHistory;
            });

            var workflowDef = ModelGeneratorService.GetRandomDefinition<WorkflowAssociationDefinition>(def =>
            {
                def.TaskListTitle = taskList.Title;
                def.HistoryListTitle = historyList.Title;
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(taskList);
                web.AddList(historyList);

                web.AddRandomList(list =>
                {
                    list.AddWorkflowAssociation(workflowDef);
                });
            });

            TestModel(model);
        }
    }
}
