using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Containers.Services;
using SPMeta2.Containers.Services.Rnd;
using SPMeta2.Containers.Standard.DefinitionGenerators;

namespace SPMeta2.Regression.Impl.Tests.ModelHandlers.Base
{
    public class ModelHandlerTestBase
    {
        #region constructors

        public ModelHandlerTestBase()
        {
            Rnd = new DefaultRandomService();
        }

        #endregion

        #region static

        static ModelHandlerTestBase()
        {
            RegressionService = new RegressionTestService();
            RegressionService.ModelGeneratorService.RegisterDefinitionGenerators(typeof(ImageRenditionDefinitionGenerator).Assembly);
        }

        #endregion

        #region properties

        public ModelGeneratorService ModelGeneratorService
        {
            get { return RegressionService.ModelGeneratorService; }
        }

        public static RegressionTestService RegressionService { get; set; }

        public RandomService Rnd { get; set; }

        #endregion
    }
}
