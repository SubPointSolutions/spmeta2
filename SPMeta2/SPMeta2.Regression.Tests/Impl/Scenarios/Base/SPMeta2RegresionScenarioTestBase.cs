using SPMeta2.Containers.Services;
using SPMeta2.Containers.Services.Rnd;
using SPMeta2.Regression.Tests.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Common;
using SPMeta2.Containers.Consts;
using SPMeta2.Containers.Utils;
using SPMeta2.Exceptions;
using SPMeta2.Regression.Tests.Impl.Services;
using SPMeta2.Regression.Utils;
using SPMeta2.Services;
using SPMeta2.Services.Impl;


namespace SPMeta2.Regression.Tests.Impl.Scenarios.Base
{
    public class SPMeta2RegresionScenarioTestBase : SPMeta2ProvisionRegresionTestBase
    {
        #region constructors

        public SPMeta2RegresionScenarioTestBase()
        {
            RegressionService.ProvisionGenerationCount = 2;
            RegressionService.ShowOnlyFalseResults = true;

            var isIncrementalProvisionEnabled = IsIncrementalProvisionMode;

            if (isIncrementalProvisionEnabled)
            {
                RegressionService.BeforeProvisionRunnerExcecution += (runner) =>
                {
                    var config = new IncrementalProvisionConfig();
                    runner.ProvisionService.SetIncrementalProvisionMode(config);

                    runner.OnBeforeDeployModel += (provisionService, model) =>
                    {
                        if (PreviousModelHash != null)
                            provisionService.SetIncrementalProvisionModelHash(PreviousModelHash);
                    };

                    runner.OnAfterDeployModel += (provisionService, model) =>
                    {
                        var tracer = new DefaultIncrementalModelPrettyPrintService();

                        RegressionUtils.WriteLine(string.Format("Deployed model with incremental updates:"));
                        RegressionUtils.WriteLine(Environment.NewLine + tracer.PrintModel(model));

                        PreviousModelHash = provisionService.GetIncrementalProvisionModelHash();
                    };
                };

                RegressionService.AfterProvisionRunnerExcecution += (runner) =>
                {

                };

                RegressionService.EnableEventValidation = false;
                RegressionService.EnableDefinitionValidation = false;
                RegressionService.EnablePropertyValidation = false;

                EnablePropertyUpdateValidation = true;
            }
        }

        #endregion

        #region properties

        public ModelHash PreviousModelHash { get; set; }

        #endregion
    }
}
