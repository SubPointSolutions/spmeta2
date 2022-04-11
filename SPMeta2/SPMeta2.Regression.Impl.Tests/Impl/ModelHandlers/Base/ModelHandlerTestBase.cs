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

        }

        #endregion

        #region properties

        private ModelGeneratorService _modelGeneratorService;

        public ModelGeneratorService ModelGeneratorService
        {
            get
            {
                if (_modelGeneratorService == null)
                {
                    _modelGeneratorService = new ModelGeneratorService();
                    _modelGeneratorService.RegisterDefinitionGenerators(typeof(ImageRenditionDefinitionGenerator).Assembly);
                }

                return _modelGeneratorService;
            }
        }

        private static RegressionTestService _regressionService;

        public static RegressionTestService RegressionService
        {
            get
            {
                if (_regressionService == null)
                {
                    _regressionService = new RegressionTestService();
                }

                return _regressionService;
            }
        }

        public RandomService Rnd { get; set; }

        #endregion
    }
}
