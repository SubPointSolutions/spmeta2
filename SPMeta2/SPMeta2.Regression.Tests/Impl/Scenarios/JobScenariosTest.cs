using System;
using System.Collections.ObjectModel;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SPMeta2.Definitions;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class JobScenariosTest : SPMeta2RegresionScenarioTestBase
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

        #region test

        [TestMethod]
        [TestCategory("Regression.Scenarios.Job")]
        public void CanDeploy_JobDefinition_WithProps()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {

                var jobDef = ModelGeneratorService.GetRandomDefinition<JobDefinition>(def =>
                {
                    def.Properties.Add(new JobDefinitionProperty
                    {
                        Key = string.Format("string_prop_{0}", Rnd.String()),
                        Value = string.Format("value_{0}", Rnd.String()),
                    });

                    def.Properties.Add(new JobDefinitionProperty
                    {
                        Key = string.Format("int_prop_{0}", Rnd.String()),
                        Value = Rnd.Int()
                    });

                    def.Properties.Add(new JobDefinitionProperty
                    {
                        Key = string.Format("double_prop_{0}", Rnd.String()),
                        Value = Math.Floor(Rnd.Double())
                    });
                });

                var model = SPMeta2Model.NewWebApplicationModel(webApp =>
                {
                    webApp.AddJob(jobDef);
                });

                TestModel(model);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Job.Scopes")]
        public void CanDeploy_Job_UnderWebApplication()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                // OOTB job with 2 parameters for the constructor
                var webAppJobDefinition = new JobDefinition
                {
                    Name = Rnd.String(),
                    Title = Rnd.String(),
                    ScheduleString = "yearly at jan 1 09:00:00",
                    JobType = "Microsoft.SharePoint.Administration.SPDeadSiteDeleteJobDefinition, Microsoft.SharePoint",
                    ConstructorParams = new Collection<JobDefinitionCtorParams>
                    {
                        JobDefinitionCtorParams.JobName,
                        JobDefinitionCtorParams.WebApplication
                    }
                };

                var model = SPMeta2Model
                    .NewWebApplicationModel(webApp =>
                    {
                        webApp.AddJob(webAppJobDefinition);
                    });

                TestModel(model);
            });
        }

        #endregion
    }
}
