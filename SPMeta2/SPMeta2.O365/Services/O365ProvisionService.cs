using SPMeta2.ModelHosts;
using SPMeta2.Models;
using SPMeta2.O365.ModelHandlers.Base;
using SPMeta2.O365.ModelHosts;
using SPMeta2.Services;
using SPMeta2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;


namespace SPMeta2.O365.Services
{
    public class O365ProvisionService : ModelServiceBase
    {
        #region constructors

        public O365ProvisionService()
        {
            RegisterModelHandlers();
        }

        #endregion

        #region methods

        private void RegisterModelHandlers()
        {
            ModelHandlers.Clear();

            var handlerTypes = ReflectionUtils.GetTypesFromAssembly<O365ModelHandlerBase>(Assembly.GetExecutingAssembly());

            foreach (var handlerType in handlerTypes)
            {
                var handlerInstance = Activator.CreateInstance(handlerType) as O365ModelHandlerBase;

                if (handlerInstance != null)
                {
                    if (!ModelHandlers.ContainsKey(handlerInstance.TargetType))
                        ModelHandlers.Add(handlerInstance.TargetType, handlerInstance);
                }
            }
        }

        public override void DeployModel(ModelHostBase modelHost, ModelNode model)
        {
            if (!(modelHost is O365ModelHostBase))
                throw new ArgumentException("modelHost for O365 needs to be inherited from O365ModelHostBase.");

            base.DeployModel(modelHost, model);
        }

        public override void RetractModel(ModelHostBase modelHost, ModelNode model)
        {
            if (!(modelHost is O365ModelHostBase))
                throw new ArgumentException("model host for O365 needs to be inherited from O365ModelHostBase.");

            base.RetractModel(modelHost, model);
        }

        #endregion
    }
}
