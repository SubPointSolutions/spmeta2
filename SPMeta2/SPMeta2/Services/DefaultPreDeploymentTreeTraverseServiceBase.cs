using System;
using SPMeta2.ModelHandlers;
using SPMeta2.ModelHosts;
using SPMeta2.Models;
using SPMeta2.Services.Impl.Validation;

namespace SPMeta2.Services
{
    public abstract class DefaultPreDeploymentTreeTraverseServiceBase<TDefinitionHandler> : PreDeploymentValidationServiceBase
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
            var defaultTraserveService = ServiceContainer.Instance.GetService<ModelTreeTraverseServiceBase>();

            defaultTraserveService.OnModelHandlerResolve += ResolveDefaultModelHandler;
            defaultTraserveService.Traverse(modelHost, model);
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
