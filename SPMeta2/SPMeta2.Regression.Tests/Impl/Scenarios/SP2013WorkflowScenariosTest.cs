using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Containers.Standard;

using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Regression.Tests.Definitions;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Standard.Definitions;
using SPMeta2.Syntax.Default;
using SPMeta2.Validation.Validators.Relationships;

using SPMeta2.Regression.Tests.Extensions;

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
            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    AddWebWorkflow(web);
                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.SP2013Workflow")]
        public void CanDeploy_SP2013ListWorkflowAccosiation()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                AddListWorkflow(web);
            });

            TestModel(model);
        }

        #endregion

        #region list workflow on sub webs

        [TestMethod]
        [TestCategory("Regression.Scenarios.SP2013Workflow")]
        public void CanDeploy_SP2013ListWorkflowAccosiation_OnHierarchicalWebs()
        {
            var level1Web = ModelGeneratorService.GetRandomDefinition<WebDefinition>(w =>
            {
                w.Url = string.Format("l1{0}", Rnd.String());
            });

            var level2Web = ModelGeneratorService.GetRandomDefinition<WebDefinition>(w =>
            {
                w.Url = string.Format("l2{0}", Rnd.String());
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddWeb(level1Web, l1w =>
                        {
                            AddListWorkflow(l1w);

                            l1w.AddWeb(level2Web, l2web =>
                            {
                                AddListWorkflow(l2web);
                            });

                        });

                    AddListWorkflow(web);
                });

            TestModel(model);
        }

        #endregion

        #region web workflow on sub webs

        [TestMethod]
        [TestCategory("Regression.Scenarios.SP2013Workflow")]
        public void CanDeploy_SP2013WebWorkflowAccosiation_OnHierarchicalWebs()
        {
            var level1Web = ModelGeneratorService.GetRandomDefinition<WebDefinition>(w =>
            {
                w.Url = string.Format("l1{0}", Rnd.String());
            });

            var level2Web = ModelGeneratorService.GetRandomDefinition<WebDefinition>(w =>
            {
                w.Url = string.Format("l2{0}", Rnd.String());
            });

            var model = SPMeta2Model
               .NewWebModel(web =>
               {
                   web
                      .AddWeb(level1Web, l1w =>
                      {
                          AddWebWorkflow(l1w);

                          l1w.AddWeb(level2Web, l2web =>
                          {
                              AddWebWorkflow(l2web);
                          });
                      });

                   AddWebWorkflow(web);
               });

            TestModel(model);

        }

        #endregion

        #region utils

        protected ListDefinition GetTaskList()
        {
            return new ListDefinition
            {
                Title = Rnd.String(),
                TemplateType = BuiltInListTemplateTypeId.Tasks,
#pragma warning disable 618
                Url = Rnd.String()
#pragma warning restore 618
            };
        }

        protected ListDefinition GetHistoryList()
        {
            return new ListDefinition
            {
                Title = Rnd.String(),
                TemplateType = BuiltInListTemplateTypeId.WorkflowHistory,
#pragma warning disable 618
                Url = Rnd.String()
#pragma warning restore 618
            };
        }

        protected void AddWebWorkflow(WebModelNode web)
        {
            var workflow = ModelGeneratorService.GetRandomDefinition<SP2013WorkflowDefinition>();

            var historyList = GetHistoryList();
            var taskList = GetTaskList();

            web
                .AddList(historyList)
                .AddList(taskList)
                .AddSP2013Workflow(workflow)
                .AddSP2013WorkflowSubscription(new SP2013WorkflowSubscriptionDefinition
                {
                    Name = Rnd.String(),
                    WorkflowDisplayName = workflow.DisplayName,
#pragma warning disable 618
                    HistoryListUrl = historyList.GetListUrl(),
                    TaskListUrl = taskList.GetListUrl()
#pragma warning restore 618
                });
        }

        protected void AddListWorkflow(WebModelNode web)
        {
            var workflow = ModelGeneratorService.GetRandomDefinition<SP2013WorkflowDefinition>();
            var workflowEnableList = ModelGeneratorService.GetRandomDefinition<ListDefinition>();

            var historyList = GetHistoryList();
            var taskList = GetTaskList();

            web

                        .AddList(historyList)
                        .AddList(taskList)
                        .AddList(workflowEnableList, list =>
                        {
                            list.AddSP2013WorkflowSubscription(new SP2013WorkflowSubscriptionDefinition
                            {
                                Name = Rnd.String(),
                                WorkflowDisplayName = workflow.DisplayName,
#pragma warning disable 618
                                HistoryListUrl = historyList.GetListUrl(),
                                TaskListUrl = taskList.GetListUrl()
#pragma warning restore 618
                            });
                        })
                        .AddSP2013Workflow(workflow);
        }

        #endregion
    }

}
