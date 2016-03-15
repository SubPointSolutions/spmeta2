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

        #endregion
    }
}
