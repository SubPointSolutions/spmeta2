using SPMeta2.Containers.Services;
using SPMeta2.Containers.Services.Rnd;
using SPMeta2.Regression.Tests.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Exceptions;


namespace SPMeta2.Regression.Tests.Impl.Scenarios.Base
{
    public class SPMeta2RegresionScenarioTestBase : SPMeta2RegresionTestBase
    {
        #region constructors

        public SPMeta2RegresionScenarioTestBase()
        {
            RegressionService.ProvisionGenerationCount = 2;
            RegressionService.ShowOnlyFalseResults = false;

            Rnd = new DefaultRandomService();
        }

        #endregion

        protected RandomService Rnd { get; set; }

        protected bool IsCorrectValidationException(Exception e)
        {
            var result = true;

            result = result & (e is SPMeta2Exception);
            result = result & (e.InnerException is SPMeta2AggregateException);
            result = result & ((e.InnerException as AggregateException)
                                    .InnerExceptions.All(ee => ee is SPMeta2ModelValidationException));

            return result;
        }
    }
}
