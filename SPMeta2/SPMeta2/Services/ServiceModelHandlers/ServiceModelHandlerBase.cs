using System;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;

namespace SPMeta2.Services.ServiceModelHandlers
{
    public abstract class ServiceModelHandlerBase : ModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(DefinitionBase); }
        }

        #endregion
    }
}
