using System;
using System.Linq;
using SPMeta2.ModelHandlers;
using SPMeta2.ModelHosts;
using SPMeta2.Models;
using SPMeta2.Services.Impl.Validation;
using SPMeta2.Exceptions;

namespace SPMeta2.Services
{
    public abstract class DefaultPreDeploymentTreeTraverseServiceBase<TDefinitionHandler>
        : PreDeploymentValidationServiceBase
          where TDefinitionHandler : ModelHandlerBase, new()
    {
        #region constructors

        public DefaultPreDeploymentTreeTraverseServiceBase()
        {
            ModelHandler = new TDefinitionHandler();
        }

        #endregion

        #region properties

        public TDefinitionHandler ModelHandler { get; set; }

        #endregion

        #region methods

        public override void DeployModel(ModelHostBase modelHost, ModelNode model)
        {
            Exceptions.Clear();

            //var defaultTraserveService = ServiceContainer.Instance.GetService<ModelTreeTraverseServiceBase>();

            //defaultTraserveService.OnModelHandlerResolve += ResolveDefaultModelHandler;
            //defaultTraserveService.Traverse(modelHost, model);

            ModelTraverseService.OnModelHandlerResolve += ResolveDefaultModelHandler;
            ModelTraverseService.Traverse(modelHost, model);

            if (Exceptions.Count > 0)
            {
                throw new SPMeta2ModelDeploymentException("Errors while validating the model",
                    new SPMeta2AggregateException(Exceptions.OfType<Exception>()));
            }
        }


        private ModelHandlerBase ResolveDefaultModelHandler(ModelNode arg)
        {
            return ModelHandler;
        }

        protected override ModelHandlerBase ResolveModelHandler(Type modelType)
        {
            return ModelHandler;
        }

        #endregion
    }
}
