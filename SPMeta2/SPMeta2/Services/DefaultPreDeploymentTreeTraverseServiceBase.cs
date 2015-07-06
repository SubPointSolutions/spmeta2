using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.ModelHandlers;
using SPMeta2.ModelHosts;
using SPMeta2.Models;

namespace SPMeta2.Services
{
    public class DefaultPreDeploymentTreeTraverseServiceBase<TDefinitionHandler> : PreDeploymentServiceBase
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

            defaultTraserveService.OnModelHandlerResolve += ResolveModelHandler;
            defaultTraserveService.Traverse(modelHost, model);
        }

        protected virtual ModelHandlerBase ResolveModelHandler(ModelNode modelNode)
        {
            return ModelHandler;
        }

        #endregion
    }
}
