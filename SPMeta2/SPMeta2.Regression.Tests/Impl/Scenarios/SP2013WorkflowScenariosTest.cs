using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Containers.Standard;
using SPMeta2.CSOM.DefaultSyntax;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Tests.Definitions;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Standard.Definitions;
using SPMeta2.Syntax.Default;
using SPMeta2.Validation.Validators.Relationships;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class SP2013WorkflowScenariosTest : SPMeta2RegresionScenarioTestBase
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
        [TestCategory("Regression.Scenarios.SP2013Workflow")]
        public void CanDeploy_SP2013WebWorkflowAccosiation()
        {
            var workflow = ModelGeneratorService.GetRandomDefinition<SP2013WorkflowDefinition>();

            var historyList = new ListDefinition
            {
                Title = Rnd.String(),
                TemplateType = BuiltInListTemplateTypeId.WorkflowHistory,
                Url = Rnd.String()
            };

            var taskList = new ListDefinition
            {
                Title = Rnd.String(),
                TemplateType = BuiltInListTemplateTypeId.Tasks,
                Url = Rnd.String()
            };

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddSP2013Workflow(workflow)
                        .AddList(historyList)
                        .AddList(taskList)
                        .AddSP2013WorkflowSubscription(new SP2013WorkflowSubscriptionDefinition
                        {
                            Name = Rnd.String(),
                            WorkflowDisplayName = workflow.DisplayName,
                            HistoryListUrl = historyList.GetListUrl(),
                            TaskListUrl = taskList.GetListUrl()
                        });

                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.SP2013Workflow")]
        public void CanDeploy_SP2013ListWorkflowAccosiation()
        {
            var workflow = ModelGeneratorService.GetRandomDefinition<SP2013WorkflowDefinition>();

            var historyList = new ListDefinition
            {
                Title = Rnd.String(),
                TemplateType = BuiltInListTemplateTypeId.WorkflowHistory,
                Url = Rnd.String()
            };

            var taskList = new ListDefinition
            {
                Title = Rnd.String(),
                TemplateType = BuiltInListTemplateTypeId.Tasks,
                Url = Rnd.String()
            };

            var workflowEnableList = ModelGeneratorService.GetRandomDefinition<ListDefinition>();

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddSP2013Workflow(workflow)
                        .AddList(historyList)
                        .AddList(taskList)
                        .AddList(workflowEnableList, list =>
                        {
                            list.AddSP2013WorkflowSubscription(new SP2013WorkflowSubscriptionDefinition
                            {
                                Name = Rnd.String(),
                                WorkflowDisplayName = workflow.DisplayName,
                                HistoryListUrl = historyList.GetListUrl(),
                                TaskListUrl = taskList.GetListUrl()
                            });
                        });
                });

            TestModel(model);
        }

        #endregion
    }

}
