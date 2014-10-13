using SPMeta2.Containers.Services;
using SPMeta2.Containers.Services.Rnd;
using SPMeta2.Regression.Tests.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.Tests.Impl.Scenarios.Base
{
    public class SPMeta2RegresionScenarioTestBase : SPMeta2RegresionEventsTestBase
    {
        #region constructors

        public SPMeta2RegresionScenarioTestBase()
        {
            ProvisionGenerationCount = 2;
            Rnd = new DefaultRandomService();
        }

        #endregion

        protected RandomService Rnd { get; set; }
    }
}
