using System;
using SPMeta2.Definitions;
using SPMeta2.ModelHosts;
using SPMeta2.Models;
using SPMeta2.Regression.SSOM.Validation;
using SPMeta2.Regression.Validation.ServerModelHandlers;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;

namespace SPMeta2.Regression.SSOM
{
    public class SSOMValidationService : ModelServiceBase
    {
        public SSOMValidationService()
        {
            RegisterModelHandlers<SSOMModelHandlerBase>(this, GetType().Assembly);
        }

        public override void DeployModel(ModelHostBase modelHost, ModelNode model)
        {
            if (!(modelHost is SSOMModelHostBase))
                throw new ArgumentException("modelHost for SSOM needs to be inherited from SSOMModelHostBase.");

            base.DeployModel(modelHost, model);
        }

        public override void RetractModel(ModelHostBase modelHost, ModelNode model)
        {
            if (!(modelHost is SSOMModelHostBase))
                throw new ArgumentException("model host for SSOM needs to be inherited from SSOMModelHostBase.");

            base.RetractModel(modelHost, model);
        }
    }
}
