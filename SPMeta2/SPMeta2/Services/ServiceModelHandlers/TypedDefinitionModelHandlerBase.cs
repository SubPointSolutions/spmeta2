using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;

namespace SPMeta2.Services.ServiceModelHandlers
{
    public abstract class TypedDefinitionModelHandlerBase<TDefinition> : ServiceModelHandlerBase
        where TDefinition : DefinitionBase
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            if (!(model is TDefinition))
                return;

            ProcessDefinition(modelHost, (TDefinition)model);
        }

        protected abstract void ProcessDefinition(object modelHost, TDefinition definitionBase);
    }
}
