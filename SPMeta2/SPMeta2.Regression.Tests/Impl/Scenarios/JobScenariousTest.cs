using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class JobScenariousTest : SPMeta2RegresionScenarioTestBase
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
                    ConstructorParams = new Collection<JobDefinitionCtorParams>()
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
